
using System;
namespace Eiap
{
    /// <summary>
    /// 数据仓储基础接口
    /// </summary>
    public interface IRepositoryLog
    {
        /// <summary>
        /// 日志输出
        /// </summary>
        Action<string> Log { get; set; }

    }
}
