using System;

namespace Eiap
{
    /// <summary>
    /// 更新实体标识
    /// </summary>
    /// <typeparam name="TUserId"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface IModifyEntity<TUserId, TPrimarykey>
        where TUserId : struct
        where TPrimarykey : struct
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        TUserId? ModifyUser { get; set; }
    }
}
