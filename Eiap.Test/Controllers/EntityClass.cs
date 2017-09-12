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
    }
}