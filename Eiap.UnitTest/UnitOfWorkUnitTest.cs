using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;
using System.Data;

namespace Eiap.UnitTest
{
    [TestClass]
    public class UnitOfWorkUnitTest
    {
        public UnitOfWorkUnitTest()
        {
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .RegisterInitialize();

        }

        [TestMethod]
        public void UnitOfWorkRepositoryAddTest()
        {
            using (IUnitOfWork uof = DependencyManager.Instance.Resolver<IUnitOfWork>())
            {
                IRepository<School, int> testschool = DependencyManager.Instance.Resolver<IRepository<School, int>>();
                uof.Open();
                for (int i = 0; i < 5; i++)
                {
                    School school = testschool.Add(new School { Name = "School" + i.ToString() });
                    IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>));
                    for (int j = 0; j < 5; j++)
                    {
                        Class cla = testclass.Add(new Class { Name = "Class" + j.ToString(), SchoolId = school.Id });
                    }

                }
                uof.Commit();
            }
        }

        [TestMethod]
        public void UnitOfWorkRepositoryUpdateTest()
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
        public void UnitOfWorkRepositoryDeleteTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                testclass.Delete(6);
            }
        }

        [TestMethod]
        public void UnitOfWorkRepositoryQueryTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                List<Class> classList = testclass.Query().Where(m => m.Id >= 20).GetEntityList();
            }
        }

        [TestMethod]
        public void UnitOfWorkRepositoryQueryResultTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                Class class1 = testclass.Query<Class>("select top 1 * from class", CommandType.Text);
            }
        }

        [TestMethod]
        public void UnitOfWorkRepositoryQueryListResultTest()
        {
            using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
            {
                List<Class> classList = testclass.Query<List<Class>>("select * from class", CommandType.Text);
            }
        }
    }
}
