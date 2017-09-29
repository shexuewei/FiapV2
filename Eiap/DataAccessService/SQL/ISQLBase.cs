
using System;
using System.Data;

namespace Eiap
{
    /// <summary>
    /// 数据库基础接口
    /// </summary>
    public interface ISQLBase : IDisposable
    {
        /// <summary>
        /// 日志输出
        /// </summary>
        Action<string> Log { get; set; }

    }
}
