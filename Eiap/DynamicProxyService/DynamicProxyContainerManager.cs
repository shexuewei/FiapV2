
using System.Collections.Generic;

namespace Eiap
{
    public class DynamicProxyContainerManager : IDynamicProxyContainerManager
    {
        private Dictionary<string, DynamicProxyContainer> _DynamicProxyContainerList = null;

        public DynamicProxyContainerManager()
        {
            _DynamicProxyContainerList = new Dictionary<string, DynamicProxyContainer>();
        }

        /// <summary>
        /// 添加动态代理容器
        /// </summary>
        /// <param name="container"></param>
        public void AddDynamicProxyContainer(DynamicProxyContainer container)
        {
            _DynamicProxyContainerList.Add(container.DynamicProxyTypeFullName, container);
        }

        /// <summary>
        /// 获取动态代理容器
        /// </summary>
        /// <param name="dynamicProxyTypeName"></param>
        /// <returns></returns>
        public DynamicProxyContainer GetDynamicProxyContainerByDynamicProxyTypeName(string dynamicProxyTypeName)
        {
            if (_DynamicProxyContainerList.ContainsKey(dynamicProxyTypeName))
            {
                return _DynamicProxyContainerList[dynamicProxyTypeName];
            }
            return null;
        }
    }
}
