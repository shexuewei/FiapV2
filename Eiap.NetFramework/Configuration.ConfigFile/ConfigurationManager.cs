
using System;
using System.Collections.Specialized;
using System.Xml;

namespace Eiap.NetFramework
{
    public class ConfigurationManager : IConfigurationManager
    {

        public string CurrentEnvironment {
            get {
                return "";
            }
        }

        /// <summary>
        /// 注册配置
        /// </summary>
        /// <returns></returns>
        public void Register()
        {
            
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
