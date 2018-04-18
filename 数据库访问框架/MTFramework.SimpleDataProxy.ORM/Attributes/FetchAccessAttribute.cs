using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 读取的存储过程标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FetchAccessAttribute : AccessAttribute
    {
        /// <summary>
        /// 读取的存储过程标记
        /// </summary>
        /// <param name="procedure">存储过程字符串</param>
        public FetchAccessAttribute(string procedure)
            : base(AccessType.Fetch, procedure)
        {
        }

        /// <summary>
        /// 读取的存储过程标记
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="procedure">存储过程字符串</param>
        public FetchAccessAttribute(string name, string procedure)
            : base(AccessType.Fetch, name, procedure)
        {
        }
    }
}
