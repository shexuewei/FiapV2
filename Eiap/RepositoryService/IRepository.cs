
using System.Data;

namespace Eiap
{
    /// <summary>
    /// 仓储管理接口
    /// </summary>
    /// <typeparam name="tEntity"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface IRepository<tEntity, TPrimarykey> : IRepositoryCommit, IUnitOfWorkCommandConnection, IRealtimeDependency
        where tEntity : IEntity<TPrimarykey>
        where TPrimarykey : struct
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        tEntity Add(tEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        tEntity Update(tEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="primarykey"></param>
        void Delete(TPrimarykey primarykey);

        /// <summary>
        /// 实体查询
        /// </summary>
        /// <returns></returns>
        ISQLDataQueryMapping<tEntity, TPrimarykey> Query();

        /// <summary>
        /// 根据查询语句查询，返回实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        TResult Query<TResult>(string cmdText, CommandType cmdType, IDataParameter[] paramters = null);
    }
}
