using System;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 方法容器管理接口
    /// </summary>
    public interface IMethodContainerManager : ISingletonDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 添加通用方法容器
        /// </summary>
        /// <param name="methodFullName"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        Func<object, object[], object> AddMethodContainer(MethodInfo methodInfo);

        /// <summary>
        /// 获取通用方法容器
        /// </summary>
        /// <param name="methodFullName"></param>
        /// <returns></returns>
        Func<object, object[], object> GetMethodByMethodFullName(MethodInfo methodInfo);
    }
}
