using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 集合表的父类标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class CombinedTable_ParentAttribute : Attribute
    {
        private string tableName = string.Empty;

        /// <summary>
        /// 集合表的父类标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        public CombinedTable_ParentAttribute(string tableName)
        {
            this.tableName = tableName;
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
