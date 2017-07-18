
namespace Eiap
{
    /// <summary>
    /// 应用工作单元标识
    /// </summary>
    public interface IAppServiceUnitOfWork
    {
        /// <summary>
        /// 当前工作单元
        /// </summary>
        ICurrentUnitOfWork CurrentUnitOfWork { get; }
    }
}
