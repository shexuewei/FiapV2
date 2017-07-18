
namespace Eiap
{
    /// <summary>
    /// 工作单元命令链接接口
    /// </summary>
    public interface IUnitOfWorkCommandConnection
    {
        ISQLCommandDataAccessConnection SQLDataAccessConnection { set; }
    }
}
