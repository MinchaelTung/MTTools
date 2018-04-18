
namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 存储过程更新语句标记
    /// </summary>
    public class UpdateAccessAttribute : AccessAttribute
    {
        /// <summary>
        /// 存储过程更新语句标记
        /// </summary>
        /// <param name="procedure">存储过程字符串</param>
        public UpdateAccessAttribute(string procedure)
            : base(AccessType.Update, procedure)
        {
        }

        /// <summary>
        /// 存储过程更新语句标记
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="procedure">存储过程字符串</param>
        public UpdateAccessAttribute(string name, string procedure)
            : base(AccessType.Update, name, procedure)
        {
        }
    }
}
