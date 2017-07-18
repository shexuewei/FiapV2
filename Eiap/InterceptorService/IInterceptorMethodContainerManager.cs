
using System;

namespace Eiap
{
    /// <summary>
    /// 拦截器方法容器管理接口
    /// </summary>
    public interface IInterceptorMethodContainerManager : ISingletonDependency,IDynamicProxyDisable
    {
        /// <summary>
        /// 注册拦截器特性和拦截方法
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        void RegisterAttibuteAndInterceptorMethod(Type interceptorMethodAttibute);

        /// <summary>
        /// 获取拦截器容器
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <returns></returns>
        InterceptorMethodContainer GetInterceptorMethodContainer(Type interceptorMethodAttibute);
    }
}
