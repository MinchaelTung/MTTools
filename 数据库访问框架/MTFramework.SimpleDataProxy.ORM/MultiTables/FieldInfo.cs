using System.Data;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// FieldInfo
    /// </summary>
    [Attribute_Class(typeof(MultiTables_FieldAttribute), RestrictPropertyName = "MultiTablesName")]
    internal class FieldInfo
    {
        private string columnName = string.Empty;
        private DbType dataType = DbType.Object;
        private IMember member;
        private string multiTablesName = string.Empty;
        /// <summary>
        /// ColumnName
        /// </summary>
        internal string ColumnName
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
        internal DbType DataType
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
