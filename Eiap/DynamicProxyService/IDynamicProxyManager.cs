using System;

namespace Eiap
{
    /// <summary>
    /// 动态代理管理
    /// </summary>
    public interface IDynamicProxyManager : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 创建动态代理类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objInstance"></param>
        /// <returns></returns>
        T Create<T>(object objInstance) where T : class;

        /// <summary>
        /// 创建动态代理类
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="objInstance"></param>
        /// <returns></returns>
        object Create(Type interfaceType, object objInstance);
    }
}
