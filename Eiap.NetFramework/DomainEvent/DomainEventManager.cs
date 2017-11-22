using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eiap.NetFramework
{
    //TODO:没有实现一个DomainEventHandle对应多个DomainEventHandle<EventData>
    public class DomainEventManager : IDomainEventManager
    {
        private static IDomainEventManager _Container = null;
        private IDomainEventContainerManager _DomainEventContainerManager = null;

        private DomainEventManager()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Initialization()
        {
            _DomainEventContainerManager = DomainEventContainerManager.Instance;
        }

        /// <summary>
        /// 单例对象
        /// </summary>
        public static IDomainEventManager Instance
        {
            get
            {
                if (_Container == null)
                {
                    _Container = new DomainEventManager();
                }
                return _Container;
            }
        }

        /// <summary>
        /// 通过程序集自动订阅事件
        /// </summary>
        /// <param name="assemblyList"></param>
        public void Register(List<Assembly> assemblyList)
        {
            foreach (Assembly assemblyItem in assemblyList)
            {
                List<Type> typeList = assemblyItem.GetTypes().Where(m => typeof(IDomainEventData).IsAssignableFrom(m) && typeof(IDomainEventData).FullName != m.FullName).ToList();
                foreach (Type typeItem in typeList)
                {
                    List<Type> classList = typeList[0].Assembly.GetTypes().Where(m => m.IsClass && m.FullName != typeItem.FullName).ToList();
                    foreach (Type classItem in classList)
                    {
                        Type[] interfaceList = classItem.GetInterfaces();
                        foreach (Type interfaceItem in interfaceList)
                        {
                            if (interfaceItem.GetGenericArguments().Length == 1 && interfaceItem.GetGenericArguments()[0].FullName == typeItem.FullName)
                            {
                                _DomainEventContainerManager.AddDomainEventContList(interfaceItem.GetGenericArguments()[0], interfaceItem, classItem);
                                //object handler = Activator.CreateInstance(classItem);
                                //string methodname = typeof(IDomainEventHandler<IDomainEventData>).GetMethods()[0].Name;
                                //handler.GetType().GetMethod(methodname).Invoke(handler, new object[] { data });
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TDomainEventData"></typeparam>
        /// <typeparam name="TDomainEventHandler"></typeparam>
        public void SubscribeEvent<TDomainEventData, TDomainEventHandler>()
            where TDomainEventData : IDomainEventData
            where TDomainEventHandler : IDomainEventHandler<TDomainEventData>
        {
            Type domainEventDataType = typeof(TDomainEventData);
            Type classItem = typeof(TDomainEventHandler);
            classItem.GetInterfaces().ToList().ForEach(m => {
                if (m.GetGenericArguments().Length == 1 && m.GetGenericArguments()[0].FullName == domainEventDataType.FullName)
                {
                    _DomainEventContainerManager.AddDomainEventContList(domainEventDataType, m, classItem);
                }
            });
            
        }

        /// <summary>
        /// 出发事件
        /// </summary>
        /// <typeparam name="TDomainEventData"></typeparam>
        /// <param name="domainEventData"></param>
        public void Trigger<TDomainEventData>(TDomainEventData domainEventData) where TDomainEventData : IDomainEventData
        {
            List<DomainEventContainer> domainEventContList = _DomainEventContainerManager.GetDomainEventContList(typeof(TDomainEventData));
            domainEventContList.ForEach(m => {
                object handler = DependencyManager.Instance.Resolver(m.DomainEventHandlerType);
                //TODO:可以用更合理的方式获取处理方法名称
                MethodInfo processEventMethodInfo = m.InterfaceDomainEventHandlerType.GetMethod("ProcessEvent", new Type[] {typeof(TDomainEventData) });
                Type methodParaType = processEventMethodInfo.GetParameters()[0].ParameterType;
                m.DomainEventHandlerType.GetMethod(processEventMethodInfo.Name, new Type[] { methodParaType }).Invoke(handler, new object[] { domainEventData });
            });
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TDomainEventData"></typeparam>
        /// <typeparam name="TDomainEventHandler"></typeparam>
        public void UnSubscribeEvent<TDomainEventData, TDomainEventHandler>()
            where TDomainEventData : IDomainEventData
            where TDomainEventHandler : IDomainEventHandler<TDomainEventData>
        {
            throw new NotImplementedException();
        }
    }
}
