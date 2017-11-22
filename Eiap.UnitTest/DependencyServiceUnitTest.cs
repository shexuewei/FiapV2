
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;

namespace Eiap.UnitTest
{
    [TestClass]
    public class DependencyServiceUnitTest
    {
        public DependencyServiceUnitTest()
        {
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .Register(DomainEventManager.Instance.Register)
               .RegisterInitialize();
        }

        [TestMethod]
        public void DependencyServiceTest()
        {
            IDomainEventHandler<StudentDomainEventData> tmp =  DependencyManager.Instance.Resolver<IDomainEventHandler<StudentDomainEventData>>();
            tmp.ProcessEvent(new StudentDomainEventData { Name = "123" });

        }
    }
}
