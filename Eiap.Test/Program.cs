
using Eiap.NetFramework;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Eiap.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyManager.Instance.AssemblyInitialize().Register(DependencyManager.Instance.Register);
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
            ISchool school1 = new Schools2
            {
                SchoolName = "school1",
                SchoolAge = 10,
                IsPubSchool = false,
                Amt = 1999.12345m,
                ClassOne = class1,
                ClassList = new List<Classes> { class1, class2 },
                IsPriSchool = true,
                Building = new string[] { "1", "2", "3" },
                Dict = dict,
                SchoolFirstStu = student1
            };
            List<ISchool> schoolList = new List<ISchool>();
            schoolList.Add(school1);

            int count = 10;
            int num = 10000;
            ISerializationManager serliz = DependencyManager.Instance.Resolver<ISerializationManager>();
            //serliz.GetOrAddSerializeObject(school1.GetType());
            #region 序列化
            //double sum4 = 0;
            ////
            ////var yy = JsonConvert.SerializeObject(school1);
            ////var xx = serliz.SerializeObject(school1);
            //Stopwatch stopwatch1 = new Stopwatch();
            //for (int m = 0; m < count; m++)
            //{
            //    stopwatch1.Start();
            //    for (int i = 0; i < num; i++)
            //    {
            //        var xx = JsonConvert.SerializeObject(school1);
            //    }
            //    stopwatch1.Stop();
            //    sum4 += stopwatch1.Elapsed.TotalMilliseconds;
            //}
            //Console.WriteLine("Avg:" + sum4 / count);
            //Console.WriteLine("-----------------------------");

            //double sum3 = 0;
            //Stopwatch stopwatch2 = new Stopwatch();
            //for (int m = 0; m < count; m++)
            //{
            //    stopwatch2.Start();
            //    for (int i = 0; i < num; i++)
            //    {
            //        var xx = serliz.SerializeObject(school1);
            //    }
            //    stopwatch2.Stop();
            //    sum3 += stopwatch2.Elapsed.TotalMilliseconds;
            //}
            //Console.WriteLine("Avg:" + sum3 / count);
            #endregion

            #region 反序列化
            var testobject = JsonConvert.SerializeObject(schoolList);

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
                Console.WriteLine("Newtonsoft:" + stopwatch1.Elapsed.TotalMilliseconds);
                sum1 += stopwatch1.Elapsed.TotalMilliseconds;
            }
            Console.WriteLine("Newtonsoft Avg:" + sum1 / count);

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
                Console.WriteLine("SXW:" + stopwatch2.Elapsed.TotalMilliseconds);
                sum2 += stopwatch2.Elapsed.TotalMilliseconds;
            }
            Console.WriteLine("SXW Avg:" + sum2 / count);

            #endregion

            Console.ReadLine();
        }
    }

    public interface ISchool
    { }

    public class Schools: ISchool
    {
        public string SchoolName { get; set; }
        public int SchoolAge { get; set; }
        public bool? IsPubSchool { get; set; }
        public bool? IsPriSchool { get; set; }
        public string[] Building { get; set; }
        public decimal Amt { get; set; }
        public Classes ClassOne { get; set; }
        public IEnumerable<Classes> ClassList { get; set; }
        public Dictionary<string, string> Dict { get; set; }
        public Students SchoolFirstStu { get; set; }
    }

    public class Schools2 : Schools
    { }

    public class Classes
    {
        public string ClassName { get; set; }
        public Students StudentOne { get; set; }
        public IList<Students> StudentList { get; set; }
    }

    public class Students
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public int? Height { get; set; }
    }

    public class Student
    {
        public int A { get; set; }
    }

    public class TestModule : IComponentModule
    {
        public void AssemblyInitialize()
        {
            AssemblyManager.Instance.RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 注册
        /// </summary>
        public void RegisterInitialize()
        {
            
        }
    }
}
