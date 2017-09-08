using System;
using System.Data;
using System.Data.SqlClient;

namespace Eiap.NetFramework
{
    public class SQLServerQueryDataAccessConnection : ISQLQueryDataAccessConnection
    {
        private string _ConnectionString;
        private IDbConnection _DbConnection;
        private IDbTransaction _DbTransaction;
        private bool _IsTransaction;
        private readonly ISQLDataAccessConnectionString _SQLDataAccessConnectionString;

        public SQLServerQueryDataAccessConnection(ISQLDataAccessConnectionString SQLDataAccessConnectionString)
        {
            _SQLDataAccessConnectionString = SQLDataAccessConnectionString;
            _ConnectionString = _SQLDataAccessConnectionString.QueryConnectionString();
        }

        public virtual string ConnectionString
        { 
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

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

        public virtual void DBOpen()
        {
            CheckDbConnection();
            if (_DbConnection.State != ConnectionState.Open)
            {
                _DbConnection.Open();
            }
        }

        public virtual void DBClose()
        {
            CheckDbConnection();
            if (_DbConnection.State == ConnectionState.Open)
            {
                _IsTransaction = false;
                _DbConnection.Close();
                _DbConnection.Dispose();
            }
        }

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

        public virtual void Commit()
        {
            CheckDbTransaction();
            if (_IsTransaction)
            {
                _DbTransaction.Commit();
                _IsTransaction = false;
            }
        }

        public virtual void Rollback()
        {
            CheckDbTransaction();
            if (_IsTransaction)
            {
                _DbTransaction.Rollback();
                _IsTransaction = false;
            }
        }

        public virtual IDbCommand CreateCommand()
        {
            return _DbConnection.CreateCommand();
        }

        public virtual IDbTransaction GetTransaction()
        {
            return _DbTransaction;
        }

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
    }
}
