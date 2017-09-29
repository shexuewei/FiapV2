
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Eiap.Framework
{
    public class SQLDataMappingExtension : ISQLDataMappingExtension
    {
        public string GetOperationValue(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.NotEqual:
                    return " != ";
                case ExpressionType.Call:
                    return " like '%'+{0}+'%' ";
                default:
                    return "";
            }
        }

        public IDataParameter[] GetDataParameter(IMethodManager methodManager, IEntity entity, int index = 0)
        {
            PropertyInfo[] pi = entity.GetType().GetProperties().Where(prop => !prop.IsComplexClass()).ToArray();
            IDataParameter[] para = new SqlParameter[pi.Length];
            for (int i = 0; i < pi.Length; i++)
            {
                object objvalue = methodManager.MethodInvoke(entity, new object[] { }, pi[i].GetGetMethod()); //pi[i].GetValue(entity, null);
                if (objvalue == null)
                {

                    para[i] = new SqlParameter("@" + pi[i].Name + "_" + index, DBNull.Value);
                }
                else
                {
                    para[i] = new SqlParameter("@" + pi[i].Name + "_" + index, objvalue);
                }
            }
            return para;
        }
    }
}
