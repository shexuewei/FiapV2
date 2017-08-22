using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Eiap.NetFramework
{
    /// <summary>
    /// JSON反序列化处理类
    /// </summary>
    public static class JsonDeserializeProcess
    {
        public static event EventHandler<JsonDeserializeEventArgs> JsonDeserializeArraySymbol_Begin_Event;
        public static event EventHandler<JsonDeserializeEventArgs> JsonDeserializeArraySymbol_End_Event;
        public static event EventHandler<JsonDeserializeEventArgs> JsonDeserializeObjectSymbol_Begin_Event;
        public static event EventHandler<JsonDeserializeEventArgs> JsonDeserializeObjectSymbol_End_Event;
        public static event EventHandler<JsonDeserializeEventArgs> JsonDeserializePropertySymbol_Event;
        public static event EventHandler<JsonDeserializeEventArgs> JsonDeserializeSeparateSymbol_Event;

        public static object Deserialize(string jsonString, Type objectType, SerializationSetting setting, IMethodManager methodManager)
        {
            Stack<char> jsonStringStack = new Stack<char>();
            Stack<DeserializeObjectContainer> containerStack = new Stack<DeserializeObjectContainer>();
            JsonDeserializeEventArgs args = new JsonDeserializeEventArgs { RootType = objectType, ContainerStack = containerStack, JsonStringStack = jsonStringStack, MethodManager = methodManager };
            char[] jsonCharList = jsonString.ToCharArray();
            foreach (char charitem in jsonCharList)
            {
                jsonStringStack.Push(charitem);
                args.CurrentCharItem = charitem;
                //数组开始
                if (charitem == JsonSymbol.JsonArraySymbol_Begin)
                {
                    if (JsonDeserializeArraySymbol_Begin_Event != null)
                    {
                        JsonDeserializeArraySymbol_Begin_Event(null, args);
                    }
                }
                //数组结束
                else if (charitem == JsonSymbol.JsonArraySymbol_End)
                {
                    if (JsonDeserializeArraySymbol_End_Event != null)
                    {
                        JsonDeserializeArraySymbol_End_Event(null, args);
                    }
                }
                //对象开始
                else if (charitem == JsonSymbol.JsonObjectSymbol_Begin)
                {
                    if (JsonDeserializeObjectSymbol_Begin_Event != null)
                    {
                        JsonDeserializeObjectSymbol_Begin_Event(null, args);
                    }
                }
                //属性名
                else if (charitem == JsonSymbol.JsonPropertySymbol && IsPropertyHandler(args.JsonStringStack))
                {
                    if (JsonDeserializePropertySymbol_Event != null)
                    {
                        JsonDeserializePropertySymbol_Event(null, args);
                    }
                }
                //对象结束
                else if (charitem == JsonSymbol.JsonObjectSymbol_End)
                {
                    if (JsonDeserializeObjectSymbol_End_Event != null)
                    {
                        JsonDeserializeObjectSymbol_End_Event(null, args);
                    }
                }
                //逗号
                else if (charitem == JsonSymbol.JsonSeparateSymbol)
                {
                    if (JsonDeserializeSeparateSymbol_Event != null)
                    {
                        JsonDeserializeSeparateSymbol_Event(null, args);
                    }
                }
            }
            return containerStack.Pop().ContainerObject;
        }

        /// <summary>
        /// 数组开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void JsonDeserializeProcess_JsonDeserializeArraySymbol_Begin_Event(object sender, JsonDeserializeEventArgs e)
        {
            e.JsonStringStack.Pop();//[出栈
            Type currentObjectType = null;
            if (e.ContainerStack.Count() == 0)
            {
                currentObjectType = e.RootType;
            }
            else
            {
                DeserializeObjectContainer container = e.ContainerStack.Peek();
                if (container.ContainerType == DeserializeObjectContainerType.Property)
                {
                    PropertyInfo currentPropertyInfo = container.ContainerObject as PropertyInfo;
                    if (currentPropertyInfo != null)
                    {
                        currentObjectType = currentPropertyInfo.PropertyType;
                    }
                }
            }
            IList objectInstance = null;
            if (currentObjectType.IsGenericType)
            {
                Type genType = currentObjectType.GetGenericTypeDefinition();
                Type[] genParaType = currentObjectType.GetGenericArguments();
                Type objtype = typeof(List<>).MakeGenericType(genParaType);
                objectInstance = Activator.CreateInstance(objtype) as IList;
            }
            else if (currentObjectType.IsArray)
            {
                Type arrayElementType = currentObjectType.GetElementType();
                Type[] genParaType = new Type[] { arrayElementType };
                Type objtype = typeof(List<>).MakeGenericType(genParaType);
                objectInstance = Activator.CreateInstance(objtype) as IList;
            }
            else
            {
                objectInstance = Activator.CreateInstance(currentObjectType) as IList;
            }
            e.ContainerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.List, ContainerObject = objectInstance });
        }

        /// <summary>
        /// 数组结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void JsonDeserializeProcess_JsonDeserializeArraySymbol_End_Event(object sender, JsonDeserializeEventArgs e)
        {
            e.JsonStringStack.Pop();//]出栈
            DeserializeObjectContainer currentObjectContainer = e.ContainerStack.Pop();
            object objvalue = null;
            IList listvalue = null;
            if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Object)
            {
                objvalue = currentObjectContainer.ContainerObject;
                currentObjectContainer = e.ContainerStack.Peek();
                if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.List)
                {
                    listvalue = currentObjectContainer.ContainerObject as IList;
                    listvalue.Add(objvalue);
                }
            }
            else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.List)
            {
                string valuestring = GetValueContainerByPropertyType(e.JsonStringStack);
                listvalue = currentObjectContainer.ContainerObject as IList;
                listvalue.Add(valuestring);
                e.ContainerStack.Push(currentObjectContainer);
            }

        }

        /// <summary>
        /// 对象开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void JsonDeserializeProcess_JsonDeserializeObjectSymbol_Begin_Event(object sender, JsonDeserializeEventArgs e)
        {
            Type currentObjectType = null;
            if (e.ContainerStack.Count() == 0)
            {
                currentObjectType = e.RootType;
            }
            else
            {
                DeserializeObjectContainer container = e.ContainerStack.Peek();
                if (container.ContainerType == DeserializeObjectContainerType.Property)
                {
                    PropertyInfo currentPropertyInfo = container.ContainerObject as PropertyInfo;
                    if (currentPropertyInfo != null)
                    {
                        currentObjectType = currentPropertyInfo.PropertyType;
                    }
                }
                else if (container.ContainerType == DeserializeObjectContainerType.List)
                {
                    currentObjectType = container.ContainerObject.GetType().GetGenericArguments()[0];
                }
                else if (container.ContainerType == DeserializeObjectContainerType.DictionaryKey)
                {
                    currentObjectType = container.ContainerObject.GetType().GetGenericArguments()[1];
                }
            }
            object objectInstance = Activator.CreateInstance(currentObjectType);
            e.ContainerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.Object, ContainerObject = objectInstance });
            e.JsonStringStack.Pop();
        }

        /// <summary>
        /// 对象结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void JsonDeserializeProcess_JsonDeserializeObjectSymbol_End_Event(object sender, JsonDeserializeEventArgs e)
        {
            e.JsonStringStack.Pop();//}出栈
            DeserializeObjectContainer currentObjectContainer = e.ContainerStack.Pop();
            PropertyInfo currentPropertyInfo = null;
            object objvalue = null;

            if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Object
                || currentObjectContainer.ContainerType == DeserializeObjectContainerType.List)
            {
                objvalue = currentObjectContainer.ContainerObject;
                currentObjectContainer = e.ContainerStack.Pop();
                currentPropertyInfo = currentObjectContainer.ContainerObject as PropertyInfo;
                if (currentPropertyInfo != null)
                {
                    PropertySetValue(e.ContainerStack.Peek().ContainerObject, currentPropertyInfo, objvalue, e.MethodManager, e.ContainerStack.Peek().ContainerObjectTypeName);
                }
            }
            else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.DictionaryKey)
            {
                IDictionary dic = e.ContainerStack.Peek().ContainerObject as IDictionary;
                string valuestring = GetValueContainerByPropertyType(e.JsonStringStack);
                dic.Add(currentObjectContainer.ContainerObject, valuestring);
            }
            else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Property)
            {
                Type currentPropertyType = null;
                currentPropertyInfo = currentObjectContainer.ContainerObject as PropertyInfo;
                if (currentPropertyInfo.PropertyType.IsGenericType)
                {
                    currentPropertyType = currentPropertyInfo.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    currentPropertyType = currentPropertyInfo.PropertyType;
                }
                string valuestring = GetValueContainerByPropertyType(e.JsonStringStack);
                if (currentPropertyType == typeof(int))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = int.Parse(valuestring);
                    }
                }
                else if (currentPropertyType == typeof(string))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = valuestring;
                    }
                }
                else if (currentPropertyType == typeof(DateTime))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = DateTime.Parse(valuestring);
                    }
                }
                else if (currentPropertyType == typeof(decimal))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = Decimal.Parse(valuestring);
                    }
                }
                else if (currentPropertyType == typeof(bool))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = bool.Parse(valuestring);
                    }
                }
                PropertySetValue(e.ContainerStack.Peek().ContainerObject, currentPropertyInfo, objvalue, e.MethodManager, currentObjectContainer.ContainerObjectTypeName);
            }
        }

        private static string GetValueContainerByPropertyType(Stack<char> jsonStringStack)
        {
            List<char> valuestring = new List<char>();
            int count = -1;
            while (jsonStringStack.Count > 0)
            {
                count++;
                char valueSymbol = jsonStringStack.Pop();
                if ((count == 0 && valueSymbol == JsonSymbol.JsonQuotesSymbol)
                    || (jsonStringStack.Count == 0 && valueSymbol == JsonSymbol.JsonQuotesSymbol))
                {
                    continue;
                }
                else
                {
                    valuestring.Add(valueSymbol);
                }
            }
            valuestring.Reverse();
            return new string(valuestring.ToArray());
        }

        /// <summary>
        /// 属性事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void JsonDeserializeProcess_JsonDeserializePropertySymbol_Event(object sender, JsonDeserializeEventArgs e)
        {
            e.JsonStringStack.Pop();//属性分隔符出栈
            List<char> propertyNameList = new List<char>();

            //属性引号出栈
            while (true)
            {
                char beginQuotes = e.JsonStringStack.Pop();
                if (beginQuotes == JsonSymbol.JsonQuotesSymbol)
                {
                    break;
                }
            }
            //属性引号出栈
            while (true)
            {
                char propertyNameChar = e.JsonStringStack.Pop();
                if (propertyNameChar == JsonSymbol.JsonQuotesSymbol)
                {
                    break;
                }
                else if (propertyNameChar != JsonSymbol.JsonSpaceSymbol)
                {
                    propertyNameList.Add(propertyNameChar);
                }
            }
            propertyNameList.Reverse();
            string propertyNameStr = new string(propertyNameList.ToArray());
            DeserializeObjectContainer currentObj = e.ContainerStack.Peek() as DeserializeObjectContainer;
            if (currentObj != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(currentObj.ContainerObject.GetType()))
                {
                    e.ContainerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.DictionaryKey, ContainerObject = propertyNameStr });
                }
                else
                {
                    Type currentObjectType = GetCurrentObject(e.ContainerStack).ContainerObject.GetType();
                    string currentObjectTypeName = currentObjectType.FullName;
                    PropertyInfo propertyinfo = currentObjectType.GetProperty(propertyNameStr);
                    e.ContainerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.Property, ContainerObject = propertyinfo, ContainerObjectTypeName = currentObjectTypeName });
                }
            }
        }

        /// <summary>
        /// 逗号事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void JsonDeserializeProcess_JsonDeserializeSeparateSymbol_Event(object sender, JsonDeserializeEventArgs e)
        {
            e.JsonStringStack.Pop();//,出栈
            DeserializeObjectContainer currentObjectContainer = e.ContainerStack.Pop();
            PropertyInfo currentPropertyInfo = null;
            object objvalue = null;
            IList listvalue = null;
            if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.List)
            {
                listvalue = currentObjectContainer.ContainerObject as IList;
                if (e.JsonStringStack.Count > 0)
                {
                    string valuestring = GetValueContainerByPropertyType(e.JsonStringStack);
                    listvalue.Add(valuestring);
                    e.ContainerStack.Push(currentObjectContainer);
                }
                else
                {
                    objvalue = IListToArray(listvalue);
                    currentObjectContainer = e.ContainerStack.Pop();
                    if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Property)
                    {
                        currentPropertyInfo = currentObjectContainer.ContainerObject as PropertyInfo;
                        PropertySetValue(e.ContainerStack.Peek().ContainerObject, currentPropertyInfo, objvalue, e.MethodManager, e.ContainerStack.Peek().ContainerObjectTypeName);
                    }
                }
            }
            else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.DictionaryKey)
            {
                IDictionary dic = e.ContainerStack.Peek().ContainerObject as IDictionary;
                string valuestring = GetValueContainerByPropertyType(e.JsonStringStack);
                dic.Add(currentObjectContainer.ContainerObject, valuestring);
            }
            else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Object)
            {
                objvalue = currentObjectContainer.ContainerObject;
                currentObjectContainer = e.ContainerStack.Pop();
                if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Property)
                {
                    currentPropertyInfo = currentObjectContainer.ContainerObject as PropertyInfo;
                    PropertySetValue(e.ContainerStack.Peek().ContainerObject, currentPropertyInfo, objvalue, e.MethodManager, e.ContainerStack.Peek().ContainerObjectTypeName);
                }
                else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.List)
                {
                    listvalue = currentObjectContainer.ContainerObject as IList;
                    listvalue.Add(objvalue);
                    e.ContainerStack.Push(currentObjectContainer);
                }
            }
            else if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Property)
            {
                currentPropertyInfo = currentObjectContainer.ContainerObject as PropertyInfo;
                Type currentPropertyType = null;
                if (currentPropertyInfo.PropertyType.IsGenericType)
                {
                    currentPropertyType = currentPropertyInfo.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    currentPropertyType = currentPropertyInfo.PropertyType;
                }
                string valuestring = GetValueContainerByPropertyType(e.JsonStringStack);
                if (currentPropertyType == typeof(int))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = int.Parse(valuestring);
                    }
                }
                else if (currentPropertyType == typeof(string))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = valuestring;
                    }
                }
                else if (currentPropertyType == typeof(DateTime))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = DateTime.Parse(valuestring);
                    }
                }
                else if (currentPropertyType == typeof(decimal))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = Decimal.Parse(valuestring);
                    }
                }
                else if (currentPropertyType == typeof(bool))
                {
                    if (valuestring != JsonSymbol.JsonNullSymbol)
                    {
                        objvalue = bool.Parse(valuestring);
                    }
                }
                PropertySetValue(e.ContainerStack.Peek().ContainerObject, currentPropertyInfo, objvalue, e.MethodManager, currentObjectContainer.ContainerObjectTypeName);
            }
        }

        private static DeserializeObjectContainer GetCurrentObject(Stack<DeserializeObjectContainer> containerStack)
        {
            DeserializeObjectContainer currentObjectContainer = null;
            Stack<DeserializeObjectContainer> tmpDeserializeObjectContainerStack = new Stack<DeserializeObjectContainer>();
            while (true)
            {
                currentObjectContainer = containerStack.Pop();
                tmpDeserializeObjectContainerStack.Push(currentObjectContainer);
                if (currentObjectContainer.ContainerType == DeserializeObjectContainerType.Object)
                {
                    break;
                }
            }
            while (tmpDeserializeObjectContainerStack.Count() > 0)
            {
                containerStack.Push(tmpDeserializeObjectContainerStack.Pop());
            }
            return currentObjectContainer;
        }

        private static bool IsPropertyHandler(Stack<char> jsonStringStack)
        {
            bool res = false;
            bool isQuotesSymbol = false;
            Stack<char> charList = new Stack<char>();
            while (true)
            {
                var currentChar = jsonStringStack.Pop();
                charList.Push(currentChar);
                if (currentChar != JsonSymbol.JsonPropertySymbol && currentChar != JsonSymbol.JsonQuotesSymbol && currentChar != JsonSymbol.JsonSpaceSymbol)
                {
                    if (isQuotesSymbol)
                    {
                        res = true;
                    }
                    break;
                }
                else if (currentChar == JsonSymbol.JsonQuotesSymbol)
                {
                    isQuotesSymbol = true;
                }
            }
            while (charList.Count > 0)
            {
                jsonStringStack.Push(charList.Pop());
            }
            return res;
        }

        private static object IListToArray(IList list)
        {
            object arrayObj = null;
            Type listType = list.GetType().GetGenericArguments()[0];
            IEnumerator enumeratorList = list.GetEnumerator();
            int currentListIndex = 0;
            if (listType == typeof(int))
            {
                int[] tmpIntArray = new int[list.Count];
                while (enumeratorList.MoveNext())
                {
                    tmpIntArray[currentListIndex] = Convert.ToInt32(enumeratorList.Current);
                    currentListIndex++;
                }
                arrayObj = tmpIntArray;
            }
            else if (listType == typeof(string))
            {
                string[] tmpStringArray = new string[list.Count];
                while (enumeratorList.MoveNext())
                {
                    tmpStringArray[currentListIndex] = Convert.ToString(enumeratorList.Current);
                    currentListIndex++;
                }
                arrayObj = tmpStringArray;
            }
            else if (listType == typeof(DateTime))
            {
                DateTime[] tmpDTArray = new DateTime[list.Count];
                while (enumeratorList.MoveNext())
                {
                    tmpDTArray[currentListIndex] = Convert.ToDateTime(enumeratorList.Current);
                    currentListIndex++;
                }
                arrayObj = tmpDTArray;
            }
            else if (listType == typeof(decimal))
            {
                decimal[] tmpDecimalArray = new decimal[list.Count];
                while (enumeratorList.MoveNext())
                {
                    tmpDecimalArray[currentListIndex] = Convert.ToDecimal(enumeratorList.Current);
                    currentListIndex++;
                }
                arrayObj = tmpDecimalArray;
            }
            else if (listType == typeof(bool))
            {
                bool[] tmpBoolArray = new bool[list.Count];
                while (enumeratorList.MoveNext())
                {
                    tmpBoolArray[currentListIndex] = Convert.ToBoolean(enumeratorList.Current);
                    currentListIndex++;
                }
                arrayObj = tmpBoolArray;
            }
            else
            {
                arrayObj = list;
            }
            return arrayObj;
        }

        private static void PropertySetValue(object instanceObj, PropertyInfo currentPropertyInfo, object objvalue, IMethodManager methodManager, string instanceTypeName = null)
        {
            methodManager.MethodInvoke(instanceObj, new object[] { objvalue }, currentPropertyInfo.GetSetMethod(), instanceTypeName);
        }

        public static object Deserialize2(string jsonString, Type objectType, SerializationSetting setting, IMethodManager methodManager)
        {
            Stack<char> jsonStringStack = new Stack<char>();
            Stack<DeserializeObjectContainer> containerStack = new Stack<DeserializeObjectContainer>();
            char[] jsonCharList = jsonString.ToCharArray();
            foreach (char charitem in jsonCharList)
            {
                jsonStringStack.Push(charitem);
                //数组开始
                if (charitem == JsonSymbol.JsonArraySymbol_Begin)
                {
                    jsonStringStack.Pop();//[出栈
                    Type currentObjectType = null;
                    if (containerStack.Count() == 0)
                    {
                        currentObjectType = objectType;
                    }
                    else
                    {
                        DeserializeObjectContainer container = containerStack.Peek();
                        if (container.ContainerType == DeserializeObjectContainerType.Property)
                        {
                            PropertyInfo currentPropertyInfo = container.ContainerObject as PropertyInfo;
                            if (currentPropertyInfo != null)
                            {
                                currentObjectType = currentPropertyInfo.PropertyType;
                            }
                        }
                    }
                    IList objectInstance = null;
                    string typeName = null;
                    if (currentObjectType.IsGenericType && !typeof(IDictionary).IsAssignableFrom(currentObjectType))
                    {
                        Type genType = currentObjectType.GetGenericTypeDefinition();
                        Type[] genParaType = currentObjectType.GetGenericArguments();
                        typeName = genParaType[0].FullName;
                        Type objtype = typeof(List<>).MakeGenericType(genParaType);
                        objectInstance = Activator.CreateInstance(objtype) as IList;
                    }
                    else if (currentObjectType.IsArray)
                    {
                        Type arrayElementType = currentObjectType.GetElementType();
                        typeName = arrayElementType.FullName;
                        Type[] genParaType = new Type[] { arrayElementType };
                        Type objtype = typeof(List<>).MakeGenericType(genParaType);
                        objectInstance = Activator.CreateInstance(objtype) as IList;
                    }
                    else if (typeof(IDictionary).IsAssignableFrom(currentObjectType))
                    {
                        //TODO:字典类型处理
                        objectInstance = Activator.CreateInstance(currentObjectType) as IList;
                    }
                    else
                    {
                        objectInstance = Activator.CreateInstance(currentObjectType) as IList;
                    }
                    containerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.List, ContainerObject = objectInstance, ContainerObjectTypeName = typeName });
                }
                //对象开始
                else if (charitem == JsonSymbol.JsonObjectSymbol_Begin)
                {
                    Type currentObjectType = null;
                    if (containerStack.Count() == 0)
                    {
                        currentObjectType = objectType;
                    }
                    else
                    {
                        DeserializeObjectContainer container = containerStack.Peek();
                        if (container.ContainerType == DeserializeObjectContainerType.Property)
                        {
                            PropertyInfo currentPropertyInfo = container.ContainerObject as PropertyInfo;
                            if (currentPropertyInfo != null)
                            {
                                currentObjectType = currentPropertyInfo.PropertyType;
                            }
                        }
                        else if (container.ContainerType == DeserializeObjectContainerType.List)
                        {
                            currentObjectType = container.ContainerObject.GetType().GetGenericArguments()[0];
                        }
                        else if (container.ContainerType == DeserializeObjectContainerType.DictionaryKey)
                        {
                            //TODO:字段类型待处理
                        }
                    }
                    object objectInstance = Activator.CreateInstance(currentObjectType);
                    containerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.Object, ContainerObject = objectInstance, ContainerObjectTypeName = currentObjectType.FullName });
                    jsonStringStack.Pop();
                }
                //属性名
                else if (charitem == JsonSymbol.JsonPropertySymbol && IsPropertyHandler(jsonStringStack))
                {
                    jsonStringStack.Pop();//属性分隔符出栈
                    Stack<char> propertyNameList = new Stack<char>(); 

                    //属性引号出栈
                    while (true)
                    {
                        char beginQuotes = jsonStringStack.Pop();
                        if (beginQuotes == JsonSymbol.JsonQuotesSymbol)
                        {
                            break;
                        }
                    }
                    //属性引号出栈
                    while (true)
                    {
                        char propertyNameChar = jsonStringStack.Pop();
                        if (propertyNameChar == JsonSymbol.JsonQuotesSymbol)
                        {
                            break;
                        }
                        else if (propertyNameChar != JsonSymbol.JsonSpaceSymbol)
                        {
                            propertyNameList.Push(propertyNameChar);
                        }
                    }
                    string propertyNameStr = new string(propertyNameList.ToArray());
                    DeserializeObjectContainer currentObj = containerStack.Peek() as DeserializeObjectContainer;
                    if (currentObj != null)
                    {
                        if (typeof(IDictionary).IsAssignableFrom(currentObj.ContainerObject.GetType()))
                        {
                            containerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.DictionaryKey, ContainerObject = propertyNameStr });
                        }
                        else
                        {
                            DeserializeObjectContainer currentObjectContainer = GetCurrentObject(containerStack);
                            Type currentObjectType = currentObjectContainer.ContainerObject.GetType();
                            string currentObjectTypeName = currentObjectContainer.ContainerObjectTypeName;
                            PropertyInfo propertyinfo = currentObjectType.GetProperty(propertyNameStr);
                            containerStack.Push(new DeserializeObjectContainer { ContainerType = DeserializeObjectContainerType.Property, ContainerObject = propertyinfo, ContainerObjectTypeName = currentObjectTypeName });
                        }
                    }
                }
                //逗号
                else if (charitem == JsonSymbol.JsonSeparateSymbol)
                {

                }
                //对象结束
                else if (charitem == JsonSymbol.JsonObjectSymbol_End)
                {

                }
                //数组结束
                else if (charitem == JsonSymbol.JsonArraySymbol_End)
                {
                    
                }
            }
            return null;
        }
    }
}
