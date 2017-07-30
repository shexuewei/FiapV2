using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eiap.NetFramework
{
    public class SerializeContainerManager : ISerializeContainerManager
    {
        private ConcurrentDictionary<string, LinkedList<SerializeObjectContainer>> _SerializeObjectList = new ConcurrentDictionary<string, LinkedList<SerializeObjectContainer>>();

        public LinkedList<SerializeObjectContainer> GetOrAddSerializeObject(Type serializeObjectType)
        {
            string serializeObjectFullName = serializeObjectType.FullName;
            LinkedList<SerializeObjectContainer> serializeObjectStack = null;
            if (_SerializeObjectList.ContainsKey(serializeObjectFullName))
            {
                serializeObjectStack = _SerializeObjectList[serializeObjectFullName];
            }
            else
            {
                serializeObjectStack = new LinkedList<SerializeObjectContainer>();
                CreateSerializeObjectStack(serializeObjectType, serializeObjectStack);
                _SerializeObjectList.TryAdd(serializeObjectFullName, serializeObjectStack);
            }
            return serializeObjectStack;
        }

        private void CreateSerializeObjectStack(Type serializeObjectType, LinkedList<SerializeObjectContainer> serializeObjectStack)
        {
            if (typeof(IEnumerable).IsAssignableFrom(serializeObjectType) && serializeObjectType != typeof(String) && !typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                if (serializeObjectType.IsGenericType)
                {
                    serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = serializeObjectType, ContainerType = SerializeObjectContainerType.List_Begin });
                    CreateSerializeObjectStack(serializeObjectType.GetGenericArguments()[0], serializeObjectStack);
                    serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = null, ContainerType = SerializeObjectContainerType.List_End });
                }
                else if (serializeObjectType.IsArray)
                {
                    serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = serializeObjectType, ContainerType = SerializeObjectContainerType.Array_Begin });
                    CreateSerializeObjectStack(serializeObjectType.GetElementType(), serializeObjectStack);
                    serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = null, ContainerType = SerializeObjectContainerType.Array_End });
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(serializeObjectType))
            {
                serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = serializeObjectType, ContainerType = SerializeObjectContainerType.DictionaryKey_Begin });
                CreateSerializeObjectStack(serializeObjectType.GetGenericArguments()[1], serializeObjectStack);
                serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = serializeObjectType, ContainerType = SerializeObjectContainerType.DictionaryKey_End });
            }
            else if (!serializeObjectType.IsNormalType())
            {
                serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = serializeObjectType, ContainerType = SerializeObjectContainerType.Object_Begin });
                PropertyInfo[] propertyInfoList = serializeObjectType.GetProperties();
                int propertyCount = propertyInfoList.Length;
                int propertyIndex = -1;
                foreach (PropertyInfo propertyInfoItem in propertyInfoList)
                {
                    propertyIndex++;
                    if (propertyIndex == propertyCount-1)
                    {
                        serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = propertyInfoItem, ContainerType = SerializeObjectContainerType.Property_End });
                    }
                    else
                    {
                        serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = propertyInfoItem, ContainerType = SerializeObjectContainerType.Property_Normal });
                    }
                    if (typeof(IDictionary).IsAssignableFrom(propertyInfoItem.PropertyType))
                    {
                        CreateSerializeObjectStack(propertyInfoItem.PropertyType.GetGenericArguments()[1], serializeObjectStack);
                    }
                    else if (propertyInfoItem.PropertyType.IsGenericType)
                    {
                        CreateSerializeObjectStack(propertyInfoItem.PropertyType.GetGenericArguments()[0], serializeObjectStack);
                    }
                    else
                    {
                        CreateSerializeObjectStack(propertyInfoItem.PropertyType, serializeObjectStack);
                    }

                }
                serializeObjectStack.AddLast(new SerializeObjectContainer { ContainerObject = serializeObjectType, ContainerType = SerializeObjectContainerType.Object_End });
            }
        }
    }
}
