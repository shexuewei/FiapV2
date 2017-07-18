
using System.Data;

namespace Eiap
{
    /// <summary>
    /// SQL数据库执行接口
    /// </summary>
    public interface ISQLDataAccess : IRealtimeDependency
    {
        /// <summary>
        /// 获取返回影响行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        int ExcuteNonQuery(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        DataSet ExcuteGetDateSet(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 获取只读数据集
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        IDataReader ExcuteGetDataReader(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 获取数据集第一行第一列对象
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="paramters">SQL参数</param>
        /// <returns></returns>
        object ExecuteScalar(string cmdText, CommandType cmdType, IDataParameter[] paramters);

        /// <summary>
        /// 链接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 创建数据库链接
        /// </summary>
        /// <returns></returns>
        void Create();

        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        void DBClose();

        /// <summary>
        /// 启用数据库事物
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事物
        /// </summary>
        void Commit();
    }
}
