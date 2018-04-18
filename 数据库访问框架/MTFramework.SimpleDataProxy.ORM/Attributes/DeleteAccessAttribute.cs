
namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 存储过程删除资料语句标记
    /// </summary>
    public class DeleteAccessAttribute : AccessAttribute
    {
        /// <summary>
        /// 实例化存储过程删除资料语句标记
        /// </summary>
        /// <param name="procedure">存储过程名称</param>
        public DeleteAccessAttribute(string procedure)
            : base(AccessType.Delete, procedure)
        {
        }

        /// <summary>
        /// 实例化存储过程删除资料语句标记
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="procedure">存储过程字符串</param>
        public DeleteAccessAttribute(string name, string procedure)
            : base(AccessType.Delete, name, procedure)
        {
        }
    }
}
