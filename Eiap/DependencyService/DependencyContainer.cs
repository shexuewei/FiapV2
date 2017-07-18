using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 依赖容器类
    /// </summary>
    public class DependencyContainer
    {
        /// <summary>
        /// 依赖注入接口类型
        /// </summary>
        public Type DependencyInterface {
            get {
                return Type.GetTypeFromHandle(DependencyInterfaceTypeHandle);
            }
        }

        /// <summary>
        /// 依赖注入接口名称
        /// </summary>
        public string DependencyInterfaceName { get { return DependencyInterface.FullName; } }

        /// <summary>
        /// 依赖注入接口实现类型
        /// </summary>
        public Type DependencyInterfaceClass {
            get
            {
                return Type.GetTypeFromHandle(DependencyInterfaceClassTypeHandle);
            }
        }

        /// <summary>
        /// 依赖注入接口实现类名称
        /// </summary>
        public string DependencyInterfaceClassName { get { return DependencyInterfaceClass.FullName; } }

        /// <summary>
        /// 接口实现类和接口是否是直接关系
        /// </summary>
        public bool IsDirectRelation { get; set; }

        /// <summary>
        /// 依赖注入接口类型句柄
        /// </summary>
        public RuntimeTypeHandle DependencyInterfaceTypeHandle { get; set; }

        /// <summary>
        /// 依赖注入接口实现类句柄
        /// </summary>
        public RuntimeTypeHandle DependencyInterfaceClassTypeHandle { get; set; }

        /// <summary>
        /// 依赖注入适配名称
        /// </summary>
        public string DependencyAdaperName { get; set; }

    }
}
