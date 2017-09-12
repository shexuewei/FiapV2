
using System.Data;
using System.Linq.Expressions;

namespace Eiap
{
    /// <summary>
    /// SQL数据查询和实体映射扩展接口
    /// </summary>
    /// <typeparam name="tEntity"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface ISQLDataMappingExtension : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 根据表达式获取操作符
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GetOperationValue(Expression expr);

        /// <summary>
        /// 根据实体获取参数
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        IDataParameter[] GetDataParameter(IEntity entity, int index = 0);
    }
}
