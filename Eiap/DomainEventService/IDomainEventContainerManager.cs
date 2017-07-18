using System;
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// 领域事件容器管理接口
    /// </summary>
    public interface IDomainEventContainerManager
    {
        /// <summary>
        /// 添加领域事件关系
        /// </summary>
        /// <param name="domainEventDataTypeItem"></param>
        /// <param name="interfaceItem"></param>
        /// <param name="classItem"></param>
        void AddDomainEventContList(Type domainEventDataTypeItem, Type interfaceItem, Type classItem);

        /// <summary>
        /// 根据领域事件类型获取领域事件容器集合
        /// </summary>
        /// <param name="domainEventDataTypeItem"></param>
        /// <returns></returns>
        List<DomainEventContainer> GetDomainEventContList(Type domainEventDataTypeItem);

        /// <summary>
        /// 移除领域事件关系
        /// </summary>
        /// <param name="domainEventDataTypeItem"></param>
        /// <param name="interfaceItem"></param>
        /// <param name="classItem"></param>
        void RemoveDomainEventContList(Type domainEventDataTypeItem, Type interfaceItem, Type classItem);
    }
}
