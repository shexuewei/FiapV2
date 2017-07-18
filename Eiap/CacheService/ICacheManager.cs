
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : ISingletonDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cachecontent"></param>
        /// <param name="CacheExpiredTimeType"></param>
        /// <param name="timer"></param>
        void SetCache(string key, object cacheContent, int? absoluteExpiration = null, int? slidingExpiration = null);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetCache(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        bool RemoveCache(string key);

        /// <summary>
        /// 获取缓存个数
        /// </summary>
        /// <returns></returns>
        int GetCacheCount();

        /// <summary>
        /// 获取缓存所有键
        /// </summary>
        /// <returns></returns>
        List<string> GetAllCacheKey();

    }
}
