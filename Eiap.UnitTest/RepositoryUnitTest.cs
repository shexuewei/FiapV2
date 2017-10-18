using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;
using System.Data;

namespace Eiap.UnitTest
{
    [TestClass]
    public class RepositoryUnitTest
    {
        public RepositoryUnitTest()
        {
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .RegisterInitialize();

        }

        [TestMethod]
        public void RepositoryAddTest()
        {
            using (IRepository<School, int> testschool = (IRepository<School, int>)DependencyManager.Instance.Resolver(typeof(IRepository<School, int>)))
            {
                for (int i = 0; i < 5; i++)
                {
                    School school = testschool.Add(new School { Name = "School" + i.ToString() });
                    using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Class cla = testclass.Add(new Class { Name = "Class" + j.ToString(), SchoolId = school.Id });
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void RepositoryUpdateTest()
        {
            using (IRepository<School, int> testschool = (IRepository<School, int>)DependencyManager.Instance.Resolver(typeof(IRepository<School, int>)))
            {
                List<School> schoollist = testschool.Query().Where(m => m.Id < 50).Top(100).GetEntityList();
                foreach (School schoolentity in schoollist)
                {
                    schoolentity.Name = schoolentity.Name + new Random().Next(1, 100).ToString();
                    testschool.Update(schoolentity);
                }
            }
        }

        [TestMethod]
        public void RepositoryDeleteTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                testclass.Delete(6);
            }
        }

        [TestMethod]
        public void RepositoryQueryTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                List<Class> classList = testclass.Query().Where(m => m.Id >= 20).GetEntityList();
            }
        }

        [TestMethod]
        public void RepositoryQueryResultTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                Class class1 = testclass.Query<Class>("select top 1 * from class", CommandType.Text);
            }
        }
    }
}
