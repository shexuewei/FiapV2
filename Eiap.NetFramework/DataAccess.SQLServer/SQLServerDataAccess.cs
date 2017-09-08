
using System;
using System.Data;
using System.Data.SqlClient;

namespace Eiap.Framework.DataFramework
{
    /// <summary>
    /// SQLServer数据库链接
    /// </summary>
    public class SQLServerDataAccess : ISQLDataAccess
    {
        private readonly ISQLDataAccessConnection _SQLDataAccessConnection;

        public SQLServerDataAccess(ISQLDataAccessConnection SQLDataAccessConnection)
        {
            _SQLDataAccessConnection = SQLDataAccessConnection;
        }

        /// <summary>
        /// 获取返回影响行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        public virtual int ExcuteNonQuery(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            int res = 0;
            try
            {
                _SQLDataAccessConnection.Create();
                _SQLDataAccessConnection.DBOpen();
                IDbCommand _DbCommand = _SQLDataAccessConnection.CreateCommand();
                if (_SQLDataAccessConnection.IsTransaction)
                {
                    _DbCommand.Transaction = _SQLDataAccessConnection.GetTransaction();
                }
                res = Excute<int>(_DbCommand.ExecuteNonQuery, _DbCommand, cmdText, cmdType, paramters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        public virtual DataSet ExcuteGetDateSet(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            IDbCommand _DBCommand = _SQLDataAccessConnection.CreateCommand();
            _DBCommand.CommandType = cmdType;
            _DBCommand.CommandText = cmdText;
            if (paramters != null)
            {
                foreach (SqlParameter pa in paramters)
                {
                    _DBCommand.Parameters.Add(pa);
                }
            }
            DataSet ds = null;
            IDataAdapter adp = null;
            try
            {
                ds = new DataSet();
                adp = new SqlDataAdapter((SqlCommand)_DBCommand);
                adp.Fill(ds);
                if (paramters != null)
                {
                    _DBCommand.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        /// <summary>
        /// 获取只读数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        public virtual IDataReader ExcuteGetDataReader(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            IDataReader dr = null;
            try
            {
                _SQLDataAccessConnection.DBOpen();
                IDbCommand _DbCommand = _SQLDataAccessConnection.CreateCommand();
                dr = Excute<IDataReader>(_DbCommand.ExecuteReader, _DbCommand, cmdText, cmdType, paramters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dr;
        }

        /// <summary>
        /// 获取数据集第一行第一列对象
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        public virtual object ExecuteScalar(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            object retuObj = null;
            try
            {
                _SQLDataAccessConnection.DBOpen();
                IDbCommand _DbCommand = _SQLDataAccessConnection.CreateCommand();
                retuObj = Excute<object>(_DbCommand.ExecuteScalar, _DbCommand, cmdText, cmdType, paramters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retuObj;
        }

        private T Excute<T>(Func<T> commandToExecute, IDbCommand _DbCommand, string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            T retuObj = default(T);
            try
            {
                _DbCommand.CommandText = cmdText;
                _DbCommand.CommandType = cmdType;

                if (paramters != null)
                {
                    foreach (SqlParameter pa in paramters)
                    {
                        _DbCommand.Parameters.Add(pa);
                    }
                }
                retuObj = commandToExecute();
                if (paramters != null)
                {
                    _DbCommand.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retuObj;
        }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public virtual string ConnectionString
        {
            get
            {
                return _SQLDataAccessConnection.ConnectionString;
            }
            set
            {
                _SQLDataAccessConnection.ConnectionString = value;
            }
        }

        /// <summary>
        /// 创建数据库链接
        /// </summary>
        /// <returns></returns>
        public virtual void Create()
        {
            _SQLDataAccessConnection.Create();
        }

        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        public virtual void DBClose()
        {
            _SQLDataAccessConnection.DBClose();
        }

        /// <summary>
        /// 启用数据库事物
        /// </summary>
        public virtual void BeginTransaction()
        {
            _SQLDataAccessConnection.BeginTransaction();
        }

        /// <summary>
        /// 提交事物
        /// </summary>
        public virtual void Commit()
        {
            _SQLDataAccessConnection.Commit();
        }
    }
}
