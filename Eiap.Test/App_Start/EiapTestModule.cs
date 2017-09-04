
using System.Reflection;

namespace Eiap.Test
{
    public class EiapTestModule : IComponentModule
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
