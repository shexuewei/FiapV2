
using System;
using System.Data;
using System.Data.SqlClient;


namespace Eiap.NetFramework
{
    public class SQLServerQuery : ISQLQuery
    {
        private ISQLQueryDataAccessConnection _ReadSQLDataAccessConnection;

        public SQLServerQuery(ISQLQueryDataAccessConnection ReadSQLDataAccessConnection)
        {
            _ReadSQLDataAccessConnection = ReadSQLDataAccessConnection;
        }

        public virtual DataSet ExcuteGetDateSet(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            _ReadSQLDataAccessConnection.Create();
            _ReadSQLDataAccessConnection.DBOpen();
            IDbCommand _DBCommand = _ReadSQLDataAccessConnection.CreateCommand();
            _DBCommand.CommandType = cmdType;
            _DBCommand.CommandText = cmdText;
            if (paramters != null)
            {
                foreach (IDataParameter pa in paramters)
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
            finally
            {
                _ReadSQLDataAccessConnection.DBClose();
            }
            return ds;
        }


        public virtual string SetConnectionString
        {
            set 
            { 
                _ReadSQLDataAccessConnection.ConnectionString = value; 
            }
        }
    }
}
