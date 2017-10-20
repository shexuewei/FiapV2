
using System;

namespace Eiap
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IContextDependency, IDynamicProxyDisable, IDisposable
    {
        /// <summary>
        /// 工作单元设置仓储
        /// </summary>
        /// <param name="res"></param>
        void SetRepository(IUnitOfWorkCommandConnection res);

        /// <summary>
        /// 开启工作单元
        /// </summary>
        /// <param name="istransaction"></param>
        void Open(bool istransaction = false);

        /// <summary>
        /// 提交工作单元
        /// </summary>
        /// <param name="istransaction"></param>
        void Commit(bool istransaction = false);

        /// <summary>
        /// 关闭工作单元
        /// </summary>
        void Close();
    }
}
