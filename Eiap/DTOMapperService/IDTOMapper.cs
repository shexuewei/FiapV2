
namespace Eiap
{
    /// <summary>
    /// 对象转换接口
    /// </summary>
    public interface IDTOMapper : IPropertyDependency, IRealtimeDependency, IDynamicProxyDisable
    {
        T Mapper<T>(object entity);

        T Mapper<T>(object entity, object mapperEntity);
    }
}
