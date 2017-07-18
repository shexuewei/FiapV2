
namespace Eiap
{
    /// <summary>
    /// 链接字符串
    /// </summary>
    public interface IDataAccessConnectionString : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 命令链接
        /// </summary>
        /// <returns></returns>
        string CommandConnectionString();

        /// <summary>
        /// 查询链接
        /// </summary>
        /// <returns></returns>
        string QueryConnectionString();

        /// <summary>
        /// 查询链接
        /// </summary>
        /// <returns></returns>
        string DataQueryConnectionString();

        /// <summary>
        /// 默认链接
        /// </summary>
        /// <returns></returns>
        string DefaultConnectionString();

    }
}
