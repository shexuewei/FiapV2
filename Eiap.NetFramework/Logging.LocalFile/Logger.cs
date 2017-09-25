using System;
using System.Text;
using System.IO;
using System.Collections.Concurrent;

namespace Eiap.NetFramework
{
    /// <summary>
    /// 日志组件实现类
    /// </summary>
    public class Logger : ILogger
    {
        private readonly ISerializationManager _SerializationManager;
        private readonly ILoggerConfigeration _LoggerConfigeration;
        private ConcurrentQueue<LogMessage> _LogMessageQueue;
        public EventHandler<EventArgs> PublishLogMessage_Event;

        public Logger(ILoggerConfigeration loggerConfigeration, ISerializationManager serializationManager)
        {
            _LoggerConfigeration = loggerConfigeration;
            _SerializationManager = serializationManager;
        }

        /// <summary>
        /// 根据日志级别输出
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        /// <param name="level"></param>
        public void Print(string message, string logKey, LogLevel level = LogLevel.INFO, int logSource = 0, string logName = null, LoggerTrace logTrace = null)
        {
            switch (level)
            {
                case LogLevel.DEBUG:
                    Debug(message, logKey, logSource, logName, logTrace);
                    break;
                case LogLevel.ERROR:
                    Error(message, logKey, logSource, logName, logTrace);
                    break;
                case LogLevel.FATAL:
                    Fatal(message, logKey, logSource, logName, logTrace);
                    break;
                case LogLevel.INFO:
                    Info(message, logKey, logSource, logName, logTrace);
                    break;
                case LogLevel.WARN:
                    Warn(message, logKey, logSource, logName, logTrace);
                    break;
            }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        public void Debug(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null)
        {
            LogMessage logMessage = GetLogMessage(message, logKey, LogLevel.DEBUG, logName, logSource, logTrace);
            PublishLogMessage(logMessage);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        public void Error(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null)
        {
            LogMessage logMessage = GetLogMessage(message, logKey, LogLevel.ERROR, logName, logSource, logTrace);
            PublishLogMessage(logMessage);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        public void Fatal(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null)
        {
            LogMessage logMessage = GetLogMessage(message, logKey, LogLevel.FATAL, logName, logSource, logTrace);
            PublishLogMessage(logMessage);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        public void Info(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null)
        {
            LogMessage logMessage = GetLogMessage(message, logKey, LogLevel.INFO, logName, logSource, logTrace);
            PublishLogMessage(logMessage);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logTrace">日志跟踪对象</param>
        public void Warn(string message, string logKey, int logSource = 0, string logName = null, LoggerTrace logTrace = null)
        {
            LogMessage logMessage = GetLogMessage(message, logKey, LogLevel.WARN, logName, logSource, logTrace);
            PublishLogMessage(logMessage);
        }

        /// <summary>
        /// 获取日志消息对象
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logKey">日志Key</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="logName">日志名称</param>
        /// <param name="logSource">日志来源</param>
        /// <param name="logTrace">日志跟踪对象</param>
        /// <returns></returns>
        private LogMessage GetLogMessage(string message, string logKey, LogLevel logLevel, string logName, int logSource, LoggerTrace logTrace)
        {
            Guid logBodyKey = Guid.NewGuid();
            LogMessage logmessage = new LogMessage
            {
                LogHead = new LogHead
                {
                    Id = Guid.NewGuid(),
                    LogBodyKey = logBodyKey,
                    LogDateTime = DateTime.Now,
                    LogKey = logKey,
                    LogLevel = logLevel,
                    //TODO:后续完善
                    LogName = logName,
                    LogSource = logSource,
                    ApplicationName = "",
                    ServerIp = "",
                    ModulesName = ""
                },
                LogBody = new LogBody
                {
                    Id = logBodyKey,
                    LogBodyContent = message
                }
            };
            if (logTrace != null)
            {
                logmessage.LogHead.LoggerTrace = logTrace;
            }
            return logmessage;
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveLogMessage(object sender, EventArgs args)
        {
            LogMessage logMessage = null;
            _LogMessageQueue.TryDequeue(out logMessage);
            if (logMessage != null)
            {
                string logpathformat = _LoggerConfigeration.LogPathFormat;
                long logsize = _LoggerConfigeration.LogSize;
                string logpath = logpathformat
                    .Replace("{AppCode}", logMessage.LogHead.LogName)
                    .Replace("{LogLevel}", logMessage.LogHead.LogLevel.ToString())
                    .Replace("{YYYY}", DateTime.Now.Year.ToString())
                    .Replace("{MM}", DateTime.Now.Month.ToString())
                    .Replace("{DD}", DateTime.Now.Day.ToString())
                    .Replace("{HH}", DateTime.Now.Hour.ToString())
                    .Replace("{mm}", DateTime.Now.Minute.ToString());
                string logpathDirectoryName = Path.GetDirectoryName(logpath);
                if (!Directory.Exists(logpathDirectoryName))
                {
                    Directory.CreateDirectory(logpathDirectoryName);
                }
                FileInfo fileinfo = new FileInfo(logpath);
                if (fileinfo.Exists && fileinfo.Length >= logsize)
                {
                    logpath = logpath.Replace(".log", "_" + DateTime.Now.Ticks.ToString() + ".log");
                }
                File.AppendAllText(logpath, _SerializationManager.SerializeObject(logMessage), Encoding.UTF8);
            }
        }

        /// <summary>
        /// 创建日志消息队列并注册日志消费事件
        /// </summary>
        private void InitializationLogMessageQueue()
        {
            if (_LogMessageQueue == null)
            {
                _LogMessageQueue = new ConcurrentQueue<LogMessage>();
                PublishLogMessage_Event += SaveLogMessage;
            }
        }

        /// <summary>
        /// 发布日志消息到队列，并触发日志消息事件
        /// </summary>
        /// <param name="logMessage"></param>
        private void PublishLogMessage(LogMessage logMessage)
        {
            _LogMessageQueue.Enqueue(logMessage);
            PublishLogMessage_Event?.Invoke(null, null);
        }
    }
}
