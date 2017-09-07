
namespace Eiap
{
    /// <summary>
    /// 应用接口
    /// </summary>
    [UnitOfWorkCommit]
    public interface IAppService : IAppServiceUnitOfWork, IRealtimeDependency
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        ILogger Log { get; set; }

        /// <summary>
        /// 对象转换接口
        /// </summary>
        IDTOMapper Mapper { get; set; }

        /// <summary>
        /// 应用程序配置
        /// </summary>
        IConfigurationManager Config { get; set; }
    }
}
