using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 方法容器
    /// </summary>
    public class MethodContainer
    {
        /// <summary>
        /// 通用方法类型
        /// </summary>
        public Func<object, object[], object> Method { get; set; }
    }
}
