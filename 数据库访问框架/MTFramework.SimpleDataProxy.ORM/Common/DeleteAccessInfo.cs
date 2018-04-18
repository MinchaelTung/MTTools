
using MTFramework.SimpleDataProxy.ORM.Attributes;
namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 删除资料存储过程标记
    /// </summary>
    public class DeleteAccessInfo : AccessInfo
    {
        /// <summary>
        /// 删除资料存储过程标记
        /// </summary>
        public DeleteAccessInfo()
        {
            base.Accesstype = AccessType.Delete;
        }
    }
}
