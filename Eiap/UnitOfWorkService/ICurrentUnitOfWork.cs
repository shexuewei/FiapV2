
namespace Eiap
{
    /// <summary>
    /// 当前工作单元接口
    /// </summary>
    public interface ICurrentUnitOfWork : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 当前工作单元
        /// </summary>
        IUnitOfWork CurrentUnitOfWork { get; }
    }
}
