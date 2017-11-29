
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;
using System.Collections.Generic;
using System;

namespace Eiap.UnitTest
{
    [TestClass]
    public class LocalCacheUnitTest
    {
        public LocalCacheUnitTest()
        {
            AssemblyManager.Instance
               .AssemblyInitialize(typeof(EiapModule), typeof(EiapNetFrameworkModule), typeof(EiapUnitTestModule))
               .Register(DependencyManager.Instance.Register)
               .Register(InterceptorManager.Instance.Register)
               .Register(DomainEventManager.Instance.Register)
               .RegisterInitialize();
        }

        [TestMethod]
        public void LocalCacheTest()
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

            ICacheManager tmp =  DependencyManager.Instance.Resolver<ICacheManager>();

            tmp.SetCache("11111", schoolList);
            object xx = tmp.GetCache("11111");
        }

        [TestMethod]
        public void LocalCacheAbsoluteExpirationTest()
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

            ICacheManager tmp = DependencyManager.Instance.Resolver<ICacheManager>();

            tmp.SetCache("11111", schoolList, 5);
            object xx = tmp.GetCache("11111");
        }

        [TestMethod]
        public void LocalCacheSlidingExpirationTest()
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

            ICacheManager tmp = DependencyManager.Instance.Resolver<ICacheManager>();

            tmp.SetCache("11111", schoolList, 5, 5);
            object xx = tmp.GetCache("11111");
        }
    }
}
