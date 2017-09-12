
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eiap.Framework
{
    //TODO:启动时，自动将所有领域对象加载进来
    public class DataManager
    {
        private static DataManager _DataManager;

        public static DataManager Instance
        {
            get
            {
                if (_DataManager == null)
                {
                    _DataManager = new DataManager();
                }
                return _DataManager;
            }
        }

        private Dictionary<string, DataDescription> _DataDescriptionList;

        private DataManager()
        {
            _DataDescriptionList = new Dictionary<string, DataDescription>();
        }

        public DataDescription GetDataDescription(Type type)
        {
            DataDescription datadescription = null;
            if (_DataDescriptionList.ContainsKey(type.FullName))
            {
                datadescription = _DataDescriptionList[type.FullName];
            }
            else
            {
                datadescription = new DataDescription();
                datadescription.TableName = type.GetTableName();
                datadescription.PrimaryKeyName = type.GetPrimaryKeyName(datadescription.TableName);
                datadescription.PrimaryKeyParameterName = type.GetPrimaryKeyParameterName();
                datadescription.SelectAllSQL = GetSelectAllSQL(datadescription, type);
                datadescription.SelectSQL = GetSelectSQL(datadescription, type);
                datadescription.InsertSQL = GetInsertSQL(datadescription, type);
                datadescription.UpdateSQL = GetUpdateSQL(datadescription, type);
                datadescription.DeleteSQL = GetDeleteSQL(datadescription, type);
                datadescription.JoinSQL = GetJoinSQL(datadescription, type);
                _DataDescriptionList.Add(type.FullName, datadescription);
            }
            return datadescription;
        }

        private string GetSelectSQL(DataDescription datadescription, Type t)
        {

            return datadescription.SelectAllSQL + " where " + datadescription.PrimaryKeyName + " = @" + datadescription.PrimaryKeyParameterName;
        }

        private string GetSelectAllSQL(DataDescription datadescription, Type t)
        {
            string propertyInfostring = t.GetProperties().ToList().GetPropertyInfoToSelectSql(datadescription.TableName);
            return "select " + propertyInfostring + " from " + datadescription.TableName + " with(nolock) ";
        }

        private string GetInsertSQL(DataDescription datadescription, Type t)
        {
            string sql = "insert into " + datadescription.TableName + " ({0}) values ({1});";
            PropertyInfo[] pi = t.GetProperties().Where(m => !m.IsComplexClass() && m.PropertyType.Name == typeof(int).Name).ToArray();
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();
            int index = -1;
            foreach (PropertyInfo info in pi)
            {
                index++;
                if (!info.IsComplexClass())
                {
                    fields.Append(info.GetColumnName(datadescription.TableName));
                    values.Append("@");
                    values.Append(info.Name);
                    values.Append("_{0}");
                    if (index != pi.Length - 1)
                    {
                        fields.Append(",");
                        values.Append(",");
                    }

                }
            }
            return string.Format(sql, fields.ToString(), values.ToString());
        }

        private string GetUpdateSQL(DataDescription datadescription, Type t)
        {
            StringBuilder sql = new StringBuilder("update ");
            sql.Append(datadescription.TableName);
            sql.Append(" set ");
            PropertyInfo[] pi = t.GetProperties().Where(m => !m.IsPrimaryKey()).ToArray();
            int index = -1;
            foreach (PropertyInfo info in pi)
            {
                index++;
                if (!info.IsComplexClass())
                {
                    sql.Append(info.GetColumnName(datadescription.TableName));
                    sql.Append(" = @");
                    sql.Append(info.Name);
                    sql.Append("_{0}");
                    if (index != pi.Length - 1)
                    {
                        sql.Append(",");
                    }
                }
            }
            return sql.ToString() + " where " + datadescription.PrimaryKeyName + " = @" + datadescription.PrimaryKeyParameterName + "_{0};";
        }

        private string GetDeleteSQL(DataDescription datadescription, Type t)
        {
            return "delete " + datadescription.TableName + " where " + datadescription.PrimaryKeyName + " = @" + datadescription.PrimaryKeyParameterName + "_{0};";
        }

        private string GetJoinSQL(DataDescription datadescription, Type t)
        {
            StringBuilder joinsql = new StringBuilder();
            List<PropertyInfo> propertyInfoList = t.GetProperties().Where(m => m.IsNavigationProperty()).ToList();
            if (propertyInfoList.Count > 0)
            {
                for (int i = 0; i < propertyInfoList.Count; i++)
                {
                    PropertyInfo m = propertyInfoList[i];
                    string tmpTableName = DataManager.Instance.GetDataDescription(m.PropertyType).TableName;
                    string tmpIndexTableName = tmpTableName.Replace("]", "_" + i.ToString() + "]");
                    string tmpKeyName = DataManager.Instance.GetDataDescription(m.PropertyType).PrimaryKeyName;
                    joinsql.Append(" join (");
                    joinsql.Append(DataManager.Instance.GetDataDescription(m.PropertyType).SelectAllSQL);
                    joinsql.Append(") ");
                    joinsql.Append(tmpIndexTableName);
                    joinsql.Append(" on ");
                    joinsql.Append(tmpKeyName.Replace(tmpTableName, tmpIndexTableName));
                    joinsql.Append(" = ");
                    joinsql.Append(t.GetForeignKeyName(m.PropertyType));
                    joinsql.Append(DataManager.Instance.GetDataDescription(m.PropertyType).JoinSQL.Replace(tmpTableName, tmpIndexTableName));
                }
            }
            return joinsql.ToString();
        }
    }
}
