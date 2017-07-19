
namespace Eiap.NetFramework
{
    /// <summary>
    /// 日志配置接口
    /// </summary>
    public interface ILoggerConfigeration : ISingletonDependency, IPropertyDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 日志路径模板
        /// </summary>
        string LogPathFormat { get; }

        /// <summary>
        /// 日志大小
        /// </summary>
        long LogSize { get; }

        /// <summary>
        /// 日志内容模板
        /// </summary>
        string LogContentFormat { get; }
    }
}
