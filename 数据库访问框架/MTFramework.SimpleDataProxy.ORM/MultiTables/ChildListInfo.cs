using System;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// ChildListInfo
    /// </summary>
    [Attribute_Class(typeof(MultiTables_ChildListAttribute), RestrictPropertyName = "MultiTablesName")]
    internal class ChildListInfo
    {
        private int? itemTableIndex = null;
        private Type itemType;
        private IMember member;
        private string multiTablesName = string.Empty;
        /// <summary>
        /// ItemTableIndex
        /// </summary>
        public int? ItemTableIndex
        {
            get
            {
                return this.itemTableIndex;
            }
            set
            {
                this.itemTableIndex = value;
            }
        }
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
        /// MultiTablesName
        /// </summary>
        internal string MultiTablesName
        {
            get
            {
                return this.multiTablesName;
            }
            set
            {
                this.multiTablesName = value;
            }
        }
    }

}
