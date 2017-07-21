using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        public object MethodInvoke(object instance,object[] paramsValue, MethodInfo methodInfo, string instanceTypeName = null)
        {
            string methidFullName = _methodContainerManager.GetMethodFullName(instance, methodInfo, instanceTypeName);
            Func<object, object[], object> methodFun = _methodContainerManager.GetMethodByMethodFullName(methidFullName);
            if (methodFun == null)
            {
                methodFun = _methodContainerManager.AddMethodContainer(methidFullName, methodInfo);
            }
            var retvalue = methodFun(instance, paramsValue);
            return retvalue;
        }
    }
}
