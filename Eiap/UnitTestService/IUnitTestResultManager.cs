
namespace Eiap
{
    /// <summary>
    /// 单元测试结果管理接口
    /// </summary>
    public interface IUnitTestResultManager : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 添加单元测试结果
        /// </summary>
        /// <param name="container"></param>
        void AddUnitTestResult(UnitTestResultContainer container);

        /// <summary>
        /// 根据命名空间输出单元测试结果
        /// </summary>
        /// <param name="unitTestNamespace"></param>
        void PrintUnitTestResultByNamespace(string unitTestNamespace);
    }
}
