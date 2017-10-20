using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;

namespace Eiap.UnitTest
{
    [TestClass]
    public class DataMappingUnitTest
    {
        public DataMappingUnitTest()
        {
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .RegisterInitialize();

        }

        [TestMethod]
        public void InsertEntitySQLCommandMappingTest()
        {

            using (ISQLCommandMapping<School, int> testschool = (ISQLCommandMapping<School, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<School, int>)))
            {
                for (int i = 0; i < 5; i++)
                {
                    School school = testschool.InsertEntity(new School { Name = "School" + i.ToString() });
                    using (ISQLCommandMapping<Class, int> testclass = (ISQLCommandMapping<Class, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Class, int>)))
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Class cla = testclass.InsertEntity(new Class { Name = "Class" + j.ToString(), SchoolId = school.Id });
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void UpdateEntitySQLCommandMappingTest()
        {
            Student student = new Student { Age = 10, Birthday = DateTime.Now, Id = 20, Name = "XXXX" };
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                test.UpdateEntity(student);
            }
        }

        [TestMethod]
        public void DeleteEntitySQLCommandMappingTest()
        {
            int id = 30;
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                test.DeleteEntity(id);
            }
        }

        [TestMethod]
        public void BatchInsertEntitySQLCommandMappingTest()
        {
            List<School> schoollist = new List<School>() {
                new School { Name = "School1"+ DateTime.Now.Millisecond.ToString() },
                new School { Name = "School1"+ DateTime.Now.Millisecond.ToString() },
                new School { Name = "School1"+ DateTime.Now.Millisecond.ToString() },
                new School { Name = "School1"+ DateTime.Now.Millisecond.ToString() },
                new School { Name = "School1"+ DateTime.Now.Millisecond.ToString() }
            };
            using (ISQLCommandMapping<School, int> test = (ISQLCommandMapping<School, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<School, int>)))
            {
                test.BatchInsertEntity(schoollist);
            }
        }

        [TestMethod]
        public void BatchUpdateEntitySQLCommandMappingTest()
        {
            List<Student> list = new List<Student>();
            list.Add(new Student { Id = 2, Age = 10 });
            list.Add(new Student { Id = 3, Age = 20 });
            list.Add(new Student { Id = 4, Age = 30 });
            list.Add(new Student { Id = 5, Age = 40 });
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                test.BatchUpdateEntity(list);
            }
        }

        [TestMethod]
        public void BatchDeleteEntitySQLCommandMappingTest()
        {
            List<int> list = new List<int> { 2, 3, 4, 5 };
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                test.BatchDeleteEntity(list);
            }
        }

        [TestMethod]
        public void SQLDataQueryGetEntityAllListMappingTest()
        {
            List<Student> list = null;
            using (ISQLDataQueryMapping<Student, int> test = (ISQLDataQueryMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLDataQueryMapping<Student, int>)))
            {
                list = test.GetEntityAllList();
            }
        }

        [TestMethod]
        public void SQLDataQueryGetEntityListMappingTest()
        {
            List<Class> list = null;
            using (ISQLDataQueryMapping<Class, int> test = (ISQLDataQueryMapping<Class, int>)DependencyManager.Instance.Resolver(typeof(ISQLDataQueryMapping<Class, int>)))
            {
                test.Log = (c) => System.Diagnostics.Debug.Write(c);
                list = test
                    .Where(m => m.Id > 9)
                    .OrderByDesc(m => m.SchoolId)
                    .OrderBy(m => m.Id)
                    .Top(20)
                    .Skip(10)
                    .Select(m => new Class { Id = m.Id, Name = m.Name, SchoolId = m.SchoolId })
                    .GetEntityList();
            }
        }
    }
}
