
namespace Eiap
{
    /// <summary>
    /// 拦截器接口
    /// </summary>
    public interface IInterceptorMethod
    {

    }

    /// <summary>
    /// 执行前拦截接口
    /// </summary>
    public interface IInterceptorMethodBegin: IInterceptorMethod
    {
        void Execute(InterceptorMethodArgs args);
    }

    /// <summary>
    /// 执行后拦截
    /// </summary>
    public interface IInterceptorMethodEnd : IInterceptorMethod
    {
        void Execute(InterceptorMethodArgs args);
    }

    /// <summary>
    /// 异常拦截
    /// </summary>
    public interface IInterceptorMethodException : IInterceptorMethod
    {
        void Execute(InterceptorMethodArgs args);
    }
}
