
using System;
using System.Data;
using System.Data.SqlClient;

namespace Eiap.Framework.DataFramework
{
    public class SQLServerDataAccess : ISQLDataAccess
    {
        private readonly ISQLDataAccessConnection _SQLDataAccessConnection;

        public SQLServerDataAccess(ISQLDataAccessConnection SQLDataAccessConnection)
        {
            _SQLDataAccessConnection = SQLDataAccessConnection;
        }

        public virtual int ExcuteNonQuery(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            int res = 0;
            try
            {
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

        public virtual void Create()
        {
            _SQLDataAccessConnection.Create();
        }

        public virtual void DBClose()
        {
            _SQLDataAccessConnection.DBClose();
        }

        public virtual void BeginTransaction()
        {
            _SQLDataAccessConnection.BeginTransaction();
        }

        public virtual void Commit()
        {
            _SQLDataAccessConnection.Commit();
        }
    }
}
