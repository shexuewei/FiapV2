
using System;
using System.Collections.Generic;

namespace Eiap.NetFramework
{
    public interface ISerializeContainerManager : ISingletonDependency, IDynamicProxyDisable
    {
        LinkedList<SerializeObjectContainer> GetOrAddSerializeObject(Type serializeObjectType);
    }
}
