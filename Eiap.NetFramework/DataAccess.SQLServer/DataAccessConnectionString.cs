
namespace Eiap.NetFramework
{
    /// <summary>
    /// 数据库链接字符串
    /// </summary>
    public class DataAccessConnectionString : ISQLDataAccessConnectionString
    {
        private readonly IConfigurationManager _ConfigurationManager;

        public DataAccessConnectionString(IConfigurationManager configurationManager)
        {
            _ConfigurationManager = configurationManager;
        }

        /// <summary>
        /// 命令链接
        /// </summary>
        /// <returns></returns>
        public virtual string CommandConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }

        /// <summary>
        /// 查询链接
        /// </summary>
        /// <returns></returns>
        public virtual string QueryConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }

        /// <summary>
        /// 查询链接
        /// </summary>
        /// <returns></returns>
        public virtual string DataQueryConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }

        /// <summary>
        /// 默认链接
        /// </summary>
        /// <returns></returns>
        public virtual string DefaultConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }
    }
}
