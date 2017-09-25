
using System;
using System.Data;

namespace Eiap
{
    /// <summary>
    /// SQL数据库查询接口
    /// </summary>
    public interface ISQLDataQuery : IRealtimeDependency, ISQLBase
    {

        /// <summary>
        /// 获取只读数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>返回只读数据集</returns>
        IDataReader ExcuteGetDataReader(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 获取数据集第一行第一列对象
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>返回数据集第一行第一列对象</returns>
        object ExecuteScalar(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 数据库访问连接
        /// </summary>
        ISQLDataQueryDataAccessConnection SQLDataAccessConnection { set; get; }
    }
}
