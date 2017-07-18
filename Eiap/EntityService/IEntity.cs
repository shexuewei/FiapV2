
namespace Eiap
{
    /// <summary>
    /// 实体接口标识
    /// </summary>
    public interface IEntity
    { }

    /// <summary>
    /// 带主键的实体标识
    /// </summary>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface IEntity<TPrimarykey> : IEntity where TPrimarykey : struct
    {
        /// <summary>
        /// 主键，默认为ID
        /// </summary>
        [Property("Id", IsPrimaryKey = true)]
        TPrimarykey Id { get; set; }
    }
}
