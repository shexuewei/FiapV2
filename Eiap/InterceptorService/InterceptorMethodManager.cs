using System;
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// 拦截器方法管理
    /// </summary>
    public class InterceptorMethodManager : IInterceptorMethodManager
    {
        private readonly IInterceptorMethodContainerManager _InterceptorMethodContainerManager;
        public InterceptorMethodManager(IInterceptorMethodContainerManager interceptorMethodContainerManager)
        {
            _InterceptorMethodContainerManager = interceptorMethodContainerManager;
        }

        /// <summary>
        /// 注册拦截属性和拦截方法
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <param name="interceptorMethod"></param>
        public void RegisterAttibuteAndInterceptorMethod(Type interceptorMethodAttibute)
        {
            _InterceptorMethodContainerManager.RegisterAttibuteAndInterceptorMethod(interceptorMethodAttibute);
        }

        /// <summary>
        /// 根据拦截属性获取拦截方法
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <returns></returns>
        public List<Action<InterceptorMethodArgs>> GetInterceptorMethodList(Type interceptorMethodAttibute)
        {
            InterceptorMethodContainer container = _InterceptorMethodContainerManager.GetInterceptorMethodContainer(interceptorMethodAttibute);
            if (container != null)
            {
                return container.InterceptorMethodList;
            }
            return null;
        }
    }
}
