
using System;
namespace Eiap
{
    /// <summary>
    /// 数据访问基础接口
    /// </summary>
    public interface ISQLDataAccessLog
    {
        /// <summary>
        /// 日志输出
        /// </summary>
        Action<string> Log { get; set; }

    }
}
