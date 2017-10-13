using System;
using System.Collections.Generic;

namespace Eiap.NetFramework
{
    /// <summary>
    /// Json反序列化事件参数
    /// </summary>
    public class JsonDeserializeEventArgs : EventArgs
    {
        /// <summary>
        /// JSON字符串栈
        /// </summary>
        public Stack<char> JsonStringStack { get; set; }

        /// <summary>
        /// 反序列化对象容器栈
        /// </summary>
        public Stack<DeserializeObjectContainer> ContainerStack { get; set; }

        /// <summary>
        /// 反序列化根类型
        /// </summary>
        public Type RootType { get; set; }

        /// <summary>
        /// 方法管理对象
        /// </summary>
        public IMethodManager MethodManager { get; set; }
    }
}
