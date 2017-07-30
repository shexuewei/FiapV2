using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Eiap.NetFramework
{
    /// <summary>
    /// JSON序列化处理类
    /// </summary>
    public static class JsonSerializeProcess
    {
        /// <summary>
        /// 常用类型对象序列化处理
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="valueSb"></param>
        /// <param name="setting"></param>
        /// <param name="isShowPropertyName"></param>
        /// <param name="propertyName"></param>
        private static void Process(object objectValue, StringBuilder valueSb, SerializationSetting setting, Type objectValueType)
        {
            if (objectValue != null)
            {
                if (objectValueType == typeof(DateTime))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    DateTime objectValueDateTime = (DateTime)objectValue;
                    string objectValueDateTimeString = objectValueDateTime.ToString(setting.DataTimeFomatter);
                    valueSb.Append(objectValueDateTimeString);
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                }
                else if (objectValueType == typeof(Int32)
                    || objectValueType == typeof(Decimal)
                    || objectValueType == typeof(Guid))
                {
                    valueSb.Append(objectValue.ToString());
                }
                else if (objectValueType == typeof(String))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(objectValue.ToString());
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                }
                else if (objectValueType == typeof(Boolean))
                {
                    valueSb.Append(objectValue.ToString().ToLower());
                }
            }
            else
            {
                valueSb.Append(JsonSymbol.JsonNullSymbol);
            }
        }

        public static void Serialize(object serializeObject, SerializationSetting setting, StringBuilder valueSb, IMethodManager methodManager)
        {
            Type serializeObjectType = serializeObject.GetType();
            Stack<ObjectAndFlag> serializeObjectList = new Stack<ObjectAndFlag>();
            AddOrUpdateSerializeObjectList(serializeObject, valueSb, serializeObjectList, serializeObjectType);
            Stack<object> currentObjectList = new Stack<object>();
            while (serializeObjectList.Count > 0)
            {
                ObjectAndFlag currentObjectAndFlag = serializeObjectList.Pop();
                switch (currentObjectAndFlag.Flag)
                {
                    case ObjectFlag.Symbol:
                        valueSb.Append(currentObjectAndFlag.CurrentObject);
                        char currentSymbol = (char)currentObjectAndFlag.CurrentObject;
                        if (currentSymbol == JsonSymbol.JsonObjectSymbol_End)
                        {
                            currentObjectList.Pop();
                        }
                        break;
                    case ObjectFlag.Property:
                        PropertyInfo currentPropertyInfo = currentObjectAndFlag.CurrentObject as PropertyInfo;
                        valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                        valueSb.Append(currentPropertyInfo.Name);
                        valueSb.Append(JsonSymbol.JsonSerializePropertySymbol);
                        //object objectValue = currentPropertyInfo.GetGetMethod().Invoke(currentObjectList.Peek(), new object[] { });
                        object objectValue =  methodManager.MethodInvoke(currentObjectList.Peek(), new object[] { }, currentPropertyInfo.GetGetMethod(), currentPropertyInfo.DeclaringType.FullName);
                        if (objectValue == null || currentPropertyInfo.PropertyType.IsNormalType())
                        {
                            Process(objectValue, valueSb, setting, currentPropertyInfo.PropertyType);
                        }
                        else if (currentPropertyInfo.PropertyType.IsGenericType && (currentPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            Process(objectValue, valueSb, setting, currentPropertyInfo.PropertyType.GetGenericArguments()[0]);
                        }
                        else if (typeof(IDictionary).IsAssignableFrom(currentPropertyInfo.PropertyType))
                        {
                            currentObjectList.Push(currentObjectAndFlag.CurrentObject);
                            AddOrUpdateSerializeObjectList(objectValue, valueSb, serializeObjectList, currentPropertyInfo.PropertyType);
                        }
                        else
                        {
                            AddOrUpdateSerializeObjectList(objectValue, valueSb, serializeObjectList, currentPropertyInfo.PropertyType);
                        }
                        break;
                    case ObjectFlag.Object:
                        currentObjectList.Push(currentObjectAndFlag.CurrentObject);
                        PropertyInfo[] currentPropertyInfoList = currentObjectAndFlag.CurrentObject.GetType().GetProperties();
                        int tmpObjCount = 0;
                        foreach (PropertyInfo propertyInfoItem in currentPropertyInfoList)
                        {
                            tmpObjCount++;
                            if (tmpObjCount > 1)
                            {
                                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = ObjectFlag.Symbol });
                            }
                            serializeObjectList.Push(new ObjectAndFlag { CurrentObject = propertyInfoItem, Flag = ObjectFlag.Property });
                        }
                        break;
                    case ObjectFlag.Value:
                        Process(currentObjectAndFlag.CurrentObject, valueSb, setting, currentObjectAndFlag.CurrentObjectType);
                        break;
                    case ObjectFlag.Dictionary:
                        DictionaryEntry dicValue = (DictionaryEntry)currentObjectAndFlag.CurrentObject;
                        valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                        valueSb.Append(dicValue.Key.ToString());
                        valueSb.Append(JsonSymbol.JsonSerializePropertySymbol);
                        if (dicValue.Value == null || currentObjectAndFlag.CurrentObjectType.IsNormalType())
                        {
                            Process(dicValue.Value, valueSb, setting, currentObjectAndFlag.CurrentObjectType);
                        }
                        else if (currentObjectAndFlag.CurrentObjectType.IsGenericType && (currentObjectAndFlag.CurrentObjectType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            Process(dicValue.Value, valueSb, setting, currentObjectAndFlag.CurrentObjectType);
                        }
                        else
                        {
                            AddOrUpdateSerializeObjectList(dicValue.Value, valueSb, serializeObjectList, currentObjectAndFlag.CurrentObjectType);
                        }
                        break;
                }
            }
        }

        private static void AddOrUpdateSerializeObjectList(object serializeObject, StringBuilder valueSb, Stack<ObjectAndFlag> serializeObjectList, Type serializeObjectType)
        {
            if (serializeObjectType.IsNormalType())
            {
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = serializeObject, Flag = ObjectFlag.Value, CurrentObjectType = serializeObjectType });
            }
            else if (typeof(IEnumerable).IsAssignableFrom(serializeObjectType) && serializeObjectType != typeof(String) && !typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonArraySymbol_End, Flag = ObjectFlag.Symbol });
                IEnumerable objectValue = (IEnumerable)serializeObject;
                IEnumerator enumeratorList = objectValue.GetEnumerator();
                enumeratorList.Reset();
                int tmpObjCount = 0;
                Type enumeratorCurrentType = null;
                if (serializeObjectType.IsGenericType)
                {
                    enumeratorCurrentType = serializeObjectType.GetGenericArguments()[0];
                }
                else if (serializeObjectType.IsArray)
                {
                    enumeratorCurrentType = serializeObjectType.GetElementType();
                }
                if (enumeratorCurrentType.IsNormalType())
                {
                    while (enumeratorList.MoveNext())
                    {
                        tmpObjCount++;
                        if (tmpObjCount > 1)
                        {
                            serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = ObjectFlag.Symbol });
                        }
                        serializeObjectList.Push(new ObjectAndFlag { CurrentObject = enumeratorList.Current, Flag = ObjectFlag.Value, CurrentObjectType = enumeratorCurrentType });
                    }
                }
                else
                {
                    while (enumeratorList.MoveNext())
                    {
                        tmpObjCount++;
                        if (tmpObjCount > 1)
                        {
                            serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = ObjectFlag.Symbol });
                        }
                        serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonObjectSymbol_End, Flag = ObjectFlag.Symbol });
                        serializeObjectList.Push(new ObjectAndFlag { CurrentObject = enumeratorList.Current, Flag = ObjectFlag.Object, CurrentObjectType = enumeratorCurrentType });
                        serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonObjectSymbol_Begin, Flag = ObjectFlag.Symbol });
                    }
                }
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonArraySymbol_Begin, Flag = ObjectFlag.Symbol });
            }
            else if (typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonObjectSymbol_End, Flag = ObjectFlag.Symbol });
                IDictionary objectValue = (IDictionary)serializeObject;
                IDictionaryEnumerator enumeratorList = objectValue.GetEnumerator();
                int tmpObjCount = 0;
                while (enumeratorList.MoveNext())
                {
                    tmpObjCount++;
                    if (tmpObjCount > 1)
                    {
                        serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = ObjectFlag.Symbol });
                    }
                    serializeObjectList.Push(new ObjectAndFlag { CurrentObject = enumeratorList.Current, Flag = ObjectFlag.Dictionary, CurrentObjectType = serializeObjectType.GetGenericArguments()[1] });
                }
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonObjectSymbol_Begin, Flag = ObjectFlag.Symbol });
            }
            else
            {
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonObjectSymbol_End, Flag = ObjectFlag.Symbol });
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = serializeObject, Flag = ObjectFlag.Object, CurrentObjectType = serializeObjectType });
                serializeObjectList.Push(new ObjectAndFlag { CurrentObject = JsonSymbol.JsonObjectSymbol_Begin, Flag = ObjectFlag.Symbol });
            }
        }
    }

    public class ObjectAndFlag
    {
        public object CurrentObject { get; set; }
        public ObjectFlag Flag { get; set; }
        public Type CurrentObjectType { get; set; }
    }

    public enum ObjectFlag
    {
        Object,
        Property,
        Dictionary,
        Value,
        Symbol
    }
}
