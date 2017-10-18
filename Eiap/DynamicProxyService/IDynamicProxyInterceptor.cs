
using System;

namespace Eiap
{
    /// <summary>
    /// 动态代理拦截方法接口
    /// </summary>
    public interface IDynamicProxyInterceptor : IRealtimeDependency, IDynamicProxyDisable
    {
        object Invoke(object instance, string name, object[] parameters, Type[] parameterTypes, Type[] methodGenericArgumentTypes);
    }
}
