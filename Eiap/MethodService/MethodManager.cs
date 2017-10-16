using System;
using System.Reflection;

namespace Eiap.MethodService
{
    /// <summary>
    /// 反射方法管理
    /// </summary>
    public class MethodManager : IMethodManager
    {
        private readonly IMethodContainerManager _methodContainerManager;

        public MethodManager(IMethodContainerManager methodContainerManager)
        {
            _methodContainerManager = methodContainerManager;
        }

        /// <summary>
        /// 方法执行
        /// </summary>
        /// <param name="methodFullName"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public object MethodInvoke(object instance,object[] paramsValue, MethodInfo methodInfo)
        {
            Func<object, object[], object> methodFun = _methodContainerManager.GetMethodByMethodFullName(methodInfo);
            if (methodFun == null)
            {
                methodFun = _methodContainerManager.AddMethodContainer(methodInfo);
            }
            var retvalue = methodFun(instance, paramsValue);
            return retvalue;
        }
    }
}
