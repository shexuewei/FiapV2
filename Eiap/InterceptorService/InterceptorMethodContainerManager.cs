using System;

namespace Eiap
{
    /// <summary>
    /// 拦截方法容器管理
    /// </summary>
    public class InterceptorMethodContainerManager : InterceptorMethodContainerManagerBase
    {
        /// <summary>
        /// 注册特性和对应的拦截方法
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        public override void RegisterAttibuteAndInterceptorMethod(Type interceptorMethodAttibute)
        {
            if (typeof(InterceptorMethodBeginAttibute).IsAssignableFrom(interceptorMethodAttibute)&& typeof(IInterceptorMethod).IsAssignableFrom(interceptorMethodAttibute))
            {
                object interceptorMethodAttibuteInstance = Activator.CreateInstance(interceptorMethodAttibute);
                Action<InterceptorMethodArgs> interceptorMethod = null;
                if (typeof(IInterceptorMethodBegin).IsAssignableFrom(interceptorMethodAttibuteInstance.GetType()))
                {
                    interceptorMethod = ((IInterceptorMethodBegin)interceptorMethodAttibuteInstance).Execute;
                }
                else if (typeof(IInterceptorMethodEnd).IsAssignableFrom(interceptorMethodAttibuteInstance.GetType()))
                {
                    interceptorMethod = ((IInterceptorMethodEnd)interceptorMethodAttibuteInstance).Execute;
                }
                else if (typeof(IInterceptorMethodException).IsAssignableFrom(interceptorMethodAttibuteInstance.GetType()))
                {
                    interceptorMethod = ((IInterceptorMethodException)interceptorMethodAttibuteInstance).Execute;
                }

                InterceptorMethodContainer containter = IsExistSameInterceptorMethodAttibute(interceptorMethodAttibute);
                if (containter != null)
                {
                    if (!IsExistSameInterceptorMethod(containter, interceptorMethod))
                    {
                        containter.InterceptorMethodList.Add(interceptorMethod);
                    }
                    else
                    {
                        //TODO:存在相同interceptorMethod
                    }
                }
                else
                {
                    _InterceptorMethodContainerList.Add(interceptorMethodAttibute.FullName, new InterceptorMethodContainer { InterceptorMethodAttibuteTypeHandle = interceptorMethodAttibute.TypeHandle, InterceptorMethodList = { interceptorMethod } });
                }
            }
        }

        /// <summary>
        /// 根据特性名称获取拦截容器
        /// </summary>
        /// <param name="interceptorMethodAttibute"></param>
        /// <returns></returns>
        public override InterceptorMethodContainer GetInterceptorMethodContainer(Type interceptorMethodAttibute)
        {
            InterceptorMethodContainer interceptorMethodContainer = null;
            if (typeof(InterceptorMethodBeginAttibute).IsAssignableFrom(interceptorMethodAttibute) && typeof(IInterceptorMethod).IsAssignableFrom(interceptorMethodAttibute))
            {
                interceptorMethodContainer = IsExistSameInterceptorMethodAttibute(interceptorMethodAttibute);
            }
            return interceptorMethodContainer;
        }
    }
}
