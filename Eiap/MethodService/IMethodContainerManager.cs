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
        Func<object, object[], object> AddMethodContainer(string methodFullName, MethodInfo methodInfo);

        /// <summary>
        /// 获取通用方法容器
        /// </summary>
        /// <param name="methodFullName"></param>
        /// <returns></returns>
        Func<object, object[], object> GetMethodByMethodFullName(string methodFullName);

        /// <summary>
        /// 根据方法和实例对象返回方法全名（可删除）
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodinfo"></param>
        /// <returns></returns>
        string GetMethodFullName(object instance, MethodInfo methodinfo);
    }
}
