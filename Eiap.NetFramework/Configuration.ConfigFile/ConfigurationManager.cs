
using System;
using System.Collections.Specialized;
using System.Xml;

namespace Eiap.NetFramework
{
    public class ConfigurationManager : IConfigurationManager
    {
        private IConfigurationContainerManager _ConfigurationContainerManager = null;
        private ConfigurationContainer _CurrentConfigurationContainer = new ConfigurationContainer();
        private readonly string _ConfigPath = @"Configs\{Environment}\appSettings.config";
        private const string _ConfigTagName = "add";

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
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "\\" + _ConfigPath);
            NameValueCollection appsettingsitem = new NameValueCollection();
            foreach (XmlElement element in xmlDoc.GetElementsByTagName(_ConfigTagName))
            {

                appsettingsitem.Add(element.GetAttribute("key"), element.GetAttribute("value"));
            }
            System.Configuration.ConfigurationManager.AppSettings.Add(appsettingsitem);
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
