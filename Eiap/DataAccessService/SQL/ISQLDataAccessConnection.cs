
using System.Data;

namespace Eiap
{
    /// <summary>
    /// SQL链接接口
    /// </summary>
    public interface ISQLDataAccessConnection : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 创建数据库链接
        /// </summary>
        /// <returns></returns>
        void Create();

        /// <summary>
        /// 链接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        void DBClose();

        /// <summary>
        /// 打开数据库链接
        /// </summary>
        void DBOpen();

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand();

        /// <summary>
        /// 启用数据库事物
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        IDbTransaction GetTransaction();

        /// <summary>
        /// 是否启动事物
        /// </summary>
        bool IsTransaction { get; }

        /// <summary>
        /// 提交事物
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事物
        /// </summary>
        void Rollback();
    }
}
