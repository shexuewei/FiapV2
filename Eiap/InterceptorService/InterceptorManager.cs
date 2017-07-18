
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eiap
{
    /// <summary>
    /// 拦截器管理
    /// </summary>
    public class InterceptorManager : IInterceptorManager
    {
        private static IInterceptorManager _Manager = null;
        private IInterceptorMethodManager _interceptorMethodManager = null;

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Initialization()
        {
            _interceptorMethodManager = DependencyManager.Instance.Resolver<IInterceptorMethodManager>();
        }

        private InterceptorManager()
        {
            Initialization();
        }

        /// <summary>
        /// 单例对象
        /// </summary>
        public static IInterceptorManager Instance
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new InterceptorManager();
                }
                return _Manager;
            }
        }

        /// <summary>
        /// 根据程序集集合，注册拦截器
        /// </summary>
        /// <param name="assemblyList"></param>
        public void Register(List<Assembly> assemblyList)
        {
            assemblyList.ForEach(assemblyItem => {
                assemblyItem.GetTypes().Where(m => typeof(IInterceptorMethod).IsAssignableFrom(m)).ToList().ForEach(interceptorMethodItem => {
                    _interceptorMethodManager.RegisterAttibuteAndInterceptorMethod(interceptorMethodItem);
                });
            });
        }
    }
}
