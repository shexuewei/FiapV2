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
        /// <param name="objectValueType"></param>
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

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializeObject"></param>
        /// <param name="setting"></param>
        /// <param name="valueSb"></param>
        /// <param name="methodManager"></param>
        public static void Serialize(object serializeObject, SerializationSetting setting, StringBuilder valueSb, IMethodManager methodManager)
        {
            Type serializeObjectType = serializeObject.GetType();
            Stack<SerializeObjectContainer> serializeObjectList = new Stack<SerializeObjectContainer>(1024);
            Stack<SerializeObjectContainer> tmpSerializeObjectList = new Stack<SerializeObjectContainer>(1024);
            AddOrUpdateSerializeObjectList(serializeObject, serializeObjectList, serializeObjectType, tmpSerializeObjectList);
            Stack<object> currentObjectList = new Stack<object>(1024);
            while (serializeObjectList.Count > 0)
            {
                SerializeObjectContainer currentSerializeObjectContainer = serializeObjectList.Pop();
                switch (currentSerializeObjectContainer.Flag)
                {
                    case SerializeObjectFlag.Symbol:
                        valueSb.Append(currentSerializeObjectContainer.CurrentObject);
                        char currentSymbol = (char)currentSerializeObjectContainer.CurrentObject;
                        if (currentSymbol == JsonSymbol.JsonObjectSymbol_End)
                        {
                            currentObjectList.Pop();
                        }
                        break;
                    case SerializeObjectFlag.Property:
                        PropertyInfo currentPropertyInfo = currentSerializeObjectContainer.CurrentObject as PropertyInfo;
                        if (currentPropertyInfo.Name == "Dict")
                        { }
                        valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                        valueSb.Append(currentPropertyInfo.Name);
                        valueSb.Append(JsonSymbol.JsonSerializePropertySymbol);
                        //object objectValue = currentPropertyInfo.GetGetMethod().Invoke(currentObjectList.Peek(), new object[] { });
                        object objectValue =  methodManager.MethodInvoke(currentObjectList.Peek(), new object[] { }, currentPropertyInfo.GetGetMethod());
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
                            currentObjectList.Push(currentSerializeObjectContainer.CurrentObject);
                            AddOrUpdateSerializeObjectList(objectValue, serializeObjectList, currentPropertyInfo.PropertyType, tmpSerializeObjectList);
                        }
                        else
                        {
                            AddOrUpdateSerializeObjectList(objectValue, serializeObjectList, currentPropertyInfo.PropertyType, tmpSerializeObjectList);
                        }
                        break;
                    case SerializeObjectFlag.Object:
                        currentObjectList.Push(currentSerializeObjectContainer.CurrentObject);
                        PropertyInfo[] currentPropertyInfoList = currentSerializeObjectContainer.CurrentObject.GetType().GetProperties();
                        int tmpObjCount = 0;
                        foreach (PropertyInfo propertyInfoItem in currentPropertyInfoList)
                        {
                            tmpObjCount++;
                            if (tmpObjCount > 1)
                            {
                                tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = SerializeObjectFlag.Symbol });
                            }
                            tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = propertyInfoItem, Flag = SerializeObjectFlag.Property });
                        }
                        while (tmpSerializeObjectList.Count > 0)
                        {
                            serializeObjectList.Push(tmpSerializeObjectList.Pop());
                        }
                        break;
                    case SerializeObjectFlag.Value:
                        Process(currentSerializeObjectContainer.CurrentObject, valueSb, setting, currentSerializeObjectContainer.CurrentObjectType);
                        break;
                    case SerializeObjectFlag.Dictionary:
                        DictionaryEntry dicValue = (DictionaryEntry)currentSerializeObjectContainer.CurrentObject;
                        valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                        valueSb.Append(dicValue.Key.ToString());
                        valueSb.Append(JsonSymbol.JsonSerializePropertySymbol);
                        if (dicValue.Value == null || currentSerializeObjectContainer.CurrentObjectType.IsNormalType())
                        {
                            Process(dicValue.Value, valueSb, setting, currentSerializeObjectContainer.CurrentObjectType);
                        }
                        else if (currentSerializeObjectContainer.CurrentObjectType.IsGenericType && (currentSerializeObjectContainer.CurrentObjectType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            Process(dicValue.Value, valueSb, setting, currentSerializeObjectContainer.CurrentObjectType);
                        }
                        else
                        {
                            AddOrUpdateSerializeObjectList(dicValue.Value, serializeObjectList, currentSerializeObjectContainer.CurrentObjectType, tmpSerializeObjectList);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 序列化对象列表
        /// </summary>
        /// <param name="serializeObject"></param>
        /// <param name="valueSb"></param>
        /// <param name="serializeObjectList"></param>
        /// <param name="serializeObjectType"></param>
        private static void AddOrUpdateSerializeObjectList(object serializeObject, Stack<SerializeObjectContainer> serializeObjectList, Type serializeObjectType, Stack<SerializeObjectContainer> tmpSerializeObjectList)
        {
            if (serializeObjectType.IsNormalType())
            {
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = serializeObject, Flag = SerializeObjectFlag.Value, CurrentObjectType = serializeObjectType });
            }
            else if (typeof(IEnumerable).IsAssignableFrom(serializeObjectType) && serializeObjectType != typeof(String) && !typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonArraySymbol_End, Flag = SerializeObjectFlag.Symbol });
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
                            tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = SerializeObjectFlag.Symbol });
                        }
                        tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = enumeratorList.Current, Flag = SerializeObjectFlag.Value, CurrentObjectType = enumeratorCurrentType });
                    }
                    while (tmpSerializeObjectList.Count > 0)
                    {
                        serializeObjectList.Push(tmpSerializeObjectList.Pop());
                    }
                }
                else
                {
                    while (enumeratorList.MoveNext())
                    {
                        tmpObjCount++;
                        if (tmpObjCount > 1)
                        {
                            tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = SerializeObjectFlag.Symbol });
                        }
                        tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonObjectSymbol_Begin, Flag = SerializeObjectFlag.Symbol });
                        tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = enumeratorList.Current, Flag = SerializeObjectFlag.Object, CurrentObjectType = enumeratorCurrentType });
                        tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonObjectSymbol_End, Flag = SerializeObjectFlag.Symbol }); 
                    }
                    while (tmpSerializeObjectList.Count > 0)
                    {
                        serializeObjectList.Push(tmpSerializeObjectList.Pop());
                    }
                }
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonArraySymbol_Begin, Flag = SerializeObjectFlag.Symbol });
            }
            else if (typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonObjectSymbol_End, Flag = SerializeObjectFlag.Symbol });
                IDictionary objectValue = (IDictionary)serializeObject;
                IDictionaryEnumerator enumeratorList = objectValue.GetEnumerator();
                int tmpObjCount = 0;
                while (enumeratorList.MoveNext())
                {
                    tmpObjCount++;
                    if (tmpObjCount > 1)
                    {
                        tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonSeparateSymbol, Flag = SerializeObjectFlag.Symbol });
                    }
                    tmpSerializeObjectList.Push(new SerializeObjectContainer { CurrentObject = enumeratorList.Current, Flag = SerializeObjectFlag.Dictionary, CurrentObjectType = serializeObjectType.GetGenericArguments()[1] });
                }
                while (tmpSerializeObjectList.Count > 0)
                {
                    serializeObjectList.Push(tmpSerializeObjectList.Pop());
                }
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonObjectSymbol_Begin, Flag = SerializeObjectFlag.Symbol });
            }
            else
            {
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonObjectSymbol_End, Flag = SerializeObjectFlag.Symbol });
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = serializeObject, Flag = SerializeObjectFlag.Object, CurrentObjectType = serializeObjectType });
                serializeObjectList.Push(new SerializeObjectContainer { CurrentObject = JsonSymbol.JsonObjectSymbol_Begin, Flag = SerializeObjectFlag.Symbol });
            }
        }
    }
}
