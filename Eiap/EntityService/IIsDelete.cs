using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 删除标识
    /// </summary>
    public interface IIsDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDelete { get; set; }
    }
}
