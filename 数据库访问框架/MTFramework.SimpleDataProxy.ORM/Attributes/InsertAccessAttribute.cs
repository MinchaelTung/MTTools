
namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 添加资料存储过程语句标记
    /// </summary>
    public class InsertAccessAttribute : AccessAttribute
    {
        /// <summary>
        /// 添加资料存储过程语句标记
        /// </summary>
        /// <param name="procedure">存储过程字符串</param>
        public InsertAccessAttribute(string procedure)
            : base(AccessType.Create, procedure)
        {
        }

        /// <summary>
        /// 添加资料存储过程语句标记
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="procedure">存储过程字符串</param>
        public InsertAccessAttribute(string name, string procedure)
            : base(AccessType.Create, name, procedure)
        {
        }
    }
}
