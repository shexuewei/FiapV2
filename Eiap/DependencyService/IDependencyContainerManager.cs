using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 依赖容器管理
    /// </summary>
    public interface IDependencyContainerManager
    {
        /// <summary>
        /// 根据接口和实现类获取依赖注入容器
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        DependencyContainer GetDependencyContainer(Type interfaceType, Type entityType = null);

        /// <summary>
        /// 注册依赖接口和依赖实现类
        /// </summary>
        /// <param name="dependencyInterface"></param>
        /// <param name="dependencyInterfaceClass"></param>
        void RegisterDependencyInterfaceClass(Type dependencyInterface, Type dependencyInterfaceClass, bool isDirectRelation = false);

        /// <summary>
        /// 加载类和依赖实现类
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="interfaceTypeList"></param>
        void RegisterClassAndDependencyInterfaceClass(Type classType, List<Type> interfaceTypeList);
    }
}
