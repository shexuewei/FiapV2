
namespace Eiap
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger : ISingletonDependency, IPropertyDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        void Debug(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        void Error(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        void Fatal(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        void Info(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        void Warn(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

    }
}
