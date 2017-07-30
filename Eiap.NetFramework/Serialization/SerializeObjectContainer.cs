using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap.NetFramework
{
    public class SerializeObjectContainer
    {
        public object CurrentObject { get; set; }
        public SerializeObjectFlag Flag { get; set; }
        public Type CurrentObjectType { get; set; }
    }
}
