
namespace Eiap.NetFramework
{
    public class DataAccessConnectionString : ISQLDataAccessConnectionString
    {
        private readonly IConfigurationManager _ConfigurationManager;
        public DataAccessConnectionString(IConfigurationManager configurationManager)
        {
            _ConfigurationManager = configurationManager;
        }

        public virtual string CommandConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }

        public virtual string QueryConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }

        public virtual string DataQueryConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }

        public virtual string DefaultConnectionString()
        {
            return _ConfigurationManager.Get("DefaultConnectionString");
        }
    }
}
