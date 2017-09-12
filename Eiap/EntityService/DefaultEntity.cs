
using System;

namespace Eiap
{
    /// <summary>
    /// 创建审计实体基类
    /// </summary>
    public class DefaultEntity<TPrimarykey> : IEntity<TPrimarykey>
        where TPrimarykey : struct
    {
        /// <summary>
        /// 主键，默认为ID
        /// </summary>
        [Property("Id", IsPrimaryKey = true)]
        public TPrimarykey Id { get; set; }

    }
}
