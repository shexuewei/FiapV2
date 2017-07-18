using System;
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// 拦截器方法容器管理基类
    /// </summary>
    public abstract class InterceptorMethodContainerManagerBase : IInterceptorMethodContainerManager
    {
        protected static IInterceptorMethodContainerManager _IInterceptorMethodContainerManager;

        protected Dictionary<string, InterceptorMethodContainer> _InterceptorMethodContainerList = null;

        protected InterceptorMethodContainerManagerBase()
        {
            _InterceptorMethodContainerList = new Dictionary<string, InterceptorMethodContainer>();
        }

        /// <summary>
        /// 判断是否有相同拦截方法
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <param name="interceptorMethod"></param>
        /// <returns></returns>
        protected bool IsExistSameInterceptorMethod(InterceptorMethodContainer interceptorMethodContainer, Action<InterceptorMethodArgs> interceptorMethod)
        {
            bool res = false;
            interceptorMethodContainer.InterceptorMethodList.ForEach(n =>
            {
                if (n.Method.Name == interceptorMethod.Method.Name)
                {
                    res = true;
                }
            });
            return res;
        }

        /// <summary>
        /// 判断是否有相同拦截特性
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <returns></returns>
        protected InterceptorMethodContainer IsExistSameInterceptorMethodAttibute(Type interceptorMethodAttibute)
        {
            InterceptorMethodContainer res = null;
            if (interceptorMethodAttibute != null)
            {
                if (_InterceptorMethodContainerList.ContainsKey(interceptorMethodAttibute.FullName))
                {
                    res = _InterceptorMethodContainerList[interceptorMethodAttibute.FullName];
                }
            }
            return res;
        }

        /// <summary>
        /// 注册特性和对应的拦截方法
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <param name="interceptorMethod"></param>
        public virtual void RegisterAttibuteAndInterceptorMethod(Type interceptorMethodAttibute) { }

        /// <summary>
        /// 根据特性名称获取拦截容器
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <returns></returns>
        public virtual InterceptorMethodContainer GetInterceptorMethodContainer(Type interceptorMethodAttibute) { return null; }
    }
}
