using System;

namespace Eiap
{
    /// <summary>
    /// 拦截器特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class InterceptorMethodAttribute : Attribute
    {
        /// <summary>
        /// 特性优先级
        /// </summary>
        public virtual int Priority { get; set; }
    }

    /// <summary>
    /// 拦截器执行前特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class InterceptorMethodBeginAttibute : InterceptorMethodAttribute, IInterceptorMethodBegin
    {
        public virtual void Execute(InterceptorMethodArgs args)
        {
        }
    }

    /// <summary>
    /// 拦截器执行后特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class InterceptorMethodEndAttibute : InterceptorMethodAttribute, IInterceptorMethodEnd
    {
        public virtual void Execute(InterceptorMethodArgs args)
        {
            
        }
    }

    /// <summary>
    /// 拦截器执行异常特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class InterceptorMethodExceptionAttibute : InterceptorMethodAttribute, IInterceptorMethodException
    {
        public virtual void Execute(InterceptorMethodArgs args)
        {

        }
    }
}
