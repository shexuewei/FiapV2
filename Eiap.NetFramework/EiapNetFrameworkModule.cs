
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
            ConfigurationManager.Instance
                .SetEnvironment("Test").SetKey("LogPathFormat").SetValue(@"c:\loggers\{AppCode}\{LogLevel}\{YYYY}\{MM}\{DD}\{HH}.log").Register()
                .SetEnvironment("PRD").SetKey("LogPathFormat").SetValue(@"c:\loggers\{AppCode}\{LogLevel}\{YYYY}\{MM}\{DD}\{HH}.log").Register()
                .SetEnvironment("Test").SetKey("LogSize").SetValue(@"204800000").Register()
                .SetEnvironment("PRD").SetKey("LogSize").SetValue(@"204800000").Register();
        }
    }
}
