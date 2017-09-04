
using System.Reflection;

namespace Eiap.NetFramework
{
    /// <summary>
    /// Eiap使用.netFramework实现组件
    /// </summary>
    public class EiapNetFrameworkModule : IComponentModule
    {
        public void AssemblyInitialize()
        {
            AssemblyManager.Instance.RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 注册
        /// </summary>
        public void RegisterInitialize()
        {
            DependencyManager.Instance.Resolver<IConfigurationManager>().Register();
        }
    }
}
