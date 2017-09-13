
using System;
using System.Data;

namespace Eiap
{
    /// <summary>
    /// SQL数据库命令接口
    /// </summary>
    public interface ISQLDataCommand : IRealtimeDependency, IDisposable
    {
        /// <summary>
        /// 获取返回影响行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns>返回影响行数</returns>
        int ExcuteNonQuery(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        ISQLCommandDataAccessConnection SQLDataAccessConnection { set; get; }
    }
}
