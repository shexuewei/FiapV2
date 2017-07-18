
using System.Collections.Generic;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 领域事件管理接口
    /// </summary>
    public interface IDomainEventManager
    {
        /// <summary>
        /// 根据程序集注册领域事件关系
        /// </summary>
        /// <param name="assemblyList"></param>
        void Register(List<Assembly> assemblyList);

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TDomainEventData"></typeparam>
        /// <typeparam name="TDomainEventHandler"></typeparam>
        void SubscribeEvent<TDomainEventData, TDomainEventHandler>() 
            where TDomainEventData : IDomainEventData
            where TDomainEventHandler : IDomainEventHandler<TDomainEventData>;

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="TDomainEventData"></typeparam>
        /// <param name="domainEventData"></param>
        void Trigger<TDomainEventData>(TDomainEventData domainEventData) where TDomainEventData : IDomainEventData;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TDomainEventData"></typeparam>
        /// <typeparam name="TDomainEventHandler"></typeparam>
        void UnSubscribeEvent<TDomainEventData, TDomainEventHandler>()
            where TDomainEventData : IDomainEventData
            where TDomainEventHandler : IDomainEventHandler<TDomainEventData>;
    }
}
