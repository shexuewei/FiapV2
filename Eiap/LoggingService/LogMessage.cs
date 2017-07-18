
namespace Eiap
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// 消息头
        /// </summary>
        public virtual LogHead LogHead { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public virtual LogBody LogBody { get; set; }
    }
}
