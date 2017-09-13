
using System;
using System.Data;
using System.Text;
using System.Web.Http;

namespace Eiap.Test.Controllers
{
    public class ValuesController : ApiController
    {
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

        [HttpPost]
        public string SQLCommandMappingTest(Student student)
        {
            int res = 0;
            using (ISQLCommandMapping<Student, int> test = (ISQLCommandMapping<Student, int>)DependencyManager.Instance.Resolver(typeof(ISQLCommandMapping<Student, int>)))
            {
                for (int i = 0; i < 100; i++)
                {
                    res += test.InsertEntity(student);
                }
            }
            return res.ToString();
        }
    }
}
