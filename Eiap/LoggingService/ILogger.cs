
namespace Eiap
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger : IRealtimeDependency, IPropertyDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        void Error(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="message"></param>
        void Fatal(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null);

    }
}
