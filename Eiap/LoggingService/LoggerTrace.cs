
using System;

namespace Eiap
{
    /// <summary>
    /// 日志追踪
    /// </summary>
    public class LoggerTrace
    {
        /// <summary>
        /// 日志追踪ID
        /// </summary>
        public Guid TraceId { get; set; }

        /// <summary>
        /// 本地ID
        /// </summary>
        public int LocalId { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
    }
}
