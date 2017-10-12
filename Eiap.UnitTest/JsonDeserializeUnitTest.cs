using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Diagnostics;
using Eiap.NetFramework;

namespace Eiap.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonDeserializeUnitTest jsonDeserializeUnitTest = new JsonDeserializeUnitTest();
            jsonDeserializeUnitTest.JsonDeserializeTest();
        }
    }

    //[TestClass]
    public class JsonDeserializeUnitTest
    {
        public JsonDeserializeUnitTest()
        {
            EiapNetFrameworkModule xx = new EiapNetFrameworkModule();
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .RegisterInitialize();
        }

        //[TestMethod]
        public void JsonDeserializeTest()
        {
            #region 构造数据
            Students student1 = new Students { Age = 10, Birthday = DateTime.Now, Height = null, Name = "Student1" };
            Students student2 = new Students { Age = 20, Birthday = DateTime.Now, Height = 160, Name = "student2" };
            Students student3 = new Students { Age = 30, Birthday = DateTime.Now, Height = null, Name = "student3" };
            Classes class1 = new Classes
            {
                ClassName = "Class1",
                StudentOne = student2,
                StudentList = new List<Students> { student1 }
            };
            Classes class2 = new Classes
            {
                ClassName = "class2",
                StudentOne = student1,
                StudentList = new List<Students> { student3, student2, student1 }
            };
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("123", "321");
            dict.Add("456", "654");
            Schools school1 = new Schools
            {
                SchoolName = "school1",
                SchoolAge = 10,
                IsPubSchool = false,
                Amt = 1999.12345m,
                ClassOne = class1,
                ClassList = new List<Classes> { class1, class2 },
                IsPriSchool = true,
                Building = new string[] { "1", "2", "3" },
                //ClassList2 = new ArrayList { class1, 2 },
                Dict = dict,
                SchoolFirstStu = student1
            };
            List<Schools> schoolList = new List<Schools>();
            schoolList.Add(school1);
            #endregion

            StringBuilder sb = new StringBuilder();
            ISerializationManager serliz = DependencyManager.Instance.Resolver<ISerializationManager>();
            var testobject = serliz.SerializeObject(schoolList);
            int count = 10;
            int num = 10000;
            double sum1 = 0;
            for (int m = 0; m < count; m++)
            {
                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Start();
                for (int i = 0; i < num; i++)
                {
                    var xx = JsonConvert.DeserializeObject<List<Schools>>(testobject);
                }
                stopwatch1.Stop();
                sb.Append("Newtonsoft:" + stopwatch1.Elapsed.TotalMilliseconds + "\r\n");
                sum1 += stopwatch1.Elapsed.TotalMilliseconds;
            }
            sb.Append("Newtonsoft Avg:" + sum1 / count + "\r\n");

            double sum2 = 0;
            for (int m = 0; m < count; m++)
            {
                Stopwatch stopwatch2 = new Stopwatch();
                stopwatch2.Start();
                for (int i = 0; i < num; i++)
                {
                    var xx = serliz.DeserializeObject<List<Schools>>(testobject);
                }
                stopwatch2.Stop();
                sb.Append("SXW:" + stopwatch2.Elapsed.TotalMilliseconds + "\r\n");
                sum2 += stopwatch2.Elapsed.TotalMilliseconds;
            }
            sb.Append("SXW Avg:" + sum2 / count + "\r\n");
            Debug.WriteLine(sb.ToString());
        }
    }
}
