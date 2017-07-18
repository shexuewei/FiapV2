
namespace Eiap
{
    /// <summary>
    /// 领域事件处理接口
    /// </summary>
    /// <typeparam name="TDomainEventData"></typeparam>
    public interface IDomainEventHandler<in TDomainEventData> : IRealtimeDependency, IDynamicProxyDisable 
        where TDomainEventData : IDomainEventData
    {
        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="eventData"></param>
        void ProcessEvent(TDomainEventData eventData);
    }
}
