
using System;

namespace Eiap
{
    /// <summary>
    /// 创建审计实体基类
    /// </summary>
    public class CreationAuditedEntity<TUserId, TPrimarykey> : IEntity<TPrimarykey>, 
        ICreationEntity<TUserId, TPrimarykey>
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

    }
}
