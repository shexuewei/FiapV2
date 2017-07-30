
namespace Eiap.NetFramework
{
    /// <summary>
    /// 序列化对象容器类型
    /// </summary>
    public enum SerializeObjectContainerType
    {
        /// <summary>
        /// 数组开始
        /// </summary>
        List_Begin = 0,

        /// <summary>
        /// 数组结束
        /// </summary>
        List_End = 1,

        /// <summary>
        /// 对象开始
        /// </summary>
        Object_Begin = 2,

        /// <summary>
        /// 对象结束
        /// </summary>
        Object_End = 3,

        /// <summary>
        /// 属性正常
        /// </summary>
        Property_Normal = 4,

        /// <summary>
        /// 最后一个属性
        /// </summary>
        Property_End = 5,

        /// <summary>
        /// 字典开始
        /// </summary>
        DictionaryKey_Begin = 6,

        /// <summary>
        /// 字典结束
        /// </summary>
        DictionaryKey_End = 7,

        /// <summary>
        /// 分隔符
        /// </summary>
        SeparateSymbol = 8,

        Array_Begin = 9,

        Array_End = 10
    }

    public enum DeserializeObjectContainerType
    {
        List = 0,
        Object = 1,
        Property = 2,
        DictionaryKey = 3,
        Value_String = 4,
        Value_Int = 5,
        Value_Decimal = 6,
        Value_DateTime = 7,
        Value_Bool = 8
    }
}
