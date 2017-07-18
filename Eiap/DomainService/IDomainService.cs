
namespace Eiap
{
    /// <summary>
    /// 领域服务接口
    /// </summary>
    public interface IDomainService : IDomainServiceUnitOfWork, IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        ILogger Log { get; set; }
    }
}
