using System;

namespace Eiap
{
    /// <summary>
    /// 拦截器方法参数
    /// </summary>
    public class InterceptorMethodArgs
    {
        /// <summary>
        /// 当前实例
        /// </summary>
        public object InstanceObject { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 方法参数
        /// </summary>
        public object[] MethodParameters { get; set; }

        /// <summary>
        /// 方法执行日期
        /// </summary>
        public DateTime MethodDateTime { get; set; }

        /// <summary>
        /// 方法异常信息
        /// </summary>
        public Exception MethodException { get; set; }

        /// <summary>
        /// 方法执行时间
        /// </summary>
        public double? MethodExecute { get; set; }

        /// <summary>
        /// 方法返回对象
        /// </summary>
        public object ReturnValue { get; set; }
    }
}
