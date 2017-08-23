
namespace Eiap
{
    /// <summary>
    /// 删除标识
    /// </summary>
    public interface ISoftDeleted
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
