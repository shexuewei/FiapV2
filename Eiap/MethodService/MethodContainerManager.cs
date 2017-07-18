﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Eiap
{
    public class MethodContainerManager : IMethodContainerManager
    {
        private ConcurrentDictionary<string, MethodContainer> _methodContainerList = null;

        public MethodContainerManager()
        {
            _methodContainerList = new ConcurrentDictionary<string, MethodContainer>();
        }

        /// <summary>
        /// 添加动态代理方法容器
        /// </summary>
        /// <param name="container"></param>
        public Func<object, object[], object> AddMethodContainer(string methodFullName, MethodInfo methodInfo)
        {
            MethodContainer container = null;
            if (!_methodContainerList.ContainsKey(methodFullName))
            {
                container = new MethodContainer
                {
                    Method = GetExecuteDelegate(methodInfo)
                };
                _methodContainerList.TryAdd(methodFullName, container);
            }
            else
            {
                container = _methodContainerList[methodFullName];
            }
            return container.Method;
        }

        /// <summary>
        /// 根据方法和实例对象返回方法全名
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodinfo"></param>
        /// <returns></returns>
        public string GetMethodFullName(object instance, MethodInfo methodinfo)
        {
            StringBuilder methidFullNameStrBui = new StringBuilder();
            methidFullNameStrBui.Append(instance.GetType().FullName);
            methidFullNameStrBui.Append(".");
            methidFullNameStrBui.Append(methodinfo.Name);
            ParameterInfo[] parametersList = methodinfo.GetParameters();
            foreach (ParameterInfo parameterItem in parametersList)
            {
                methidFullNameStrBui.Append(".");
                methidFullNameStrBui.Append(parameterItem.ParameterType.Name);
            }
            return methidFullNameStrBui.ToString();
        }

        /// <summary>
        /// 获取动态代理方法容器
        /// </summary>
        /// <param name="dynamicProxyMethodFullName"></param>
        /// <returns></returns>
        public Func<object, object[], object> GetMethodByMethodFullName(string methodFullName)
        {
            if (_methodContainerList.ContainsKey(methodFullName))
            {
                return _methodContainerList[methodFullName].Method;
            }
            return null;
        }

        /// <summary>
        /// 根据MethodInfo转表达式树，并构造成委托
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            Expression instanceCast = methodInfo.IsStatic ? null : Expression.Convert(instanceParameter, methodInfo.ReflectedType);
            MethodCallExpression methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);
            if (methodCall.Type == typeof(void))
            {
                Expression<Action<object, object[]>> lambda = Expression.Lambda<Action<object, object[]>>(methodCall, instanceParameter, parametersParameter);

                Action<object, object[]> execute = lambda.Compile();
                return (instance, parameters) =>
                {
                    execute(instance, parameters);
                    return null;
                };
            }
            else
            {
                {
                    UnaryExpression castMethodCall = Expression.Convert(methodCall, typeof(object));
                    Expression<Func<object, object[], object>> lambda = Expression.Lambda<Func<object, object[], object>>(castMethodCall, instanceParameter, parametersParameter);
                    return lambda.Compile();
                }
            }
        }
    }
}
