using System;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 子集合信息
    /// </summary>
    [Attribute_Class(typeof(Common_ChildListAttribute))]
    public class ChildListInfo
    {
        private Type itemType;
        private IMember member;
        private string tableName = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public Type ItemType
        {
            get
            {
                return this.itemType;
            }
            set
            {
                this.itemType = value;
            }
        }

        /// <summary>
        /// 成员
        /// </summary>
        public IMember Member
        {
            get
            {
                return this.member;
            }
            set
            {
                this.member = value;
            }
        }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }
    }

}
