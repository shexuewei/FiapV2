
namespace Eiap
{
    /// <summary>
    /// 配置信息管理接口
    /// </summary>
    public interface IConfigurationManager: ISingletonDependency, IPropertyDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 当前配置的环境
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        string CurrentEnvironment { get; }

        /// <summary>
        /// 注册配置
        /// </summary>
        /// <returns></returns>
        void Register();

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
    }
}
