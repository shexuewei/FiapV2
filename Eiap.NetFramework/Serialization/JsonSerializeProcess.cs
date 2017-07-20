using System;
using System.Collections;
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
        /// 序列化成JSON字符串
        /// </summary>
        /// <param name="serializeObject"></param>
        /// <param name="setting"></param>
        /// <param name="isShowPropertyName"></param>
        /// <param name="valueSb"></param>
        /// <param name="propertyAccessorManager"></param>
        /// <param name="propertyName"></param>
        public static void Serialize(object serializeObject, SerializationSetting setting, bool isShowPropertyName, StringBuilder valueSb, IMethodManager methodManager, string propertyName = "")
        {
            if (serializeObject == null)
            {
                if (isShowPropertyName && !string.IsNullOrWhiteSpace(propertyName))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(propertyName);
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(JsonSymbol.JsonPropertySymbol);
                    valueSb.Append(JsonSymbol.JsonNullSymbol);
                }
                else
                {
                    valueSb.Append(JsonSymbol.JsonNullSymbol);
                }
                return;
            }
            Type serializeObjectType = serializeObject.GetType();
            if (typeof(IEnumerable).IsAssignableFrom(serializeObjectType) && serializeObjectType != typeof(String) && !typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                IEnumerable objectValue = (IEnumerable)serializeObject;
                IEnumerator enumeratorList = objectValue.GetEnumerator();
                int objCount = enumeratorList.GetEnumeratorCount();
                enumeratorList.Reset();
                int tmpObjCount = 0;
                if (isShowPropertyName && !string.IsNullOrWhiteSpace(propertyName))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(propertyName);
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(JsonSymbol.JsonPropertySymbol);
                }
                valueSb.Append(JsonSymbol.JsonArraySymbol_Begin);
                while (enumeratorList.MoveNext())
                {
                    Type enumeratorCurrentType = enumeratorList.Current.GetType();
                    if (enumeratorCurrentType.IsNormalType())
                    {
                        Serialize(enumeratorList.Current, setting, false, valueSb, methodManager);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(propertyName))
                        {
                            Serialize(enumeratorList.Current, setting, false, valueSb, methodManager);
                        }
                        else
                        {
                            Serialize(enumeratorList.Current, setting, true, valueSb, methodManager);
                        }
                    }
                    tmpObjCount++;
                    if (tmpObjCount < objCount)
                    {
                        valueSb.Append(JsonSymbol.JsonSeparateSymbol);
                    }
                }
                valueSb.Append(JsonSymbol.JsonArraySymbol_End);

            }
            else if (typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                IDictionary objectValue = (IDictionary)serializeObject;
                int objCount = objectValue.Count;
                ICollection keyList = objectValue.Keys;
                IEnumerator enumeratorList = keyList.GetEnumerator();
                int tmpObjCount = 0;
                if (isShowPropertyName && !string.IsNullOrWhiteSpace(propertyName))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(propertyName);
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(JsonSymbol.JsonPropertySymbol);
                }
                valueSb.Append(JsonSymbol.JsonObjectSymbol_Begin);
                while (enumeratorList.MoveNext())
                {
                    if (enumeratorList.Current == null)
                    {
                        throw new Exception("Key Is Not Null");
                    }
                    Type enumeratorCurrentType = objectValue[enumeratorList.Current].GetType();
                    Serialize(objectValue[enumeratorList.Current], setting, true, valueSb, methodManager, enumeratorList.Current.ToString());
                    tmpObjCount++;
                    if (tmpObjCount < objCount)
                    {
                        valueSb.Append(JsonSymbol.JsonSeparateSymbol);
                    }
                }
                valueSb.Append(JsonSymbol.JsonObjectSymbol_End);
            }
            else if (serializeObjectType.IsNormalType())
            {
                Process(serializeObject, valueSb, setting, isShowPropertyName, propertyName);
            }
            else
            {
                if (isShowPropertyName && !string.IsNullOrWhiteSpace(propertyName))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(propertyName);
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(JsonSymbol.JsonPropertySymbol);
                }
                valueSb.Append(JsonSymbol.JsonObjectSymbol_Begin);
                PropertyInfo[] propertyInfoList = serializeObject.GetType().GetProperties();
                int propertyCount = propertyInfoList.Length;
                int propertyIndex = 0;
                foreach (PropertyInfo propertyInfoItem in propertyInfoList)
                {
                    object objectValue = methodManager.MethodInvoke(serializeObject, new object[] { }, propertyInfoItem.GetGetMethod());
                    Serialize(objectValue, setting, true, valueSb, methodManager, propertyInfoItem.Name);
                    propertyIndex++;
                    if (propertyIndex < propertyCount)
                    {
                        valueSb.Append(JsonSymbol.JsonSeparateSymbol);
                    }
                }
                valueSb.Append(JsonSymbol.JsonObjectSymbol_End);
            }
        }

        /// <summary>
        /// 常用类型对象序列化处理
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="valueSb"></param>
        /// <param name="setting"></param>
        /// <param name="isShowPropertyName"></param>
        /// <param name="propertyName"></param>
        private static void Process(object objectValue, StringBuilder valueSb, SerializationSetting setting, bool isShowPropertyName, string propertyName = "")
        {
            Type objectType = objectValue.GetType();
            if (isShowPropertyName)
            {
                valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                valueSb.Append(propertyName);
                valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                valueSb.Append(JsonSymbol.JsonPropertySymbol);
            }
            if (objectValue != null)
            {
                if (objectType == typeof(DateTime))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(Convert.ToDateTime(objectValue).ToString(setting.DataTimeFomatter));
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                }
                else if (objectType == typeof(Int32)
                    || objectType == typeof(Decimal)
                    || objectType == typeof(Guid))
                {
                    valueSb.Append(objectValue.ToString());
                }
                else if (objectType == typeof(String))
                {
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                    valueSb.Append(objectValue.ToString());
                    valueSb.Append(JsonSymbol.JsonQuotesSymbol);
                }
                else if (objectType == typeof(Boolean))
                {
                    valueSb.Append(objectValue.ToString().ToLower());
                }
            }
            else
            {
                valueSb.Append(JsonSymbol.JsonNullSymbol);
            }
        }
    }
}
