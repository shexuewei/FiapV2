
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
        public virtual int ExcuteNonQuery(string cmdText, CommandType cmdType, IDataParameter[] paramters)
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

        /// <summary>
        /// 获取返回自增Id
        /// </summary>
        /// <typeparam name="ResultID">返回的Id</typeparam>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>自增Id</returns>
        public virtual ResultID ExcuteNonQuery<ResultID>(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            object res = null;
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
                res = _DbCommand.ExcuteCommand<object>(_DbCommand.ExecuteScalar, cmdText, cmdType, paramters);
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
            return (ResultID)Convert.ChangeType(res, typeof(ResultID));
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        public virtual ISQLCommandDataAccessConnection SQLDataAccessConnection
        {
            set
            {
                _SQLDataAccessConnection = value;
            }

            get { return _SQLDataAccessConnection; }
        }

        /// <summary>
        /// 创建数据库链接
        /// </summary>
        private void CreateSQLCommandDataAccessConnection()
        {
            if (_SQLDataAccessConnection == null)
            {
                _SQLDataAccessConnection = (ISQLCommandDataAccessConnection)DependencyManager.Instance.Resolver(typeof(ISQLCommandDataAccessConnection));
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_SQLDataAccessConnection != null)
            {
                _SQLDataAccessConnection.Dispose();
            }
        }

        public ILogger Logger { get; set; }
    }
}
