using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eiap.UnitTest
{
    public class Student : DefaultEntity<int>
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime? Birthday { get; set; }

        public int ClassId { get; set; }
    }

    public class Class : DefaultEntity<int>
    {
        public string Name { get; set; }

        public int SchoolId { get; set; }
    }

    public class School : DefaultEntity<int>
    {
        public string Name { get; set; }

    }

    public class SchoolGuid : DefaultEntity<Guid>
    {
        public string Name { get; set; }

    }

    #region 用于JSON序列化测试
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
    #endregion

    public class SchoolDomainEventData : IDomainEventData
    {
        public string Name { get; set; }
    }

    public interface ISchoolDomainEventHandler : IDomainEventHandler<SchoolDomainEventData>
    { }

    public class SchoolDomainEventHandler : ISchoolDomainEventHandler
    {
        public ILogger Log { get; set; }

        public void ProcessEvent(SchoolDomainEventData eventData)
        {
            Debug.WriteLine(eventData.Name + "1111");
        }
    }

}