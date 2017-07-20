
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// Eiap框架组件
    /// </summary>
    public class EiapModule : IComponentModule
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

        }
    }
}
