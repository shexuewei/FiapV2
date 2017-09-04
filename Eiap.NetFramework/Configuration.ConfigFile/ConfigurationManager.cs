
using System;
using System.Collections.Specialized;

namespace Eiap.NetFramework
{
    public class ConfigurationManager : IConfigurationManager
    {
        private IConfigurationContainerManager _ConfigurationContainerManager = null;
        private ConfigurationContainer _CurrentConfigurationContainer = new ConfigurationContainer();
        private readonly string _ConfigPath = @"Configs\{Environment}\appSettings.config";

        public string CurrentEnvironment {
            get {
                return System.Configuration.ConfigurationManager.AppSettings["Environment"];
            }
        }

        public ConfigurationManager(IConfigurationContainerManager configurationContainerManager)
        {
            _ConfigurationContainerManager = configurationContainerManager;
            _ConfigPath = _ConfigPath.Replace("{Environment}", CurrentEnvironment);
        }


        /// <summary>
        /// 注册配置
        /// </summary>
        /// <returns></returns>
        public void Register()
        {
            System.Configuration.ConfigurationManager.OpenExeConfiguration(_ConfigPath);
        }


        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
