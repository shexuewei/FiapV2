using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eiap.Test.Controllers
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

        public List<Student> StudentList { get; set; }

        public int SchoolId { get; set; }
    }

    public class School : DefaultEntity<int>
    {
        public string Name { get; set; }

        public List<Class> ClassList { get; set; }
    }

    public class SchoolGuid : DefaultEntity<Guid>
    {
        public string Name { get; set; }

    }

}