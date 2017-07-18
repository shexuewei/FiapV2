
using System.Collections.Generic;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 拦截器管理接口
    /// </summary>
    public interface IInterceptorManager: IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 根据程序集集合，注册拦截器
        /// </summary>
        /// <param name="assemblyList"></param>
        void Register(List<Assembly> assemblyList);
    }
}
