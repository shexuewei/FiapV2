
using System.Collections.Generic;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 单元测试管理
    /// </summary>
    public interface IUnitTestManager : IRealtimeDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 根据程序集集合，注册用例
        /// </summary>
        /// <param name="assemblyList"></param>
        void Register(List<Assembly> assemblyList);

        /// <summary>
        /// 测试指定命名空间下的方法
        /// </summary>
        /// <param name="unitTestNamespace"></param>
        /// <returns></returns>
        IUnitTestManager Run(string unitTestNamespace);

        /// <summary>
        /// 输出指定命名空间下的测试结果
        /// </summary>
        /// <param name="unitTestNamespace"></param>
        /// <returns></returns>
        IUnitTestManager Print(string unitTestNamespace);
    }
}
