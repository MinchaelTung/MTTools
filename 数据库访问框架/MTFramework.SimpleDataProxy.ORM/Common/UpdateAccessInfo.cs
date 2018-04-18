using MTFramework.SimpleDataProxy.ORM.Attributes;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// UpdateAccessInfo
    /// </summary>
    internal class UpdateAccessInfo : AccessInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public UpdateAccessInfo()
        {
            base.Accesstype = AccessType.Update;
        }
    }
}
