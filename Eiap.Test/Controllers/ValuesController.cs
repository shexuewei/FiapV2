
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
            ISQLDataCommand test = (ISQLDataCommand)DependencyManager.Instance.Resolver(typeof(ISQLDataCommand));
            int res = test.ExcuteNonQuery("insert into student values('Sxw'," + DateTime.Now.Millisecond.ToString() + ",'1984-03-02 00:00:00')", CommandType.Text, null);
            return res.ToString();
        }

        [HttpGet]
        public string SQLDataQueryTest()
        {
            ISQLDataQuery test = (ISQLDataQuery)DependencyManager.Instance.Resolver(typeof(ISQLDataQuery));
            StringBuilder res = new StringBuilder();
            using (IDataReader reader = test.ExcuteGetDataReader("select top 100 * from student", CommandType.Text, null))
            {
                int rownum = 0;
                int column = 3;
                while (reader.Read())
                {
                    rownum++;
                    res.Append(rownum + ":");
                    for (int i = 0; i < column; i++)
                    {
                        res.Append(reader.GetValue(i).ToString()+"_");
                    }
                    res.Append("\r\n");
                }
            }
            return res.ToString();
        }
    }
}
