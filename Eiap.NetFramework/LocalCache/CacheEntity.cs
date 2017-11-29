using System;

namespace Eiap.NetFramework
{
    public class CacheEntity
    {
        /// <summary>
        /// 缓存值
        /// </summary>
        public string CacheValue { get; set; }

        /// <summary>
        /// 缓存版本（乐观锁）
        /// </summary>
        public int CacheVersion { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? AbsoluteExpiration { get; set; }

        /// <summary>
        /// 用于设置可调过期时间，它表示当离最后访问超过某个时间段后就过期
        /// </summary>
        public int? SlidingExpiration { get; set; }

        /// <summary>
        /// 缓存大小
        /// </summary>
        public int CacheLength { get; set; }

        /// <summary>
        /// 缓存引用次数
        /// </summary>
        public int CacheReferencesCount { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastVisitDateTime { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpiration
        {
            get {
                if (AbsoluteExpiration.HasValue)
                {
                    return DateTime.Now > AbsoluteExpiration.Value;
                }
                return false;
            }
        }

    }
}
