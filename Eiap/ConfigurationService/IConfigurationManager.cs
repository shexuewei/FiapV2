
namespace Eiap
{
    /// <summary>
    /// 配置信息管理接口
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// 设置配置的Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IConfigurationManager SetKey(string key);

        /// <summary>
        /// 设置配置的Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IConfigurationManager SetValue(string value);

        /// <summary>
        /// 设置配置的环境
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        IConfigurationManager SetEnvironment(string environment);

        /// <summary>
        /// 注册配置
        /// </summary>
        /// <returns></returns>
        IConfigurationManager Register();

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
    }
}
