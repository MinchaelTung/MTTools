using System;
using System.Data;
namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 集合表的成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class CombinedTable_FieldAttribute : Attribute
    {
        private string columnName;
        private DbType dataType;
        private string tableName;

        /// <summary>
        /// 实例化集合表的成员标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列表名称</param>
        public CombinedTable_FieldAttribute(string tableName, string columnName)
            : this(tableName, columnName, DbType.Object)
        {
        }

        /// <summary>
        /// 实例化集合表的成员标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列表名称</param>
        /// <param name="dataType">列的数据类型</param>
        public CombinedTable_FieldAttribute(string tableName, string columnName, DbType dataType)
        {
            this.tableName = string.Empty;
            this.columnName = string.Empty;
            this.dataType = DbType.Object;
            this.tableName = tableName;
            this.columnName = columnName;
            this.dataType = dataType;
        }

        /// <summary>
        /// 列表名称
        /// </summary>
        public string ColumnName
        {
            get
            {
                return this.columnName;
            }
            set
            {
                this.columnName = value;
            }
        }

        /// <summary>
        /// 列的数据类型
        /// </summary>
        public DbType DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
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
