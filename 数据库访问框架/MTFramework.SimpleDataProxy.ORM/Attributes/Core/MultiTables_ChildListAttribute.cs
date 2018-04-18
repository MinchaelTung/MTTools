using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料子级集合成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MultiTables_ChildListAttribute : Attribute
    {
        private int? itemTableIndex;
        private Type itemType;
        private string multiTablesName;

        /// <summary>
        /// 资料子级集合成员标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        /// <param name="itemType">集合类型</param>
        public MultiTables_ChildListAttribute(string multiTablesName, Type itemType)
        {
            this.multiTablesName = string.Empty;
            this.itemTableIndex = null;
            this.multiTablesName = multiTablesName;
            this.itemType = itemType;
        }

        /// <summary>
        /// 资料子级集合成员标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        /// <param name="itemType">集合类型</param>
        /// <param name="itemTableIndex">集合表索引</param>
        public MultiTables_ChildListAttribute(string multiTablesName, Type itemType, int itemTableIndex)
        {
            this.multiTablesName = string.Empty;
            this.itemTableIndex = null;
            this.multiTablesName = multiTablesName;
            this.itemType = itemType;
            this.itemTableIndex = new int?(itemTableIndex);
        }

        /// <summary>
        /// 集合表索引
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
        /// 集合类型
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
        /// 集合表名称
        /// </summary>
        public string MultiTablesName
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
