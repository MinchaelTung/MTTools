using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class FieldAttribute : Attribute
    {
        private string columnName;
        private DbType dataType;

        /// <summary>
        /// 资料成员标记
        /// </summary>
        public FieldAttribute()
        {
            this.columnName = string.Empty;
            this.dataType = DbType.Object;
        }

        /// <summary>
        /// 资料成员标记
        /// </summary>
        /// <param name="dataType">数据类型</param>
        public FieldAttribute(DbType dataType)
        {
            this.columnName = string.Empty;
            this.dataType = DbType.Object;
            this.dataType = dataType;
        }

        /// <summary>
        /// 资料成员标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        public FieldAttribute(string columnName)
            : this(columnName, DbType.Object)
        {
        }

        /// <summary>
        /// 资料成员标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        public FieldAttribute(string columnName, DbType dataType)
        {
            this.columnName = string.Empty;
            this.dataType = DbType.Object;
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
    }
}
