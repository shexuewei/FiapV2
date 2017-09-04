
namespace Eiap.NetFramework
{
    /// <summary>
    /// 日志配置接口实现
    /// </summary>
    public class LoggerConfigeration : ILoggerConfigeration
    {
        private string _LogPathFormat;
        private long _LogSize;
        private string _LogContentFormat;
        private IConfigurationManager _ConfigurationManager;

        /// <summary>
        /// 初始化
        /// </summary>
        public LoggerConfigeration(IConfigurationManager configurationManager)
        {
            _ConfigurationManager = configurationManager;
            InitializationLogPathFormat();
            InitializationLogSize();
            InitializationLogContentFormat();
        }

        /// <summary>
        /// 初始化日志路径
        /// </summary>
        private void InitializationLogPathFormat()
        {
            _LogPathFormat = _ConfigurationManager.Get("LogPathFormat");
            //TODO:需要验证格式
            if (_LogPathFormat == null || _LogPathFormat == "")
            {
                _LogPathFormat = @"c:\loggers\{AppCode}\{LogLevel}\{YYYY}\{MM}\{DD}\{HH}.log";
            }
        }

        /// <summary>
        /// 初始化日志内容
        /// </summary>
        private void InitializationLogContentFormat()
        {
            //TODO:初始化日志内容
        }

        /// <summary>
        /// 初始化日志大小
        /// </summary>
        private void InitializationLogSize()
        {
            string tmpLogSize = _ConfigurationManager.Get("LogSize");
            //TODO:需要验证格式
            if (tmpLogSize == null || tmpLogSize == "")
            {
                tmpLogSize = "204800000";
            }
            _LogSize = long.Parse(tmpLogSize);
        }

        public string LogPathFormat => _LogPathFormat;

        public long LogSize => _LogSize;

        public string LogContentFormat => _LogContentFormat;
    }
}
