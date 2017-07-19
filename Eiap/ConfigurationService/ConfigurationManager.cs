
using System;

namespace Eiap
{
    public class ConfigurationManager : IConfigurationManager
    {
        private static IConfigurationManager _Manager = null;
        private IConfigurationContainerManager _ConfigurationContainerManager = null;
        private ConfigurationContainer _CurrentConfigurationContainer = new ConfigurationContainer();

        private ConfigurationManager()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Initialization()
        {
            _ConfigurationContainerManager = ConfigurationContainerManager.Instance;
        }

        /// <summary>
        /// 单例对象
        /// </summary>
        public static IConfigurationManager Instance
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new ConfigurationManager();
                }
                return _Manager;
            }
        }

        public IConfigurationManager Register()
        {
            if (string.IsNullOrWhiteSpace(_CurrentConfigurationContainer.Key))
            {
                throw new Exception("Configuration Key IsNullOrWhiteSpace");
            }
            if (string.IsNullOrWhiteSpace(_CurrentConfigurationContainer.Value))
            {
                throw new Exception("Configuration Value IsNullOrWhiteSpace");
            }
            if (string.IsNullOrWhiteSpace(_CurrentConfigurationContainer.Environment))
            {
                throw new Exception("Configuration Environment IsNullOrWhiteSpace");
            }
            if (_ConfigurationContainerManager.ConfigurationIsExist(_CurrentConfigurationContainer.Key))
            {
                throw new Exception("Configuration Key Is Exist, Key:" + _CurrentConfigurationContainer.Key);
            }
            _ConfigurationContainerManager.RegisterConfigurationContainer(_CurrentConfigurationContainer);
            _CurrentConfigurationContainer = new ConfigurationContainer();
            return this;
        }

        public IConfigurationManager SetEnvironment(string environment)
        {
            _CurrentConfigurationContainer.Environment = environment;
            return this;
        }

        public IConfigurationManager SetKey(string key)
        {
            _CurrentConfigurationContainer.Key = key;
            return this;
        }

        public IConfigurationManager SetValue(string value)
        {
            _CurrentConfigurationContainer.Value = value;
            return this;
        }
    }
}
