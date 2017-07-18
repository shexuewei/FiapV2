
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// SQL命令和实体相互映射
    /// </summary>
    /// <typeparam name="tEntity"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface ISQLCommandMapping<tEntity, TPrimarykey> : IRealtimeDependency,IDynamicProxyDisable
        where tEntity : IEntity<TPrimarykey>
        where TPrimarykey : struct
    {
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int InsertEntity(tEntity t);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int UpdateEntity(tEntity t);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int DeleteEntity(TPrimarykey Id);

        /// <summary>
        /// SQL命令链接接口
        /// </summary>
        ISQLCommandDataAccessConnection SQLDataAccessConnection { set; get; }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="tEntityList"></param>
        /// <returns></returns>
        int BatchInsertEntity(List<tEntity> tEntityList);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="tEntityList"></param>
        /// <returns></returns>
        int BatchUpdateEntity(List<tEntity> tEntityList);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        int BatchDeleteEntity(List<TPrimarykey> idList);
    }
}
