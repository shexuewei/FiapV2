using System;
using System.Collections.Generic;
using System.Linq;

namespace Eiap.NetFramework
{
    public class DomainEventContainerManager : IDomainEventContainerManager
    {
        private static IDomainEventContainerManager _DomainEventContainerManager;
        private List<DomainEventContainer> _DomainEventContList = null;

        private DomainEventContainerManager()
        {
            _DomainEventContList = new List<DomainEventContainer>();
        }

        public static IDomainEventContainerManager Instance
        {
            get
            {
                if (_DomainEventContainerManager == null)
                {
                    _DomainEventContainerManager = new DomainEventContainerManager();
                }
                return _DomainEventContainerManager;
            }
        }

        /// <summary>
        /// 添加领域事件关系
        /// </summary>
        /// <param name="domainEventDataTypeItem"></param>
        /// <param name="interfaceItem"></param>
        /// <param name="classItem"></param>
        public void AddDomainEventContList(Type domainEventDataTypeItem, Type interfaceItem, Type classItem)
        {
            foreach (DomainEventContainer containerItem in _DomainEventContList)
            {
                if (containerItem.DomainEventDataTypeName == domainEventDataTypeItem.FullName
                    && containerItem.InterfaceDomainEventHandlerName == interfaceItem.Namespace+ interfaceItem.Name
                    && containerItem.DomainEventHandlerTypeName == classItem.FullName)
                {
                    return;
                }
            }
            _DomainEventContList.Add(new DomainEventContainer()
            {
                DomainEventDataTypeHandle = domainEventDataTypeItem.TypeHandle,
                DomainEventHandlerTypeHandle = classItem.TypeHandle,
                InterfaceDomainEventHandlerTypeHandle = interfaceItem.TypeHandle
            });
        }

        /// <summary>
        /// 根据领域对象获取对应的处理类型
        /// </summary>
        /// <param name="domainEventDataTypeItem"></param>
        /// <returns></returns>
        public List<DomainEventContainer> GetDomainEventContList(Type domainEventDataTypeItem)
        {
            return _DomainEventContList.Where(m => m.DomainEventDataTypeName == domainEventDataTypeItem.FullName).ToList();
        }

        /// <summary>
        /// 移除领域事件关系
        /// </summary>
        /// <param name="domainEventDataTypeItem"></param>
        /// <param name="interfaceItem"></param>
        /// <param name="classItem"></param>
        public void RemoveDomainEventContList(Type domainEventDataTypeItem, Type interfaceItem, Type classItem)
        {

        }
    }
}
