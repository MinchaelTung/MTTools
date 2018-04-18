using System.Data;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// FieldInfo
    /// </summary>
    [Attribute_Class(typeof(CombinedTable_FieldAttribute))]
    internal class FieldInfo
    {
        private string columnName = string.Empty;
        private DbType dataType = DbType.Object;
        private IMember member;
        private string tableName = string.Empty;
        /// <summary>
        /// ColumnName
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
        /// DataType
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
