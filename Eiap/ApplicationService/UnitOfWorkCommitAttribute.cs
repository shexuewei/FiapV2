
using System;

namespace Eiap
{
    /// <summary>
    /// 工作单元特性（工作单元拦截器）
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class UnitOfWorkCommitAttribute : InterceptorMethodEndAttibute
    {
        public UnitOfWorkCommitAttribute()
        {
            this.Priority = -1;
        }


        /// <summary>
        /// 执行工作单元提交
        /// </summary>
        /// <param name="args"></param>
        public override void Execute(InterceptorMethodArgs args)
        {
            IAppService appinstance = args.InstanceObject as IAppService;
            appinstance.CurrentUnitOfWork.Commit();
        }
    }
}
