using System;
using System.Collections.Generic;
using System.Linq;

namespace Eiap
{
    /// <summary>
    /// 依赖注入容器管理
    /// </summary>
    public class DependencyContainerManager : DependencyContainerManagerBase
    {
        public static IDependencyContainerManager Instance
        {
            get
            {
                if (_DependencyContainerManager == null)
                {
                    _DependencyContainerManager = new DependencyContainerManager();
                }
                return _DependencyContainerManager;
            }
        }

        /// <summary>
        /// 注册依赖接口和依赖实现类
        /// </summary>
        /// <param name="dependencyInterface"></param>
        /// <param name="dependencyInterfaceClass"></param>
        public override void RegisterDependencyInterfaceClass(Type dependencyInterface, Type dependencyInterfaceClass, bool isDirectRelation = false)
        {
            if (!IsExistSameDependencyInterfaceClass(dependencyInterface, dependencyInterfaceClass))
            {
                DependencyContainer cont = new DependencyContainer();
                cont.DependencyInterfaceClassTypeHandle = dependencyInterfaceClass.TypeHandle;
                cont.DependencyInterfaceTypeHandle = dependencyInterface.TypeHandle;
                cont.IsDirectRelation = isDirectRelation;
                _DepeDicList.Add(cont);
            }
        }


        /// <summary>
        /// 加载类和依赖实现类
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="interfaceTypeList"></param>
        public override void RegisterClassAndDependencyInterfaceClass(Type classType, List<Type> interfaceTypeList)
        {
            for (int i = 0; i < interfaceTypeList.Count; i++)
            {
                if (!IsExistSameDependencyInterfaceClass(interfaceTypeList[i], classType))
                {
                    bool isDirectRelation = true;
                    for (int j = 0; j < interfaceTypeList.Count; j++)
                    {
                        if (interfaceTypeList[i].FullName == interfaceTypeList[j].FullName)
                        {
                            continue;
                        }
                        if (interfaceTypeList[j].IsAssignableFrom(interfaceTypeList[i]))
                        {
                            isDirectRelation = false;
                        }
                    }
                    if (isDirectRelation)
                    {
                        RegisterDependencyInterfaceClass(interfaceTypeList[i], classType, true);
                    }
                    else
                    {
                        RegisterDependencyInterfaceClass(interfaceTypeList[i], classType, false);
                    }
                }
            }
        }

        /// <summary>
        /// 根据接口和实现类返回依赖容器
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public override DependencyContainer GetDependencyContainer(Type interfaceType, Type entityType = null)
        {
            DependencyContainer container = null;
            if (interfaceType != null && entityType != null)
            {
                container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceName == interfaceType.FullName && m.DependencyInterfaceClassName == entityType.FullName);
                if (interfaceType.IsGenericType)
                {
                    container = _DepeDicList.FirstOrDefault(m => m.DependencyInterface.Namespace + "." + m.DependencyInterface.Name == interfaceType.Namespace + "." + interfaceType.Name
                        && m.DependencyInterfaceClass.Namespace + "." + m.DependencyInterfaceClass.Name == entityType.Namespace + "." + entityType.Name);
                }
            }
            if (container == null)
            {
                if (interfaceType != null)
                {
                    if (interfaceType.IsGenericType)
                    {
                        container = _DepeDicList.FirstOrDefault(m => m.DependencyInterface.Namespace + "." + m.DependencyInterface.Name == interfaceType.Namespace + "." + interfaceType.Name && m.IsDirectRelation);
                        if (container == null)
                        {
                            container = _DepeDicList.FirstOrDefault(m => m.DependencyInterface.Namespace + "." + m.DependencyInterface.Name == interfaceType.Namespace + "." + interfaceType.Name);
                        }
                        
                    }
                    else
                    {
                        container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceName == interfaceType.FullName && m.IsDirectRelation);
                        if (container == null)
                        {
                            container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceName == interfaceType.FullName);
                        }
                    }
                }
                else if (entityType != null)
                {
                    if (entityType.IsGenericType)
                    {
                        container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceClass.Namespace + "." + m.DependencyInterfaceClass.Name == entityType.Namespace + "." + entityType.Name && m.IsDirectRelation);
                        if (container == null)
                        {
                            container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceClass.Namespace + "." + m.DependencyInterfaceClass.Name == entityType.Namespace + "." + entityType.Name);
                        }
                    }
                    else
                    {
                        container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceClassName == entityType.FullName && m.IsDirectRelation);
                        if (container == null)
                        {
                            container = _DepeDicList.FirstOrDefault(m => m.DependencyInterfaceClassName == entityType.FullName);
                        }
                    }
                }
            }
            return container;
        }
    }
}
