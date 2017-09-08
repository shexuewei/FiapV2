
using System;
using System.Data;

namespace Eiap.NetFramework
{
    public class SQLServerDataCommand : ISQLDataCommand
    {
        private ISQLCommandDataAccessConnection _SQLDataAccessConnection;

        /// <summary>
        /// 获取返回影响行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>返回影响行数</returns>
        public virtual int ExcuteNonQuery(string cmdText, CommandType cmdType,IDataParameter[] paramters)
        {
            int res = 0;
            try
            {
                CreateSQLCommandDataAccessConnection();
                _SQLDataAccessConnection.Create();
                IDbCommand _DbCommand = _SQLDataAccessConnection.CreateCommand();
                if (_SQLDataAccessConnection.IsTransaction)
                {
                    _DbCommand.Transaction = _SQLDataAccessConnection.GetTransaction();
                }
                _SQLDataAccessConnection.DBOpen();
                res = _DbCommand.ExcuteCommand<int>(_DbCommand.ExecuteNonQuery, cmdText, cmdType, paramters);
            }
            catch (Exception ex)
            {
                if (_SQLDataAccessConnection.IsTransaction)
                {
                    _SQLDataAccessConnection.Rollback();
                }
                throw ex;
            }
            finally
            {
                _SQLDataAccessConnection.DBClose();
            }
            return res;
        }

        public virtual ISQLCommandDataAccessConnection SQLDataAccessConnection
        {
            set
            {
                _SQLDataAccessConnection = value;
            }

            get { return _SQLDataAccessConnection; }
        }

        private void CreateSQLCommandDataAccessConnection()
        {
            if (_SQLDataAccessConnection == null)
            {
                _SQLDataAccessConnection = (ISQLCommandDataAccessConnection)DependencyManager.Instance.Resolver(typeof(ISQLCommandDataAccessConnection));
            }
        }
    }
}
