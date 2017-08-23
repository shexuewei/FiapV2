
using System;

namespace Eiap
{
    /// <summary>
    /// 全部审计实体基类
    /// </summary>
    public class FullAuditedEntity<TUserId, TPrimarykey> : IEntity<TPrimarykey>, 
        ICreationEntity<TUserId, TPrimarykey>, 
        IModifyEntity<TUserId, TPrimarykey>, 
        IDeletedEntity<TUserId, TPrimarykey>, 
        ISoftDeleted
        where TUserId : struct
        where TPrimarykey : struct
    {
        /// <summary>
        /// 主键，默认为ID
        /// </summary>
        [Property("Id", IsPrimaryKey = true)]
        public TPrimarykey Id { get; set; }
        /// <summary>
        /// 创建日期时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public TUserId? CreateUser { get; set; }

        /// <summary>
        /// 删除日期时间
        /// </summary>
        public DateTime? DeleteDate { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>
        public TUserId? DeleteUser { get; set; }

        /// <summary>
        /// 是否软删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 修改日期时间
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public TUserId? ModifyUser { get; set; }
    }
}
