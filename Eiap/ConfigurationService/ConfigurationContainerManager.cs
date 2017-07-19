
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Eiap
{
    /// <summary>
    /// 配置信息容器管理接口实现
    /// </summary>
    public class ConfigurationContainerManager : IConfigurationContainerManager
    {
        protected static IConfigurationContainerManager _ConfigurationContainerManager = null;
        protected List<ConfigurationContainer> _ConfigurationContainerList = null;

        private ConfigurationContainerManager()
        {
            _ConfigurationContainerList = new List<ConfigurationContainer>();
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static IConfigurationContainerManager Instance
        {
            get
            {
                if (_ConfigurationContainerManager == null)
                {
                    _ConfigurationContainerManager = new ConfigurationContainerManager();
                }
                return _ConfigurationContainerManager;
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="configurationContainer"></param>
        /// <returns></returns>
        public ConfigurationContainer GetConfigurationContainer(string key, string environment)
        {
            return _ConfigurationContainerList.FirstOrDefault(item => item.Key == key && item.Environment == environment);
        }

        /// <summary>
        /// 注册配置信息
        /// </summary>
        /// <param name="configurationContainer"></param>
        public void RegisterConfigurationContainer(ConfigurationContainer configurationContainer)
        {
            _ConfigurationContainerList.Add(configurationContainer);
        }

        /// <summary>
        /// 判断配置是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ConfigurationIsExist(string key)
        {
            return _ConfigurationContainerList.Any(item => item.Key == key);
        }
    }
}
