
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Http;

namespace Eiap.Test.Controllers
{
    public class ValuesController : ApiController
    {
        #region DataAccess
        [HttpGet]
        public string SQLDataCommandTest()
        {
            int res = 0;
            using (ISQLDataCommand test = (ISQLDataCommand)DependencyManager.Instance.Resolver(typeof(ISQLDataCommand)))
            {
                for (int i = 0; i < 100; i++)
                {
                    res += test.ExcuteNonQuery("insert into student values('Sxw'," + DateTime.Now.Millisecond.ToString() + ",'1984-03-02 00:00:00')", CommandType.Text, null);
                }
            }
            return res.ToString();
        }

        [HttpGet]
        public string SQLDataQueryTest()
        {
            StringBuilder res = new StringBuilder();
            using (ISQLDataQuery test = (ISQLDataQuery)DependencyManager.Instance.Resolver(typeof(ISQLDataQuery)))
            {
                using (IDataReader reader = test.ExcuteGetDataReader("select top 10000 * from student", CommandType.Text, null))
                {
                    int rownum = 0;
                    int column = 3;
                    while (reader.Read())
                    {
                        rownum++;
                        res.Append(rownum + ":");
                        for (int i = 0; i < column; i++)
                        {
                            res.Append(reader.GetValue(i).ToString() + "_");
                        }
                        res.Append("\r\n");
                    }
                }
            }
            return res.ToString();
        }

        [HttpGet]
        public string SQLQueryTest()
        {
            StringBuilder res = new StringBuilder();
            using (ISQLQuery test = (ISQLQuery)DependencyManager.Instance.Resolver(typeof(ISQLQuery)))
            {
                DataSet dataset = test.ExcuteGetDateSet("select top 100 * from student", CommandType.Text, null);
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    int rownum = 0;
                    int column = 3;
                    rownum++;
                    res.Append(rownum + ":");
                    for (int i = 0; i < column; i++)
                    {
                        res.Append(dr[i].ToString() + "_");
                    }
                }
            }
            return res.ToString();
        }
        #endregion

        #region DataMapping
        [HttpGet]
        public void InsertEntitySQLCommandMappingTest()
        {

            using (ISQLCommandMapping<School, int> testschool = (ISQLCommandMapping<School, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<School, int>)))
            {
                for (int i = 0; i < 5; i++)
                {
                    School school = testschool.InsertEntity(new School { Name = "School"+i.ToString() });
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

        [HttpPost]
        public string UpdateEntitySQLCommandMappingTest(Student student)
        {
            int res = 0;
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.UpdateEntity(student);
            }
            return res.ToString();
        }

        [HttpGet]
        public string DeleteEntitySQLCommandMappingTest(int id)
        {
            int res = 0;
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.DeleteEntity(id);
            }
            return res.ToString();
        }

        [HttpGet]
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

        [HttpGet]
        public string BatchUpdateEntitySQLCommandMappingTest()
        {
            int res = 0;
            List<Student> list = new List<Student>();
            list.Add(new Student { Id=2, Age = 10 });
            list.Add(new Student {Id =3, Age = 20 });
            list.Add(new Student { Id = 4, Age = 30 });
            list.Add(new Student { Id = 5, Age = 40 });
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                res = test.BatchUpdateEntity(list);
            }
            return res.ToString();
        }

        [HttpGet]
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

        [HttpGet]
        public string SQLDataQueryGetEntityAllListMappingTest()
        {
            List<Student> list = null;
            using (ISQLDataQueryMapping<Student, int> test = (ISQLDataQueryMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLDataQueryMapping<Student, int>)))
            {
                list = test.GetEntityAllList();
            }
            return "";
        }

        [HttpGet]
        public string SQLDataQueryGetEntityListMappingTest()
        {
            List<Class> list = null;
            using (ISQLDataQueryMapping<Class, int> test = (ISQLDataQueryMapping<Class, int>)DependencyManager.Instance.Resolver(typeof(ISQLDataQueryMapping<Class, int>)))
            {
                test.Log = (c) => System.Diagnostics.Debug.Write(c);
                list = test
                    .Where(m => m.Id > 9)
                    .OrderByDesc(m => m.SchoolId)
                    .OrderBy(m=>m.Id)
                    .Top(20)
                    .Skip(10)
                    .Select(m => new Class { Id = m.Id, Name = m.Name, SchoolId = m.SchoolId })
                    .GetEntityList();
            }
            return "";
        }
        #endregion

        #region Repository
        [HttpGet]
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
        #endregion
    }
}
