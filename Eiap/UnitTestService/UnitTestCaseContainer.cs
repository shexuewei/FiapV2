using System;
using System.Collections.Generic;

namespace Eiap
{
    /// <summary>
    /// 单元测试用例容器
    /// </summary>
    public class UnitTestCaseContainer
    {
        /// <summary>
        /// 单元测试模块命名空间
        /// </summary>
        public string UnitTestNamespace { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public List<object> MethodParas { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public object MethodReturn { get; set; }

        /// <summary>
        /// 断言类型
        /// </summary>
        public UnitTestCaseAssertType CaseAssertType { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 单元测试接口类型
        /// </summary>
        public Type UnitTestClassType
        {
            get
            {
                return Type.GetTypeFromHandle(UnitTestClassTypeHandle);
            }
        }

        /// <summary>
        /// 单元测试接口类型句柄
        /// </summary>
        public RuntimeTypeHandle UnitTestClassTypeHandle { get; set; }
    }
}
