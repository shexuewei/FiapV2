
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
        /// 日志组件
        /// </summary>
        ILogger Logger { get; set; }

    }
}
