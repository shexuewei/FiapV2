
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// 单元测试用例管理
    /// </summary>
    public interface IUnitTestCaseManager : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 注册用例
        /// </summary>
        /// <param name="container"></param>
        void RegisterUnitTestCase(UnitTestCaseContainer container);

        /// <summary>
        /// 根据命名空间获取用例集合
        /// </summary>
        /// <param name="unitTestNamespace"></param>
        /// <returns></returns>
        List<UnitTestCaseContainer> GetUnitTestCaseByNamespace(string unitTestNamespace);
    }
}
