using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Messaging;


namespace Eiap
{
    //TODO:缺少构造函数中泛型参数的个数和类型的比对
    public class DependencyManager : IDependencyManager
    {
        private static IDependencyManager _Manager = null;
        private IDependencyContainerManager _DependencyContainerManager = null;
        private List<object> singletonList = null;

        private DependencyManager()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Initialization()
        {
            _DependencyContainerManager = DependencyContainerManager.Instance;
            singletonList = new List<object>();
        }

        /// <summary>
        /// 单例对象
        /// </summary>
        public static IDependencyManager Instance
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new DependencyManager();
                }
                return _Manager;
            }
        }

        /// <summary>
        /// 注册依赖接口和依赖实现类
        /// </summary>
        /// <param name="dependencyInterface"></param>
        /// <param name="dependencyInterfaceClass"></param>
        public virtual void Register(Type dependencyInterface, Type dependencyInterfaceClass)
        {
            _DependencyContainerManager.RegisterDependencyInterfaceClass(dependencyInterface, dependencyInterfaceClass, true);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="tEntity">对象类型</param>
        /// <param name="consParas">构造参数</param>
        /// <param name="lifeCycle">对象生命周期</param>
        /// <param name="genArguments">泛型对象的类型参数</param>
        /// <returns></returns>
        public virtual object Resolver(Type tEntity, object[] consParas, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null, Type[] genArgumentList = null)
        {
            object t = null;
            if (tEntity != null)
            {
                if (tEntity.IsInterface)
                {
                    DependencyContainer container = _DependencyContainerManager.GetDependencyContainer(tEntity);
                    if (container != null)
                    {
                        Type classEntity = container.DependencyInterfaceClass;
                        if (classEntity != null)
                        {
                            if (tEntity.IsGenericType && genArgumentList == null)
                            {
                                genArgumentList = tEntity.GetGenericArguments();
                            }
                            t = Resolver(classEntity, consParas, lifeCycle, genArguments, genArgumentList);
                        }
                        //TODO:生成动态代理对象（可返回空类型对象）
                        //TODO:目前只根据接口生成动态代理
                        if (!typeof(IDynamicProxyDisable).IsAssignableFrom(tEntity))
                        {
                            t = this.Resolver<IDynamicProxyManager>().Create(tEntity, t);
                        }
                    }
                }
                else
                {
                    if (tEntity.IsGenericType && genArgumentList == null)
                    {
                        genArgumentList = tEntity.GetGenericArguments();
                    }
                    if (typeof(IRealtimeDependency).IsAssignableFrom(tEntity))
                    {
                        t = ResolverBy(tEntity, consParas, genArguments, genArgumentList);
                    }
                    else if (typeof(ISingletonDependency).IsAssignableFrom(tEntity))
                    {
                        t = GetSingleton(tEntity, consParas, genArguments);
                    }
                    else if (typeof(IContextDependency).IsAssignableFrom(tEntity))
                    {
                        t = GetContext(tEntity, consParas, genArguments);
                    }
                    else if (lifeCycle == ObjectLifeCycle.Realtime)
                    {
                        t = ResolverBy(tEntity, consParas, genArguments, genArgumentList);
                    }
                    else if (lifeCycle == ObjectLifeCycle.Singleton)
                    {
                        t = GetSingleton(tEntity, consParas, genArguments);
                    }
                    else if (lifeCycle == ObjectLifeCycle.Context)
                    {
                        t = GetContext(tEntity, consParas, genArguments);
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="tEntity">对象类型</param>
        /// <param name="lifeCycle">对象生命周期</param>
        /// <param name="genArguments">泛型对象的类型参数</param>
        /// <returns></returns>
        public virtual object Resolver(Type tEntity, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null, Type[] genArgumentList = null)
        {
            return Resolver(tEntity, new object[] { }, lifeCycle, genArguments, genArgumentList);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="entiType">对象类型</param>
        /// <param name="consParas">构造参数</param>
        /// <param name="genArguments">泛型对象的类型参数</param>
        /// <returns></returns>
        private object ResolverBy(Type entiType, object[] consParas, Type[] genArguments = null, Type[] genArgumentList = null)
        {
            object t = null;

            #region 构造函数
            ConstructorInfo[] consList = entiType.GetConstructors();
            foreach (ConstructorInfo consinfo in consList)
            {
                ParameterInfo[] paraList = consinfo.GetParameters();
                int paracount = paraList.Count();
                object[] paraobj = new object[paracount];
                int count = 0;
                foreach (ParameterInfo parainfo in paraList)
                {
                    object depeobj = null;
                    DependencyContainer container = _DependencyContainerManager.GetDependencyContainer(parainfo.ParameterType);
                    if (container != null)
                    {
                        if (container.DependencyInterfaceClass.IsGenericType)
                        {
                            Type[] genericArguments = null;
                            if (genArguments == null)
                            {
                                genericArguments = parainfo.ParameterType.GetGenericArguments();
                                if (genericArguments.Length == 0)
                                {
                                    genericArguments = entiType.GetGenericArguments();
                                }
                                else
                                {
                                    bool isnull = false;
                                    foreach (Type conGetType in genericArguments)
                                    {
                                        if (conGetType.FullName == null)
                                        {
                                            isnull = true;
                                            break;
                                        }
                                    }
                                    if (isnull)
                                    {
                                        genericArguments = entiType.GetGenericArguments();
                                    }
                                }
                            }
                            else
                            {
                                genericArguments = genArguments;
                            }
                            depeobj = this.Resolver(container.DependencyInterfaceClass, genArguments: genericArguments, genArgumentList: genArgumentList);
                        }
                        else
                        {
                            depeobj = this.Resolver(container.DependencyInterfaceClass);
                        }
                        paraobj[count] = depeobj;
                        count++;
                    }
                    //构造可空对象
                    if (depeobj == null)
                    {
                        paraobj[count] = depeobj;
                        count++;
                    }
                }
                foreach (object tmpobj in consParas)
                {
                    paraobj[count] = tmpobj;
                    count++;
                }
                if (count == paracount)
                {
                    object obj = null;
                    if (genArguments == null)
                    {
                        if (entiType.IsGenericType)
                        {
                            Type objtype = entiType.GetGenericTypeDefinition().MakeGenericType(genArgumentList);
                            obj = Activator.CreateInstance(objtype, paraobj);
                        }
                        else
                        { 
                            obj = consinfo.Invoke(paraobj); 
                        }
                    }
                    else
                    {
                        //TODO:匹配泛型参数类型
                        Type genType = consinfo.DeclaringType.GetGenericTypeDefinition();
                        Type objtype = null;
                        if (genArgumentList != null)
                        {
                            objtype = genType.MakeGenericType(genArgumentList);
                        }
                        else
                        {
                            objtype = genType.MakeGenericType(genArguments);
                        }
                        obj = Activator.CreateInstance(objtype, paraobj);
                    }
                    t = obj;
                    #region  属性注入
                    if (t != null)
                    {
                        PropertyInfo[] propList = t.GetType().GetProperties();
                        foreach (PropertyInfo propInfo in propList)
                        {
                            if ((typeof(IRealtimeDependency).IsAssignableFrom(propInfo.PropertyType) || typeof(ISingletonDependency).IsAssignableFrom(propInfo.PropertyType) || typeof(IContextDependency).IsAssignableFrom(propInfo.PropertyType)) && typeof(IPropertyDependency).IsAssignableFrom(propInfo.PropertyType))
                            {
                                object propObject = Resolver(propInfo.PropertyType);
                                IMethodManager methodManager = this.Resolver<IMethodManager>();
                                methodManager.MethodInvoke(t, new object[] { propObject }, propInfo.GetSetMethod());
                            }
                        }
                    }
                    #endregion
                    break;
                }
            }
            #endregion

            return t;
        }

        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <param name="tEntity"></param>
        /// <param name="consParas"></param>
        /// <param name="genArguments"></param>
        /// <returns></returns>
        private object GetSingleton(Type tEntity, object[] consParas,  Type[] genArguments = null)
        {
            foreach (object obj in singletonList)
            {
                if (obj.GetType().FullName == tEntity.FullName)
                {
                    return obj;
                }
            }
            object t = ResolverBy(tEntity, consParas, genArguments);
            singletonList.Add(t);
            return t;
        }

        /// <summary>
        /// 获取上下文对象
        /// </summary>
        /// <param name="tEntity"></param>
        /// <param name="consParas"></param>
        /// <param name="genArguments"></param>
        /// <returns></returns>
        private object GetContext(Type tEntity, object[] consParas, Type[] genArguments = null)
        {
            object t = CallContext.LogicalGetData(tEntity.FullName);
            if (t == null)
            {
                t = ResolverBy(tEntity, consParas, genArguments);
                SetContext(t);
            }
            return t;
        }

        /// <summary>
        /// 设置上下文对象
        /// </summary>
        /// <param name="obj"></param>
        private void SetContext(object obj)
        {
            CallContext.LogicalSetData(obj.GetType().FullName, obj);
        }

        /// <summary>
        /// 释放上下文对象
        /// </summary>
        /// <param name="tEntity"></param>
        public virtual void FreeContext(Type tEntity)
        {
            CallContext.FreeNamedDataSlot(tEntity.FullName);
        }

        /// <summary>
        /// 注册依赖接口和依赖实现类，非常耗时
        /// </summary>
        /// <param name="assemblyList"></param>
        public virtual void Register(List<Assembly> assemblyList)
        {
            //从所有程序集中注册
            RegisterAllAssembly(assemblyList);
        }

        /// <summary>
        /// 根据程序集集合，注册依赖接口和依赖实现类
        /// </summary>
        /// <param name="assemblyList"></param>
        private void RegisterAllAssembly(List<Assembly> assemblyList)
        {
            assemblyList.ForEach(assemblyItem =>
            {
                List<Type> classTypeList = assemblyItem.GetTypes().Where(m => m.IsClass && !m.IsAbstract).ToList();
                classTypeList.ForEach(classItem =>
                {
                    if (classItem.Name.IndexOf("ConfigurationContainerManager") >= 0)
                    {
                    }
                    List<Type> interfaceTypeList = classItem.GetInterfaces()
                        .Where(m => (typeof(IRealtimeDependency).IsAssignableFrom(m) && typeof(IRealtimeDependency).FullName != m.FullName)
                       || (typeof(ISingletonDependency).IsAssignableFrom(m) && typeof(ISingletonDependency).FullName != m.FullName)
                       || (typeof(IContextDependency).IsAssignableFrom(m) && typeof(IContextDependency).FullName != m.FullName)).ToList();
                    _DependencyContainerManager.RegisterClassAndDependencyInterfaceClass(classItem, interfaceTypeList);
                });
            });
        }

        /// <summary>
        /// 释放单例对象
        /// </summary>
        public virtual void FreeSingleton()
        {
            int count = singletonList.Count();
            for (int i = 0; i < count; i++)
            {
                singletonList[i] = null;
            }
            singletonList.Clear();
        }

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        public TEntity Resolver<TEntity>(int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null)
        {
            return (TEntity)Resolver(typeof(TEntity), lifeCycle, genArguments);
        }

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <param name="consParas">构造参数</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        public TEntity Resolver<TEntity>(object[] consParas, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null)
        {
            return (TEntity)Resolver(typeof(TEntity), consParas, lifeCycle, genArguments);
        }

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <typeparam name="TInterface">实例接口类型</typeparam>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        public TInterface Resolver<TEntity, TInterface>(int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null) where TEntity : new()
        {
            return (TInterface)Resolver(typeof(TEntity), lifeCycle, genArguments);
        }

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <typeparam name="TInterface">实例接口类型</typeparam>
        /// <param name="consParas">构造参数</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        public TInterface Resolver<TEntity, TInterface>(object[] consParas, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null) where TEntity : new()
        {
            return (TInterface)Resolver(typeof(TEntity), consParas, lifeCycle, genArguments);
        }
    }
}
