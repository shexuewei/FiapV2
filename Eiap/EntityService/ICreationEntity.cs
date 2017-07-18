using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 创建实体标识
    /// </summary>
    /// <typeparam name="TUserId"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface ICreationEntity<TUserId, TPrimarykey> : IEntity<TPrimarykey>
        where TUserId : struct
        where TPrimarykey : struct
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        TUserId? CreateUser { get; set; }
    }
}
