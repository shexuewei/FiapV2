
using System;
using System.Data;

namespace Eiap
{
    /// <summary>
    /// 数据库查询接口
    /// </summary>
    public interface ISQLQuery : IRealtimeDependency, IDisposable
    {
        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        DataSet ExcuteGetDateSet(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        string SetConnectionString { set; }

    }
}
