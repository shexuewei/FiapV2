
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Eiap.Framework
{
    public class SQLServerQueryMapping<tEntity, TPrimarykey> : ISQLDataQueryMapping<tEntity, TPrimarykey>
        where tEntity : IEntity<TPrimarykey>
        where TPrimarykey : struct
    {
        private List<Expression> _WhereExpressionList;
        private Expression _SelectExpression;
        private string _SqlOrderBy = null;
        private readonly ISQLDataQuery _SQLDataQuery;
        private List<IDataParameter> _DataParameter;
        private int _DataParameterIndex = 0;
        private List<PropertyInfo> PropertyInfoList;
        private int _Top;
        private int _Skip;
        private readonly ISQLDataQueryDataAccessConnection _ReadSQLDataAccessConnection;
        private bool _IsNolock;
        private const string RowIdName = "RowId";
        private readonly ISQLDataMappingExtension _SQLDataMappingExtension;

        public SQLServerQueryMapping(ISQLDataQuery SQLDataQuery, 
            ISQLDataQueryDataAccessConnection ReadSQLDataAccessConnection,
            ISQLDataMappingExtension SQLDataMappingExtension)
        {
            _SQLDataQuery = SQLDataQuery;
            _SQLDataMappingExtension = SQLDataMappingExtension;
            _WhereExpressionList = new List<Expression>();
            _DataParameter = new List<IDataParameter>();
            _SqlOrderBy = "";
            _ReadSQLDataAccessConnection = ReadSQLDataAccessConnection;
            _IsNolock = false;
        }

        public virtual List<tEntity> GetEntityList()
        {
            List<tEntity> entityList = new List<tEntity>();
            PropertyInfoList = typeof(tEntity).GetProperties().ToList();
            string sqlWhere = GetSqlWhere(GetTableName());
            string sqlOrderby = GetSqlOrderBy(GetTableName());
            string sqlSelect = GetSqlSelect(GetTableName());
            string withNolock = GetWithNolockString();
            string sqlJoin = GetJoinSQL();
            _Top = _Top == 0 ? 10 : _Top;
            _Top = _Top + _Skip;
            _Skip = _Skip + 1;
            string sql = (sqlSelect.Length == 0 ? GetSelectAllSQL() : sqlSelect).Replace("select", "") + " " + sqlJoin + " where 1 = 1 " + sqlWhere;
            sql = " with tmpTable as (select ROW_NUMBER() over(" + sqlOrderby + ") as " + RowIdName + ", " + sql + ") select * from tmpTable " + withNolock + " where " + RowIdName + " between " + _Skip + " and " + _Top;
            _SQLDataQuery.SQLDataAccessConnection = _ReadSQLDataAccessConnection;
            _ReadSQLDataAccessConnection.Create();
            _ReadSQLDataAccessConnection.DBOpen();
            using (SqlDataReader dr = (SqlDataReader)_SQLDataQuery.ExcuteGetDataReader(sql, CommandType.Text, _DataParameter.ToArray()))
            {
                while (dr.Read())
                {
                    tEntity entity = (tEntity)Activator.CreateInstance(typeof(tEntity));
                    GetObjectValue(entity, dr);
                    entityList.Add(entity);
                }
                dr.Close();
            }
            _ReadSQLDataAccessConnection.DBClose();
            InitializationParameter();
            return entityList;
        }

        private string WhereExpression(Expression expression, int exprtype, string entityName)
        {
            string result = "";
            if (expression is BinaryExpression)
            {
                var expr = expression as BinaryExpression;
                result += WhereExpression(expr.Left, 0, entityName);
                result = string.Format(result, _SQLDataMappingExtension.GetOperationValue(expr));
                result += WhereExpression(expr.Right, 1, entityName);
            }
            else if (expression is MethodCallExpression)
            {
                var expr = expression as MethodCallExpression;
                result += WhereExpression(expr.Object, 0, entityName);
                result = string.Format(result, _SQLDataMappingExtension.GetOperationValue(expr));
                result = string.Format(result.Replace(_DataParameter[_DataParameterIndex].ParameterName, ""), _DataParameter[_DataParameterIndex].ParameterName);
                result += WhereExpression(expr.Arguments[0], 1, entityName);
            }
            else if (expression is MemberExpression)
            {
                var expr = expression as MemberExpression;
                if (exprtype == 0)
                {
                    string memberName = expr.Member.Name;
                    if (expr.Member.ReflectedType.IsGenericType && expr.Member.ReflectedType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        memberName = (expr.Expression as MemberExpression).Member.Name;
                    }
                    result += " and " + entityName + ".[" + memberName + "] {0} @" + memberName + _DataParameterIndex.ToString();
                    _DataParameter.Add(new SqlParameter());
                    _DataParameter[_DataParameterIndex].ParameterName = "@" + memberName + _DataParameterIndex.ToString();
                }
                else if (exprtype == 1)
                {
                    _DataParameter[_DataParameterIndex].Value = Expression.Lambda(expr).Compile().DynamicInvoke();
                    _DataParameterIndex++;
                    result += "";
                }
            }
            else if (expression is ConstantExpression)
            {
                var expr = expression as ConstantExpression;
                _DataParameter[_DataParameterIndex].Value = expr.Value;
                _DataParameterIndex++;
                result += "";
            }
            return result;
        }

        public virtual List<tEntity> GetEntityAllList()
        {
            List<tEntity> entityList = new List<tEntity>();
            PropertyInfoList = typeof(tEntity).GetProperties().ToList();
            string sqlOrderby = GetSqlOrderBy(GetTableName());
            string sqlSelect = GetSqlSelect(GetTableName());
            string sqlJoin = GetJoinSQL();
            string sql = (sqlSelect.Length == 0 ? GetSelectAllSQL() : sqlSelect) + sqlJoin + sqlOrderby;
            _SQLDataQuery.SQLDataAccessConnection = _ReadSQLDataAccessConnection;
            _ReadSQLDataAccessConnection.Create();
            _ReadSQLDataAccessConnection.DBOpen();
            using (SqlDataReader dr = (SqlDataReader)_SQLDataQuery.ExcuteGetDataReader(sql, CommandType.Text, null))
            {
                while (dr.Read())
                {
                    tEntity entity = (tEntity)Activator.CreateInstance(typeof(tEntity));
                    GetObjectValue(entity, dr);
                    entityList.Add(entity);
                }
                dr.Close();
            }
            _ReadSQLDataAccessConnection.DBClose();
            InitializationParameter();
            return entityList;
        }

        public virtual tEntity GetEntity(TPrimarykey Id)
        {
            tEntity entity = (tEntity)Activator.CreateInstance(typeof(tEntity));
            PropertyInfoList = typeof(tEntity).GetProperties().ToList();
            IDataParameter[] para = new SqlParameter[] { new SqlParameter() { ParameterName = "@" + GetPrimaryKeyParameterName(), Value = Id } };
            _SQLDataQuery.SQLDataAccessConnection = _ReadSQLDataAccessConnection;
            _ReadSQLDataAccessConnection.Create();
            _ReadSQLDataAccessConnection.DBOpen();
            using (SqlDataReader dr = (SqlDataReader)_SQLDataQuery.ExcuteGetDataReader(GetSelectSQL(), CommandType.Text, para))
            {
                while (dr.Read())
                {
                    GetObjectValue(entity, dr);
                }
                dr.Close();
            }
            _ReadSQLDataAccessConnection.DBClose();
            InitializationParameter();
            return entity;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Where(Expression<Func<tEntity, bool>> lambda)
        {
            _WhereExpressionList.Add(lambda);
            return this;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> OrderBy(Expression<Func<tEntity, object>> lambda)
        {
            Expression tmpexpr = ((LambdaExpression)lambda).Body;
            if (tmpexpr is UnaryExpression)
            {
                UnaryExpression unaryexpr = tmpexpr as UnaryExpression;
                MemberExpression memberexpr = unaryexpr.Operand as MemberExpression;
                if (memberexpr != null)
                {
                    _SqlOrderBy += memberexpr.Member.Name + " asc ,";
                }
            }

            return this;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> OrderByDesc(Expression<Func<tEntity, object>> lambda)
        {
            Expression tmpexpr = ((LambdaExpression)lambda).Body;
            if (tmpexpr is UnaryExpression)
            {
                UnaryExpression unaryexpr = tmpexpr as UnaryExpression;
                MemberExpression memberexpr = unaryexpr.Operand as MemberExpression;
                if (memberexpr != null)
                {
                    _SqlOrderBy += memberexpr.Member.Name + " desc ,";
                }
            }
            return this;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Select(Expression<Action<tEntity>> lambda)
        {
            _SelectExpression = lambda;
            return this;
        }
        
        private string GetSqlWhere(string entityName)
        {
            string sqlWhere = "";
            foreach (Expression expr in _WhereExpressionList)
            {
                if (expr.NodeType == ExpressionType.Lambda)
                {
                    sqlWhere += WhereExpression(((LambdaExpression)expr).Body, 0, entityName);
                }
            }
            return sqlWhere;
        }

        private string GetSqlSelect(string entityName)
        {
            string withNolock = GetWithNolockString();
            string sqlSelect = "";
            if (_SelectExpression != null)
            {
                if (PropertyInfoList == null)
                {
                    PropertyInfoList = new List<PropertyInfo>();
                }
                else
                {
                    PropertyInfoList.Clear();
                }
                Expression tmpexpr = ((LambdaExpression)_SelectExpression).Body;
                IReadOnlyCollection<MemberBinding> MemberBindingList = ((MemberInitExpression)tmpexpr).Bindings;
                if (MemberBindingList.Count > 0)
                {
                    foreach (MemberBinding MemberBindingItem in MemberBindingList)
                    {
                        PropertyInfoList.Add(tmpexpr.Type.GetProperty(MemberBindingItem.Member.Name));
                    }
                }
                else
                {
                    PropertyInfoList = tmpexpr.Type.GetProperties().ToList();
                }

                string propertyInfostring = PropertyInfoList.GetPropertyInfoToSelectSql(GetTableName());
                sqlSelect = "select " + propertyInfostring + " from " + GetTableName() + " " + withNolock;
            }
            return sqlSelect;
        }

        private string GetWithNolockString()
        {
            return _IsNolock ? "with(nolock)" : "";
        }

        private string GetSqlOrderBy(string entityName)
        {
            string sqlOrderBy = "";
            if (_SqlOrderBy.Length > 0)
            {
                sqlOrderBy= " order by " + _SqlOrderBy.Substring(0, _SqlOrderBy.Length - 1);
            }
            else
            {
                sqlOrderBy = " order by " + GetPrimaryKeyName();
            }
            return sqlOrderBy;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Top(int top)
        {
            _Top = top;
            return this;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Skip(int skip)
        {
            _Skip = skip;
            return this;
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Nolock()
        {
            _IsNolock = true;
            return this;
        }

        private void GetObjectValue(object entity, IDataReader dr)
        {
            int tmpcount = 0;
            if (dr.GetName(0) == RowIdName)
            {
                tmpcount = 1;
            }
            foreach (PropertyInfo propertyinfo in PropertyInfoList)
            {
                if (!propertyinfo.IsComplexClass())
                {
                    propertyinfo.SetValue(entity, dr.GetValue(tmpcount).GetDefaultValue(propertyinfo.PropertyType.GetProType()), null);
                    tmpcount++;
                }
            }
        }

        private string GetSelectAllSQL()
        {
            return DataManager.Instance.GetDataDescription(typeof(tEntity)).SelectAllSQL;
        }

        private string GetPrimaryKeyName()
        {
            return DataManager.Instance.GetDataDescription(typeof(tEntity)).PrimaryKeyName;
        }

        private string GetSelectSQL()
        {
            return DataManager.Instance.GetDataDescription(typeof(tEntity)).SelectSQL;
        }

        private string GetTableName()
        {
            return DataManager.Instance.GetDataDescription(typeof(tEntity)).TableName;
        }

        private string GetJoinSQL()
        {
            return DataManager.Instance.GetDataDescription(typeof(tEntity)).JoinSQL;
        }

        private string GetPrimaryKeyParameterName()
        {
            return DataManager.Instance.GetDataDescription(typeof(tEntity)).PrimaryKeyParameterName;
        }

        public void Dispose()
        {
            if (_ReadSQLDataAccessConnection != null )
            {
                _ReadSQLDataAccessConnection.Dispose();
            }
        }

        private void InitializationParameter()
        {
            PropertyInfoList.Clear();
            _WhereExpressionList.Clear();
            _DataParameter.Clear();
            _DataParameterIndex = 0;
            _Top = 0;
            _Skip = 0;
        }
    }
}
