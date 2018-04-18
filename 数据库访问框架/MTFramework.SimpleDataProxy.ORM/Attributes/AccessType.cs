
namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 执行存储过程类型
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// 填充
        /// </summary>
        Fetch,
        /// <summary>
        /// 添加
        /// </summary>
        Create,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }
}
