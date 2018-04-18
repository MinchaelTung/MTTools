using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 集合表中的子集合标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class CombinedTable_ChildListAttribute : Attribute
    {
        private Type itemType;
        private string tableName = string.Empty;

        /// <summary>
        /// 集合表中的子集合标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="itemType">集合类型</param>
        public CombinedTable_ChildListAttribute(string tableName, Type itemType)
        {
            this.tableName = tableName;
            this.itemType = itemType;
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
