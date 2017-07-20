
namespace Eiap.NetFramework
{
    /// <summary>
    /// 反序列化对象容器类型
    /// </summary>
    public enum DeserializeObjectContainerType
    {
        /// <summary>
        /// 数组
        /// </summary>
        List = 0,

        /// <summary>
        /// 对象
        /// </summary>
        Object = 1,

        /// <summary>
        /// 属性
        /// </summary>
        Property = 2,

        /// <summary>
        /// 字典
        /// </summary>
        DictionaryKey = 3,

        /// <summary>
        /// 值字符串
        /// </summary>
        Value_String = 4,

        /// <summary>
        /// 值整型
        /// </summary>
        Value_Int = 5,

        /// <summary>
        /// 值小数
        /// </summary>
        Value_Decimal = 6,

        /// <summary>
        /// 值日期
        /// </summary>
        Value_DateTime = 7,

        /// <summary>
        /// 值布尔
        /// </summary>
        Value_Bool = 8
    }
}
