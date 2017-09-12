
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eiap.Framework
{
    public static class DataExtension
    {
        public static string GetPrimaryKeyName(this Type tEntity, string tableName)
        {
            string key = "";
            PropertyInfo[] propertyList = tEntity.GetProperties();
            foreach (PropertyInfo p in propertyList)
            {
                PropertyAttribute proAtt = p.GetCustomAttribute(typeof(PropertyAttribute)) as PropertyAttribute;
                if (proAtt != null && proAtt.IsPrimaryKey && proAtt.ColumnName != null && proAtt.ColumnName.Length > 0)
                {
                    key = tableName + ".[" + proAtt.ColumnName + "]";
                    break;
                }
            }
            if (key.Length == 0)
            {
                key = tableName + ".[" + propertyList[0].Name + "]";
            }
            return key;
        }

        public static string GetPrimaryKeyParameterName(this Type tEntity)
        {
            string paraName = "";
            PropertyInfo[] propertyList = tEntity.GetProperties();
            foreach (PropertyInfo p in propertyList)
            {
                PropertyAttribute proAtt = p.GetCustomAttribute(typeof(PropertyAttribute)) as PropertyAttribute;
                if (proAtt != null && proAtt.IsPrimaryKey && proAtt.ColumnName != null && proAtt.ColumnName.Length > 0)
                {
                    paraName = proAtt.ColumnName;
                    break;
                }
            }
            if (paraName.Length == 0)
            {
                paraName = propertyList[0].Name;
            }
            return paraName;
        }

        public static string GetTableName(this Type tEntity)
        {
            string tableName = "";
            EntityAttribute tableAttr = tEntity.GetCustomAttribute(typeof(EntityAttribute)) as EntityAttribute;
            if (tableAttr != null && tableAttr.TableName != null && tableAttr.TableName.Length > 0)
            {
                tableName = "[" + tableAttr.TableName + "]";
            }
            else
            {
                tableName = "[" + tEntity.Name + "]";
            }
            return tableName;
        }

        public static string GetColumnName(this PropertyInfo tEntityProperty, string tableName)
        {
            string propertyInfostring = "";
            //TODO:判断是否是复杂类型
            PropertyAttribute proAtt = tEntityProperty.GetCustomAttribute(typeof(PropertyAttribute)) as PropertyAttribute;
            if (proAtt == null)
            {
                propertyInfostring = tableName + ".[" + tEntityProperty.Name + "]";
            }
            else
            {
                propertyInfostring =  tableName + ".[" + proAtt.ColumnName + "]";
            }
            return propertyInfostring;
        }

        public static string GetPropertyInfoToSelectSql(this List<PropertyInfo> tEntityProperties, string tableName)
        {
            StringBuilder propertyInfoToSelectSql = new StringBuilder();
            int index = -1;
            foreach (PropertyInfo info in tEntityProperties)
            {
                index++;
                if (!info.IsComplexClass())
                {
                    propertyInfoToSelectSql.Append(info.GetColumnName(tableName));
                    if (index != tEntityProperties.Count - 1)
                    {
                        propertyInfoToSelectSql.Append(",");
                    }
                }
            }
            return propertyInfoToSelectSql.ToString();
        }

        /// <summary>
        /// 获取类型的默认值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this object obj, Type type)
        {
            if (obj == DBNull.Value)
            {
                obj = default(object);  //Null
            }
            else
            {
                obj = Convert.ChangeType(obj, type);    //将对象转换为指定类型
            }
            return obj;
        }

        /// <summary>
        /// 首先判断他是否是泛型可空型。然后再取出他原始的类型
        /// </summary>
        /// <param name="proType"></param>
        /// <returns></returns>
        public static Type GetProType(this Type proType)
        {
            bool isnullableValueType = proType.IsGenericType && (proType.GetGenericTypeDefinition() == typeof(Nullable<>));  //判断是否为泛型、可空类型
            Type genericType = proType;
            if (isnullableValueType)
            {
                genericType = proType.GetGenericArguments()[0];     //如果是泛型、可空类型，取原始类型
            }
            return genericType;
        }

        public static string GetTypeName(this PropertyInfo prop, bool isnull)
        {
            string res = "";
            string typename = isnull ? prop.PropertyType.GetProperties()[1].PropertyType.Name : prop.PropertyType.Name;
            switch (typename)
            {
                case "Boolean":
                    res = "bit";
                    break;
                case "Byte":
                    res = "tinyint";
                    break;
                case "Int16":
                    res = "smallint";
                    break;
                case "Int32":
                    res = "int";
                    break;
                case "Int64":
                    res = "bigint";
                    break;
                case "Single":
                    res = "real";
                    break;
                case "Double":
                    res = "float";
                    break;
                case "Decimal":
                    res = "money";
                    break;
                case "DateTime":
                    res = "datetime";
                    break;
                case "String":
                    res = "varchar";
                    break;
                case "Byte[]":
                    res = "binary";
                    break;
                case "Guid":
                    res = "uniqueidentifier";
                    break;
            }
            return res;
        }

        /// <summary>
        /// 判断属性是否是主键
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsPrimaryKey(this PropertyInfo prop)
        {
            PropertyAttribute proAtt = prop.GetCustomAttribute(typeof(PropertyAttribute)) as PropertyAttribute;
            if (proAtt != null && proAtt.IsPrimaryKey && proAtt.ColumnName != null && proAtt.ColumnName.Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断属性是否是复杂类型（集合类型）
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsComplexClass(this PropertyInfo prop)
        {
            if ((prop.PropertyType.IsClass && prop.PropertyType.Name.ToLower() != "string")
                || typeof(IEntity).IsAssignableFrom(prop.PropertyType)
                || (prop.PropertyType.IsGenericType && !prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断属性是否是导航属性
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsNavigationProperty(this PropertyInfo prop)
        {
            if (prop.PropertyType.IsClass && typeof(IEntity).IsAssignableFrom(prop.PropertyType))
            {
                return true;
            }
            return false;
        }

        public static string GetForeignKeyName(this Type tEntity, Type navigationType)
        {
            string foreignKeyName = "";
            List<PropertyInfo> navigationPropertyList = tEntity.GetProperties().Where(m => m.IsNavigationProperty()).ToList();
            foreach (PropertyInfo navigationPropertyItem in navigationPropertyList)
            {
                if (navigationPropertyItem.PropertyType.FullName == navigationType.FullName)
                {
                    PropertyAttribute proAtt = navigationPropertyItem.GetCustomAttribute(typeof(PropertyAttribute)) as PropertyAttribute;
                    if (proAtt != null && proAtt.ForeignKey != null && proAtt.ForeignKey.Length > 0)
                    {
                        foreignKeyName = tEntity.GetTableName() + ".[" + proAtt.ForeignKey + "]";
                    }
                    else
                    {
                        foreignKeyName = tEntity.GetTableName() + ".[" + navigationPropertyItem.PropertyType.Name + "Id" + "]";
                    }
                }
            }
            return foreignKeyName;
        }

    }
}
