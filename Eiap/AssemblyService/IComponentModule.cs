
namespace Eiap
{
    /// <summary>
    /// 组件模块接口
    /// </summary>
    public interface IComponentModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        void AssemblyInitialize();

        /// <summary>
        /// 初始化注册信息
        /// </summary>
        void RegisterInitialize();
    }
}
