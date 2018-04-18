using System;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// ChildListInfo
    /// </summary>
    [Attribute_Class(typeof(CombinedTable_ChildListAttribute))]
    internal class ChildListInfo
    {
        private Type itemType;
        private IMember member;
        private string tableName = string.Empty;
        /// <summary>
        /// ItemType
        /// </summary>
        internal Type ItemType
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
        /// Member
        /// </summary>
        internal IMember Member
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
        /// TableName
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
