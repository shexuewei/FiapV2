using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// SQL数据库数据扩展功能
    /// </summary>
    public static class SQLServerDataExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_DbCommand"></param>
        /// <param name="commandToExecute"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static T ExcuteCommand<T>(this IDbCommand _DbCommand,Func<T> commandToExecute, string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            T result = default(T);
            try
            {
                _DbCommand.CommandText = cmdText;
                _DbCommand.CommandType = cmdType;

                if (paramters != null)
                {
                    _DbCommand.Parameters.Clear();
                    foreach (IDataParameter pa in paramters)
                    {
                        _DbCommand.Parameters.Add(pa);
                    }
                }
                result = commandToExecute();
                if (paramters != null)
                {
                    _DbCommand.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static TResult DataSetToEntityList<TResult>(this DataSet ds, IMethodManager methodManager)
        {
            TResult result = default(TResult);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (typeof(TResult).IsGenericType)
                {
                    Type genType = typeof(TResult).GetGenericTypeDefinition();
                    Type genParaType = typeof(TResult).GetGenericArguments()[0];
                    Type objtype = genType.MakeGenericType(genParaType);
                    object objlist = Activator.CreateInstance(objtype);
                    IList list = (IList)objlist;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        object entity = Activator.CreateInstance(genParaType);
                        list.Add(dr.DataRowToEntity(entity, methodManager));
                    }
                    return (TResult)list;
                }
                else
                {
                    object entity = Activator.CreateInstance(typeof(TResult));
                    return (TResult)ds.Tables[0].Rows[0].DataRowToEntity(entity, methodManager);
                }
            }
            return result;
        }

        public static object DataRowToEntity(this DataRow dr, object entity, IMethodManager methodManager)
        {
            PropertyInfo[] propList = entity.GetType().GetProperties();
            foreach (PropertyInfo propInfo in propList)
            {
                methodManager.MethodInvoke(entity, new object[] { dr[propInfo.Name] }, propInfo.GetSetMethod());
            }
            return entity;
        }
    }
}
