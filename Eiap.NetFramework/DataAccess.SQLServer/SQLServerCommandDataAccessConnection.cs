using System;
using System.Data;
using System.Data.SqlClient;

namespace Eiap.NetFramework
{
    /// <summary>
    /// SQLServer命令数据库链接
    /// </summary>
    public class SQLServerCommandDataAccessConnection : ISQLCommandDataAccessConnection
    {
        private string _ConnectionString;
        private IDbConnection _DbConnection;
        private IDbTransaction _DbTransaction;
        private bool _IsTransaction;
        private readonly ISQLDataAccessConnectionString _SQLDataAccessConnectionString;

        public SQLServerCommandDataAccessConnection(ISQLDataAccessConnectionString SQLDataAccessConnectionString)
        {
            _SQLDataAccessConnectionString = SQLDataAccessConnectionString;
            _ConnectionString = _SQLDataAccessConnectionString.CommandConnectionString();
        }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public virtual string ConnectionString
        { 
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        /// <summary>
        /// 创建数据库链接
        /// </summary>
        public virtual void Create()
        {
            try
            {
                if (_DbConnection == null)
                {
                    _DbConnection = new SqlConnection(_ConnectionString);
                }
            }
            catch (Exception ex)
            {
                //TODO:自定义异常
                throw ex;
            }
        }

        /// <summary>
        /// 打开数据库链接
        /// </summary>
        public virtual void DBOpen()
        {
            CheckDbConnection();
            if (_DbConnection.State != ConnectionState.Open)
            {
                _DbConnection.Open();
            }
        }

        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        public virtual void DBClose()
        {
            CheckDbConnection();
            if (_DbConnection.State == ConnectionState.Open)
            {
                _IsTransaction = false;
                _DbConnection.Close();
                //_DbConnection.Dispose();
            }
        }

        /// <summary>
        /// 启用数据库事物
        /// </summary>
        public virtual void BeginTransaction()
        {
            CheckDbConnection();
            if (!_IsTransaction)
            {
                if (_DbConnection.State != ConnectionState.Open)
                {
                    DBOpen();
                }
                _IsTransaction = true;
                _DbTransaction = _DbConnection.BeginTransaction();
            }
        }

        /// <summary>
        /// 提交事物
        /// </summary>
        public virtual void Commit()
        {
            CheckDbTransaction();
            if (_IsTransaction)
            {
                _DbTransaction.Commit();
                _IsTransaction = false;
            }
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public virtual void Rollback()
        {
            CheckDbTransaction();
            if (_IsTransaction)
            {
                _DbTransaction.Rollback();
                _IsTransaction = false;
            }
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns></returns>
        public virtual IDbCommand CreateCommand()
        {
            return _DbConnection.CreateCommand();
        }

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        public virtual IDbTransaction GetTransaction()
        {
            return _DbTransaction;
        }

        /// <summary>
        /// 是否启动事物
        /// </summary>
        public virtual bool IsTransaction
        {
            get { return _IsTransaction; }
        }

        private void CheckDbConnection()
        {
            if (_DbConnection == null)
            {
                //TODO:抛出自定义异常
                throw new Exception("DbConnection Is Null");
            }
        }

        private void CheckDbTransaction()
        {
            if (_DbTransaction == null)
            {
                //TODO:抛出自定义异常
                throw new Exception("DbTransaction Is Null");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_DbConnection != null)
            {
                _DbConnection.Dispose();
            }
        }
    }
}
