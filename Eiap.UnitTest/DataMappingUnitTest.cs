using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eiap.UnitTest
{
    [TestClass]
    public class DataMappingUnitTest
    {
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
        public string UpdateEntitySQLCommandMappingTest()
        {
            int res = 0;
            Student student = new Student { Age = 10, Birthday = DateTime.Now, Id = 20, Name = "XXXX" };
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.UpdateEntity(student);
            }
            return res.ToString();
        }

        [TestMethod]
        public string DeleteEntitySQLCommandMappingTest()
        {
            int res = 0;
            int id = 30;
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.DeleteEntity(id);
            }
            return res.ToString();
        }

        [TestMethod]
        public void BatchInsertEntitySQLCommandMappingTest()
        {
            School school = new School { Name = "School1" };
            using (ISQLCommandMapping<School, int> test = (ISQLCommandMapping<School, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<School, int>)))
            {
                for (int i = 0; i < 100; i++)
                {
                    test.InsertEntity(school);
                }
            }
        }

        [TestMethod]
        public string BatchUpdateEntitySQLCommandMappingTest()
        {
            int res = 0;
            List<Student> list = new List<Student>();
            list.Add(new Student { Id = 2, Age = 10 });
            list.Add(new Student { Id = 3, Age = 20 });
            list.Add(new Student { Id = 4, Age = 30 });
            list.Add(new Student { Id = 5, Age = 40 });
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.BatchUpdateEntity(list);
            }
            return res.ToString();
        }

        [TestMethod]
        public string BatchDeleteEntitySQLCommandMappingTest()
        {
            int res = 0;
            List<int> list = new List<int> { 2, 3, 4, 5 };
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.BatchDeleteEntity(list);
            }
            return res.ToString();
        }

        [TestMethod]
        public string SQLDataQueryGetEntityAllListMappingTest()
        {
            List<Student> list = null;
            using (ISQLDataQueryMapping<Student, int> test = (ISQLDataQueryMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLDataQueryMapping<Student, int>)))
            {
                list = test.GetEntityAllList();
            }
            return "";
        }

        [TestMethod]
        public string SQLDataQueryGetEntityListMappingTest()
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
            return "";
        }
    }
}
