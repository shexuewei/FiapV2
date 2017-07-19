
namespace Eiap
{
    /// <summary>
    /// 配置信息容器管理接口
    /// </summary>
    public interface IConfigurationContainerManager
    {
        /// <summary>
        /// 注册配置信息
        /// </summary>
        /// <param name="configurationContainer"></param>
        void RegisterConfigurationContainer(ConfigurationContainer configurationContainer);

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="configurationContainer"></param>
        /// <returns></returns>
        ConfigurationContainer GetConfigurationContainer(string key, string environment);

        /// <summary>
        /// 判断配置是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ConfigurationIsExist(string key);
    }
}
