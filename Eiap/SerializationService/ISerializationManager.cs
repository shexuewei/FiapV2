
namespace Eiap
{
    /// <summary>
    /// 序列化管理接口
    /// </summary>
    public interface ISerializationManager : ISingletonDependency, IDynamicProxyDisable
    {
        /// <summary>
        /// 根据设置将对象序列化成字符串
        /// </summary>
        /// <param name="serializeObject"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        string SerializeObject(object serializeObject, SerializationSetting setting = null);

        /// <summary>
        /// 将字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        T DeserializeObject<T>(string value, SerializationSetting setting = null);
    }
}
