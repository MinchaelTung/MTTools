using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料集合表成员属性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MultiTables_FieldAttribute : Attribute
    {
        private string columnName;
        private DbType dataType;
        private string multiTablesName;

        /// <summary>
        /// 资料集合表成员属性标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        /// <param name="columnName">列名称</param>
        public MultiTables_FieldAttribute(string multiTablesName, string columnName)
            : this(multiTablesName, columnName, DbType.Object)
        {
        }

        /// <summary>
        /// 资料集合表成员属性标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        public MultiTables_FieldAttribute(string multiTablesName, string columnName, DbType dataType)
        {
            this.multiTablesName = string.Empty;
            this.columnName = string.Empty;
            this.dataType = DbType.Object;
            this.multiTablesName = multiTablesName;
            this.columnName = columnName;
            this.dataType = dataType;
        }

        /// <summary>
        /// 列名称
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
        /// 数据类型
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
