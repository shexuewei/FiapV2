using System;

namespace Eiap
{
    /// <summary>
    /// 领域事件容器
    /// </summary>
    public class DomainEventContainer
    {
        /// <summary>
        /// 领域事件类型句柄
        /// </summary>
        public RuntimeTypeHandle DomainEventDataTypeHandle { get; set; }

        /// <summary>
        /// 领域事件类型
        /// </summary>
        public Type DomainEventDataType { get { return Type.GetTypeFromHandle(DomainEventDataTypeHandle); } }

        /// <summary>
        /// 领域事件类型名称
        /// </summary>
        public string DomainEventDataTypeName { get { return DomainEventDataType.FullName; } }

        /// <summary>
        /// 领域事件处理类型句柄
        /// </summary>
        public RuntimeTypeHandle DomainEventHandlerTypeHandle { get; set; }

        /// <summary>
        /// 领域事件处理类型
        /// </summary>
        public Type DomainEventHandlerType { get { return Type.GetTypeFromHandle(DomainEventHandlerTypeHandle); } }

        /// <summary>
        /// 领域事件处理类型名称
        /// </summary>
        public string DomainEventHandlerTypeName { get { return DomainEventHandlerType.FullName; } }

        /// <summary>
        /// 领域事件处理接口类型句柄
        /// </summary>
        public RuntimeTypeHandle InterfaceDomainEventHandlerTypeHandle { get; set; }

        /// <summary>
        /// 领域事件处理接口类型
        /// </summary>
        public Type InterfaceDomainEventHandlerType { get { return Type.GetTypeFromHandle(InterfaceDomainEventHandlerTypeHandle); } }

        /// <summary>
        /// 领域事件处理接口名称
        /// </summary>
        public string InterfaceDomainEventHandlerName { get { return InterfaceDomainEventHandlerType.Namespace + "." + InterfaceDomainEventHandlerType.Name; } }
    }
}
