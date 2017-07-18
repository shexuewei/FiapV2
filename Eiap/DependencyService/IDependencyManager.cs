using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 依赖注入管理接口
    /// </summary>
    public interface IDependencyManager
    {
        /// <summary>
        /// 根据程序集集合，注册依赖接口和依赖实现类
        /// </summary>
        /// <param name="assemblyList"></param>
        void Register(List<Assembly> assemblyList);

        /// <summary>
        /// 依赖注入注册
        /// </summary>
        /// <typeparam name="dependencyInterface">依赖接口类型</typeparam>
        /// <typeparam name="dependencyInterfaceClass">依赖实现类型</typeparam>
        void Register(Type dependencyInterface, Type dependencyInterfaceClass);

        /// <summary>
        /// 获取调用类型实例
        /// </summary>
        /// <param name="tEntity">实例类型</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns>返回实例类型对象</returns>
        object Resolver(Type tEntity, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null, Type[] genArgumentList = null);

        /// <summary>
        /// 获取调用类型实例
        /// </summary>
        /// <param name="tEntity">实例类型</param>
        /// <param name="consParas">构造参数</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        object Resolver(Type tEntity, object[] consParas, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null, Type[] genArgumentList = null);

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        TEntity Resolver<TEntity>(int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null);

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <typeparam name="TInterface">实例接口类型</typeparam>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        TInterface Resolver<TEntity, TInterface>(int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null) where TEntity : new();

        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <param name="consParas">构造参数</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        TEntity Resolver<TEntity>(object[] consParas, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null);
        
        /// <summary>
        /// 根据泛型获取实现类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <typeparam name="TInterface">实例接口类型</typeparam>
        /// <param name="consParas">构造参数</param>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="genArguments">泛型参数</param>
        /// <returns></returns>
        TInterface Resolver<TEntity, TInterface>(object[] consParas, int lifeCycle = ObjectLifeCycle.Realtime, Type[] genArguments = null) where TEntity : new();

        /// <summary>
        /// 释放上下文
        /// </summary>
        /// <param name="tEntity"></param>
        void FreeContext(Type tEntity);

        /// <summary>
        /// 释放单例对象
        /// </summary>
        void FreeSingleton();
    }
}
