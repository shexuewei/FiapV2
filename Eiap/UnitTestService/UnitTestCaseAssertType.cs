
namespace Eiap
{
    /// <summary>
    /// 单元测试断言类型
    /// </summary>
    public enum UnitTestCaseAssertType
    {
        /// <summary>
        /// 值相等
        /// </summary>
        AssertEquals,

        /// <summary>
        /// 等于False
        /// </summary>
        AssertFalse,

        /// <summary>
        /// 等于True
        /// </summary>
        AssertTrue,

        /// <summary>
        /// 不为Null
        /// </summary>
        AssertNotNull,

        /// <summary>
        /// 为Null
        /// </summary>
        AssertNull,

        /// <summary>
        /// 引用不相等
        /// </summary>
        AssertNotSame,

        /// <summary>
        /// 引用相等
        /// </summary>
        AssertSame,
    }
}
