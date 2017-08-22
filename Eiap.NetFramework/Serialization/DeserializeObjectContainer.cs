
namespace Eiap.NetFramework
{
    /// <summary>
    /// 反序列化对象容器
    /// </summary>
    public class DeserializeObjectContainer
    {
        /// <summary>
        /// 容器存储类型
        /// </summary>
        public DeserializeObjectContainerType ContainerType { get; set; }

        /// <summary>
        /// 容器存储对象
        /// </summary>
        public object ContainerObject { get; set; }

        /// <summary>
        /// 容器存储对象类型名称
        /// </summary>
        public string ContainerObjectTypeName { get; set; }
    }
}
