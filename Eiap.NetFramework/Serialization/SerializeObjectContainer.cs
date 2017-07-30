using System;

namespace Eiap.NetFramework
{
    public class SerializeObjectContainer
    {
        /// <summary>
        /// 容器存储类型
        /// </summary>
        public SerializeObjectContainerType ContainerType { get; set; }

        /// <summary>
        /// 容器存储对象
        /// </summary>
        public object ContainerObject { get; set; }
    }
}
