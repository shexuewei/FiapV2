using System;

namespace Eiap
{
    /// <summary>
    /// 动态代理容器
    /// </summary>
    public class DynamicProxyContainer
    {
        /// <summary>
        /// 动态代理类型句柄
        /// </summary>
        public RuntimeTypeHandle DynamicProxyTypeHandle { get; set; }

        /// <summary>
        /// 动态代理类型名称
        /// </summary>
        public string DynamicProxyTypeFullName { get; set; }

        /// <summary>
        /// 动态代理类型
        /// </summary>
        public Type DynamicProxyType {
            get {
                return Type.GetTypeFromHandle(DynamicProxyTypeHandle);
            }
        }
    }
}
