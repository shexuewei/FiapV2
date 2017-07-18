
using System;

namespace Eiap
{
    /// <summary>
    /// 日志详情
    /// </summary>
    public class LogBody
    {
        /// <summary>
        /// 日志消息体Key
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 日志消息内容
        /// </summary>
        public virtual string LogBodyContent { get; set; }

    }
}
