
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 反射方法管理接口
    /// </summary>
    public interface IMethodManager: IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 方法执行
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paramsValue"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        object MethodInvoke(object instance, object[] paramsValue, MethodInfo methodInfo);
    }
}
