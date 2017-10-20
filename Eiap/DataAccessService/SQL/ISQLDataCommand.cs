
using System;
using System.Data;

namespace Eiap
{
    /// <summary>
    /// SQL数据库命令接口
    /// </summary>
    public interface ISQLDataCommand : IRealtimeDependency, ISQLDataAccessLog, IDisposable
    {
        /// <summary>
        /// 获取返回影响行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>返回影响行数</returns>
        int ExcuteNonQuery(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 获取返回自增Id
        /// </summary>
        /// <typeparam name="ResultID">返回的Id</typeparam>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>自增Id</returns>
        ResultID ExcuteGenericNonQuery<ResultID>(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// SQL命令数据访问链接
        /// </summary>
        ISQLCommandDataAccessConnection SQLDataAccessConnection { set; get; }
    }
}
