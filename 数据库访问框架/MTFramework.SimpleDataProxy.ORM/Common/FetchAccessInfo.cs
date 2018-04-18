using System;
using System.Collections.Generic;
using System.Text;
using MTFramework.SimpleDataProxy.ORM.Attributes;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 填充存储资料过程标记
    /// </summary>
    public class FetchAccessInfo : AccessInfo
    {
        /// <summary>
        /// 填充存资料储过程标记
        /// </summary>
        public FetchAccessInfo()
        {
            base.Accesstype = AccessType.Fetch;
        }
    }
}
