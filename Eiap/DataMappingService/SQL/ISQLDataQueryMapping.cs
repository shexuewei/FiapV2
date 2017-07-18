
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Eiap
{
    /// <summary>
    /// SQL数据查询和实体映射接口
    /// </summary>
    /// <typeparam name="tEntity"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface ISQLDataQueryMapping<tEntity, TPrimarykey> : IRealtimeDependency, IDynamicProxyDisable
        where tEntity : IEntity<TPrimarykey>
        where TPrimarykey : struct
    {
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        List<tEntity> GetEntityList();

        /// <summary>
        /// 获取实体所有集合
        /// </summary>
        /// <returns></returns>
        List<tEntity> GetEntityAllList();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        tEntity GetEntity(TPrimarykey Id);

        /// <summary>
        /// 获取实体条件
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> Where(Expression<Func<tEntity, bool>> lambda);

        /// <summary>
        /// 获取实体排序
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> OrderBy(Expression<Func<tEntity, object>> lambda);

        /// <summary>
        /// 获取实体倒序
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> OrderByDesc(Expression<Func<tEntity, object>> lambda);

        /// <summary>
        /// 获取实体显示字段
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> Select(Expression<Action<tEntity>> lambda);

        /// <summary>
        /// 获取实体前top条
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> Top(int top);

        /// <summary>
        /// 跳过skip条获取实体
        /// </summary>
        /// <param name="skip"></param>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> Skip(int skip);

        /// <summary>
        /// Nolock模式获取实体
        /// </summary>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> Nolock();
    }
}
