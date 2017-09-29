
using System;

namespace Eiap
{
    /// <summary>
    /// 数据映射基础接口
    /// </summary>
    public interface ISQLMappingLog
    {
        /// <summary>
        /// 日志输出
        /// </summary>
        Action<string> Log { get; set; }

    }
}
