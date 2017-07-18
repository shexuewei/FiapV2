using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 启用标识
    /// </summary>
    public interface IIsEnable
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnable { get; set; }
    }
}
