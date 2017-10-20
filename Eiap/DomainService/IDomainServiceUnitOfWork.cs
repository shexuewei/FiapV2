
namespace Eiap
{
    /// <summary>
    /// 领域服务工作单元
    /// </summary>
    public interface IDomainServiceUnitOfWork
    {
        /// <summary>
        /// 当前工作单元接口
        /// </summary>
        IUnitOfWork CurrentUnitOfWork { get; }
    }
}
