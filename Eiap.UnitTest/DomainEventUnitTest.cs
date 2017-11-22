
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;

namespace Eiap.UnitTest
{
    [TestClass]
    public class DomainEventUnitTest
    {
        public DomainEventUnitTest()
        {
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .Register(DomainEventManager.Instance.Register)
               .RegisterInitialize();
        }

        [TestMethod]
        public void DomainEventTest()
        {
            DomainEventManager.Instance.Trigger<SchoolDomainEventData>(new SchoolDomainEventData { Name = "SSS" });
            DomainEventManager.Instance.Trigger<StudentDomainEventData>(new StudentDomainEventData { Name = "AAA" });
        }
    }
}
