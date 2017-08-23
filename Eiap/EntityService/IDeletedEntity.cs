using System;

namespace Eiap
{
    /// <summary>
    /// 删除实体标识
    /// </summary>
    /// <typeparam name="TUserId"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface IDeletedEntity<TUserId, TPrimarykey>
        where TUserId : struct
        where TPrimarykey : struct
    {
        /// <summary>
        /// 删除日期
        /// </summary>
        DateTime? DeleteDate { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>
        TUserId? DeleteUser { get; set; }
    }
}
