
namespace Eiap
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IContextDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 工作单元设置仓储
        /// </summary>
        /// <param name="res"></param>
        void SetRepository(IUnitOfWorkCommandConnection res);

        /// <summary>
        /// 工作单元提交
        /// </summary>
        /// <param name="IsTransaction"></param>
        void Commit(bool IsTransaction = false);
    }
}
