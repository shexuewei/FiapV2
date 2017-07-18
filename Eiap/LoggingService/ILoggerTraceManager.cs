
namespace Eiap
{
    /// <summary>
    /// 日志追踪管理接口
    /// </summary>
    public interface ILoggerTraceManager : IContextDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 设置日志追踪
        /// </summary>
        /// <param name="logTrace"></param>
        void SetLogTrace(LoggerTrace logTrace);

        /// <summary>
        /// 获取日志追踪
        /// </summary>
        /// <returns></returns>
        LoggerTrace GetLogTrace();
    }
}
