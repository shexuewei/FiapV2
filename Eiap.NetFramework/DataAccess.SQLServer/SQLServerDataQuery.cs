﻿
using System;
using System.Data;

namespace Eiap.NetFramework
{
    public class SQLServerDataQuery : ISQLDataQuery
    {
        private ISQLDataQueryDataAccessConnection _SQLDataAccessConnection;

        /// <summary>
        /// 获取只读数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>返回只读数据集</returns>
        public virtual IDataReader ExcuteGetDataReader(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            IDataReader dr = null;
            try
            {
                CreateSQLDataQueryDataAccessConnection();
                _SQLDataAccessConnection.Create();
                IDbCommand _DbCommand = _SQLDataAccessConnection.CreateCommand();
                _SQLDataAccessConnection.DBOpen();
                dr = _DbCommand.ExcuteCommand<IDataReader>(_DbCommand.ExecuteReader, cmdText, cmdType, paramters);
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
        /// <returns>返回数据集第一行第一列对象</returns>
        public virtual object ExecuteScalar(string cmdText, CommandType cmdType, IDataParameter[] paramters)
        {
            object retuObj = null;
            try
            {
                CreateSQLDataQueryDataAccessConnection();
                _SQLDataAccessConnection.Create();
                IDbCommand _DbCommand = _SQLDataAccessConnection.CreateCommand();
                retuObj = _DbCommand.ExcuteCommand<object>(_DbCommand.ExecuteScalar, cmdText, cmdType, paramters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _SQLDataAccessConnection.DBClose();
            }
            return retuObj;
        }

        public virtual ISQLDataQueryDataAccessConnection SQLDataAccessConnection
        {
            set
            {
                _SQLDataAccessConnection = value;
            }

            get { return _SQLDataAccessConnection; }
        }

        private void CreateSQLDataQueryDataAccessConnection()
        {
            if (_SQLDataAccessConnection == null)
            {
                _SQLDataAccessConnection = (ISQLDataQueryDataAccessConnection)DependencyManager.Instance.Resolver(typeof(ISQLDataQueryDataAccessConnection));
            }
        }
    }
}
