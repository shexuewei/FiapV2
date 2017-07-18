
namespace Eiap
{
    /// <summary>
    /// 动态代理容器管理接口
    /// </summary>
    public interface IDynamicProxyContainerManager: ISingletonDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 添加动态代理容器
        /// </summary>
        /// <param name="container"></param>
        void AddDynamicProxyContainer(DynamicProxyContainer container);

        /// <summary>
        /// 获取动态代理容器
        /// </summary>
        /// <param name="dynamicProxyTypeName"></param>
        /// <returns></returns>
        DynamicProxyContainer GetDynamicProxyContainerByDynamicProxyTypeName(string dynamicProxyTypeName);
    }
}
