
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
            //Students student1 = new Students { Age = 10, Birthday = DateTime.Now, Height = null, Name = "Student1" };
            //Students student2 = new Students { Age = 20, Birthday = DateTime.Now, Height = 160, Name = "student2" };
            //Students student3 = new Students { Age = 30, Birthday = DateTime.Now, Height = null, Name = "student3" };

            //Classes class1 = new Classes
            //{
            //    ClassName = "Class1",
            //    StudentOne = student2,
            //    StudentList = new List<Students> { student1 }
            //};
            //Classes class2 = new Classes
            //{
            //    ClassName = "class2",
            //    StudentOne = student1,
            //    StudentList = new List<Students> { student3, student2, student1 }
            //};
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("123", "321");
            //dict.Add("456", "654");
            //Schools school1 = new Schools
            //{
            //    SchoolName = "school1",
            //    SchoolAge = 10,
            //    IsPubSchool = false,
            //    Amt = 1999.12345m,
            //    ClassOne = class1,
            //    ClassList = new List<Classes> { class1, class2 },
            //    IsPriSchool = true,
            //    Building = new string[] { "1", "2", "3" },
            //    //ClassList2 = new ArrayList { class1, 2 },
            //    Dict = dict,
            //    SchoolFirstStu = student1
            //};
            //List<Schools> schoolList = new List<Schools>();
            //schoolList.Add(school1);

            //#region 序列化
            //int count = 10;
            //int num = 10000;
            //double sum4 = 0;
            //ISerializationManager serliz = DependencyManager.Instance.Resolver<ISerializationManager>();

            //Stopwatch stopwatch = new Stopwatch();
            //for (int m = 0; m < count; m++)
            //{
            //    stopwatch.Restart();
            //    for (int i = 0; i < num; i++)
            //    {
            //        var xx = JsonConvert.SerializeObject(school1);
            //    }
            //    stopwatch.Stop();
            //    Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            //    sum4 += stopwatch.Elapsed.TotalMilliseconds;
            //}
            //Console.WriteLine("Avg:" + sum4 / count);
            //Console.WriteLine("-----------------------------");
            //double sum3 = 0;
            //for (int m = 0; m < count; m++)
            //{
            //    stopwatch.Restart();
            //    for (int i = 0; i < num; i++)
            //    {
            //        var xx = serliz.SerializeObject(school1);
            //    }
            //    stopwatch.Stop();
            //    Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            //    sum3 += stopwatch.Elapsed.TotalMilliseconds;
            //}
            //Console.WriteLine("Avg:" + sum3 / count);
            //#endregion

            //#region 反序列化
            //var testobject = serliz.SerializeObject(schoolList);

            //double sum1 = 0;
            //for (int m = 0; m < count; m++)
            //{
            //    Stopwatch stopwatch1 = new Stopwatch();
            //    stopwatch1.Start();
            //    for (int i = 0; i < num; i++)
            //    {
            //        var xx = JsonConvert.DeserializeObject<List<Schools>>(testobject);
            //    }
            //    stopwatch1.Stop();
            //    Console.WriteLine("Newtonsoft:" + stopwatch1.Elapsed.TotalMilliseconds);
            //    sum1 += stopwatch1.Elapsed.TotalMilliseconds;
            //}
            //Console.WriteLine("Newtonsoft Avg:" + sum1 / count);

            //double sum2 = 0;
            //for (int m = 0; m < count; m++)
            //{
            //    Stopwatch stopwatch2 = new Stopwatch();
            //    stopwatch2.Start();
            //    for (int i = 0; i < num; i++)
            //    {
            //        var xx = serliz.DeserializeObject<List<Schools>>(testobject);
            //    }
            //    stopwatch2.Stop();
            //    Console.WriteLine("SXW:" + stopwatch2.Elapsed.TotalMilliseconds);
            //    sum2 += stopwatch2.Elapsed.TotalMilliseconds;
            //}
            //Console.WriteLine("SXW Avg:" + sum2 / count);


            //Console.WriteLine("ArraySymbol_Begin_Time:" + ProcessTime.ArraySymbol_Begin_Time);
            //Console.WriteLine("ArraySymbol_End_Time:" + ProcessTime.ArraySymbol_End_Time);
            //Console.WriteLine("ObjectSymbol_Begin_Time:" + ProcessTime.ObjectSymbol_Begin_Time);
            //Console.WriteLine("ObjectSymbol_End_Time:" + ProcessTime.ObjectSymbol_End_Time);
            //Console.WriteLine("PropertySymbol_Time:" + ProcessTime.PropertySymbol_Time);
            //Console.WriteLine("SeparateSymbol_Time:" + ProcessTime.SeparateSymbol_Time);


            //#endregion


            int count = 10000000;
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            for (int i = 0; i < count; i++)
            {
                Student s1 = (Student)Activator.CreateInstance(typeof(Student));
                s1.A = 5;
            }
            stopwatch1.Stop();
            Console.WriteLine("直接调用：" + stopwatch1.Elapsed.TotalMilliseconds);

            Stopwatch stopwatch4 = new Stopwatch();
            stopwatch4.Start();
            for (int i = 0; i < count; i++)
            {
                Student s2 = (Student)Activator.CreateInstance(typeof(Student));
                s2.GetType().GetProperty("A").SetValue(s2, 1);
            }
            stopwatch4.Stop();
            Console.WriteLine("反射调用：" + stopwatch4.Elapsed.TotalMilliseconds);

            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            MethodInfo methodinfo1 = typeof(Student).GetProperty("A").GetSetMethod();
            for (int i = 0; i < count; i++)
            {
                Student s3 = (Student)Activator.CreateInstance(typeof(Student));
                methodinfo1.Invoke(s3, new object[] { 1 });
            }
            stopwatch3.Stop();
            Console.WriteLine("反射 方法缓存：" + stopwatch3.Elapsed.TotalMilliseconds);

            MethodInfo methodinfo2 = typeof(Student).GetProperty("A").GetSetMethod();
            IMethodManager methodamanger = DependencyManager.Instance.Resolver<IMethodManager>();
            //Func<object, object[], object> xx = DynamicMethodFactory.GetExecuteDelegate(methodinfo2);
            double sum2 = 0;
            for (int i = 0; i < count; i++)
            {
                Stopwatch stopwatch2 = new Stopwatch();
                stopwatch2.Start();
                Student s3 = (Student)Activator.CreateInstance(typeof(Student));
                object x = methodamanger.MethodInvoke(s3, new object[] { 1 }, methodinfo2);
                //object x = xx(s3, new object[] { 2 });
                //methodinfo2.Invoke(s3, new object[] { 1, 2 });
                stopwatch2.Stop();
                sum2 += stopwatch2.Elapsed.TotalMilliseconds;
            }
            Console.WriteLine("表达式树：" + sum2);
            //Console.WriteLine("GetAllTime1：" + methodamanger.GetAllTime1());
            //Console.WriteLine("GetAllTime2：" + methodamanger.GetAllTime2());
            //Console.WriteLine("GetAllTime3：" + methodamanger.GetAllTime3());
            //Console.WriteLine("GetAllTime4：" + methodamanger.GetAllTime4());

            Console.ReadLine();
        }
    }

    public class Schools
    {
        public string SchoolName { get; set; }
        public int SchoolAge { get; set; }
        public bool? IsPubSchool { get; set; }
        public bool? IsPriSchool { get; set; }
        public string[] Building { get; set; }
        public decimal Amt { get; set; }
        public Classes ClassOne { get; set; }
        public IEnumerable<Classes> ClassList { get; set; }
        public ArrayList ClassList2 { get; set; }
        public Dictionary<string, string> Dict { get; set; }
        public Students SchoolFirstStu { get; set; }
    }

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
