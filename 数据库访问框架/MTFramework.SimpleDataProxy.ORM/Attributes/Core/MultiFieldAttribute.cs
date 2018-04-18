using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{

    /// <summary>
    /// 资料子级成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MultiFieldAttribute : Attribute
    {
        private DbType _dataType;
        private string columnName;
        private string memberName;

        /// <summary>
        /// 资料子级成员标记
        /// </summary>
        public MultiFieldAttribute()
        {
            this.columnName = string.Empty;
            this._dataType = DbType.Object;
        }

        /// <summary>
        /// 资料子级成员标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        public MultiFieldAttribute(string columnName, DbType dataType)
        {
            this.columnName = string.Empty;
            this._dataType = DbType.Object;
            this.columnName = columnName;
            this._dataType = dataType;
        }

        /// <summary>
        /// 资料子级成员标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <param name="memberName">成员名称</param>
        public MultiFieldAttribute(string columnName, string memberName)
        {
            this.columnName = string.Empty;
            this._dataType = DbType.Object;
            this.columnName = columnName;
            this.memberName = memberName;
        }

        /// <summary>
        /// 资料子级成员标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="memberName">成员名称</param>
        public MultiFieldAttribute(string columnName, DbType dataType, string memberName)
        {
            this.columnName = string.Empty;
            this._dataType = DbType.Object;
            this.columnName = columnName;
            this._dataType = dataType;
            this.memberName = memberName;
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
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName
        {
            get
            {
                return this.memberName;
            }
            set
            {
                this.memberName = value;
            }
        }
    }

}
