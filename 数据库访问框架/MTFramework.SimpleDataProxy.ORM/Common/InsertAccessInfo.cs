using MTFramework.SimpleDataProxy.ORM.Attributes;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 添加数据存储过程信息
    /// </summary>
    public class InsertAccessInfo : AccessInfo
    {
        /// <summary>
        /// 添加数据存储过程信息
        /// </summary>
        public InsertAccessInfo()
        {
            base.Accesstype = AccessType.Create;
        }
    }
}
