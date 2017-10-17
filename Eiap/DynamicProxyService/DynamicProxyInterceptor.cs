
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 动态代理拦截器
    /// </summary>
    public class DynamicProxyInterceptor : IDynamicProxyInterceptor
    {
        private List<Action<InterceptorMethodArgs>> _InterceptorActionList = null;
        private readonly IInterceptorMethodManager _InterceptorMethodManager;
        private readonly IMethodManager _methodManager;

        public DynamicProxyInterceptor(IInterceptorMethodManager interceptorMethodManager,
            IMethodManager methodManager)
        {
            _InterceptorMethodManager = interceptorMethodManager;
            _methodManager = methodManager;
            _InterceptorActionList = new List<Action<InterceptorMethodArgs>>();
        }

        public object Invoke(object instance, string name, object[] parameters)
        {
            object objres = null;
            MethodInfo methodinfo = null;
            InterceptorMethodArgs args = null;
            Stopwatch stopwatch = null;
            MethodInfo excuMethod = null;

            try
            {
                if (instance != null)
                {
                    Type instanceType = instance.GetType();
                    if (parameters == null)
                    {
                        methodinfo = instanceType.GetMethod(name, new Type[] { });
                    }
                    else
                    {
                        Type[] paraTypes = new Type[parameters.Length];
                        for (int i = 0; i < paraTypes.Length; i++)
                        {
                            paraTypes[i] = parameters[i].GetType();
                        }
                        methodinfo = instanceType.GetMethod(name, paraTypes);
                    }
                    if (methodinfo != null)
                    {
                        if (methodinfo.IsGenericMethod)
                        {
                            Type[] genericArgumentsList = methodinfo.GetGenericArguments().Select(m => m.DeclaringType).ToArray();
                            excuMethod = methodinfo.MakeGenericMethod(genericArgumentsList);
                        }
                        else
                        {
                            excuMethod = methodinfo;
                        }
                        if (excuMethod != null)
                        {
                            stopwatch = new Stopwatch();
                            stopwatch.Start();
                            args = new InterceptorMethodArgs { MethodName = name, MethodDateTime = DateTime.Now, MethodParameters = parameters, InstanceObject = instance };
                            InvokeBegin(methodinfo, args);
                            objres = _methodManager.MethodInvoke(instance, parameters, excuMethod);
                            stopwatch.Stop();
                            args.MethodExecute = stopwatch.Elapsed.TotalMilliseconds;
                            args.ReturnValue = objres;
                            InvokeEnd(methodinfo, args);
                        }
                        else
                        {
                            //TODO:自定义异常
                            throw new Exception("No Method");
                        }
                    }
                }
                else
                {
                    //TODO:自定义异常
                    throw new Exception("No Instance Object");
                }
            }
            catch (Exception ex)
            {
                if (args != null && methodinfo != null)
                {
                    args.MethodException = ex;
                    InvokeException(methodinfo, args);
                }
                throw ex;
            }
            return objres;
        }

        /// <summary>
        /// 清理拦截方法
        /// </summary>
        private void ClearInterceptorActionList()
        {
            if (_InterceptorActionList != null && _InterceptorActionList.Count > 0)
            {
                _InterceptorActionList.Clear();
            }
        }

        /// <summary>
        /// 方法前拦截方法
        /// </summary>
        /// <param name="methodinfo"></param>
        /// <param name="args"></param>
        /// <returns>True：成功执行拦截方法；False：拦截方法终止</returns>
        private void InvokeBegin(MethodInfo methodinfo, InterceptorMethodArgs args)
        {
            ClearInterceptorActionList();
            methodinfo.GetCustomAttributes(typeof(InterceptorMethodBeginAttibute), true).ToList().ForEach(m => {
                List<Action<InterceptorMethodArgs>> tmpInterceptorActionList = _InterceptorMethodManager.GetInterceptorMethodList(m.GetType());
                if (tmpInterceptorActionList != null && tmpInterceptorActionList.Count > 0)
                {
                    _InterceptorActionList.AddRange(tmpInterceptorActionList);
                }
            });
            foreach (Action<InterceptorMethodArgs> interceptorActionItem in _InterceptorActionList)
            {
                interceptorActionItem(args);
            }
        }

        /// <summary>
        /// 方法后拦截方法
        /// </summary>
        /// <param name="methodinfo"></param>
        /// <param name="args"></param>
        private void InvokeEnd(MethodInfo methodinfo, InterceptorMethodArgs args)
        {
            ClearInterceptorActionList();
            methodinfo.GetCustomAttributes(typeof(InterceptorMethodEndAttibute), true).ToList().ForEach(m => {
                List<Action<InterceptorMethodArgs>> tmpInterceptorActionList = _InterceptorMethodManager.GetInterceptorMethodList(m.GetType());
                if (tmpInterceptorActionList != null && tmpInterceptorActionList.Count > 0)
                {
                    _InterceptorActionList.AddRange(tmpInterceptorActionList);
                }
            });

            foreach (Action<InterceptorMethodArgs> interceptorActionItem in _InterceptorActionList)
            {
                interceptorActionItem(args);
            }
        }

        /// <summary>
        /// 方法异常拦截
        /// </summary>
        /// <param name="methodinfo"></param>
        /// <param name="args"></param>
        private void InvokeException(MethodInfo methodinfo, InterceptorMethodArgs args)
        {
            ClearInterceptorActionList();
            methodinfo.GetCustomAttributes(typeof(InterceptorMethodExceptionAttibute), true).ToList().ForEach(m => {
                List<Action<InterceptorMethodArgs>> tmpInterceptorActionList = _InterceptorMethodManager.GetInterceptorMethodList(m.GetType());
                if (tmpInterceptorActionList != null && tmpInterceptorActionList.Count > 0)
                {
                    _InterceptorActionList.AddRange(tmpInterceptorActionList);
                }
            });
            foreach (Action<InterceptorMethodArgs> interceptorActionItem in _InterceptorActionList)
            {
                interceptorActionItem(args);
            }
        }
    }
}
