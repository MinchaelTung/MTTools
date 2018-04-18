using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料子级类成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MultiTables_ClassAttribute : Attribute
    {
        private string multiTablesName;
        private string parentColumnName;
        private int tableIndex;

        /// <summary>
        /// 资料子级类成员标记
        /// </summary>
        /// <param name="multiTablesName">表名称</param>
        public MultiTables_ClassAttribute(string multiTablesName)
        {
            this.multiTablesName = string.Empty;
            this.multiTablesName = multiTablesName;
        }

        /// <summary>
        /// 资料子级类成员标记
        /// </summary>
        /// <param name="multiTablesName">表名称</param>
        /// <param name="tableIndex">表索引</param>
        public MultiTables_ClassAttribute(string multiTablesName, int tableIndex)
            : this(multiTablesName, tableIndex, null)
        {
        }

        /// <summary>
        /// 资料子级类成员标记
        /// </summary>
        /// <param name="multiTablesName">表名称</param>
        /// <param name="parentColumnName">父级表列名称</param>
        public MultiTables_ClassAttribute(string multiTablesName, string parentColumnName)
        {
            this.multiTablesName = string.Empty;
            this.multiTablesName = multiTablesName;
            this.parentColumnName = parentColumnName;
        }

        /// <summary>
        /// 资料子级类成员标记
        /// </summary>
        /// <param name="multiTablesName">表名称</param>
        /// <param name="tableIndex">表索引</param>
        /// <param name="parentColumnName">父级表列名称</param>
        public MultiTables_ClassAttribute(string multiTablesName, int tableIndex, string parentColumnName)
        {
            this.multiTablesName = string.Empty;
            this.multiTablesName = multiTablesName;
            this.tableIndex = tableIndex;
            this.parentColumnName = parentColumnName;
        }

        /// <summary>
        /// 表名称
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

        /// <summary>
        /// 父级表列名称
        /// </summary>
        public string ParentColumnName
        {
            get
            {
                return this.parentColumnName;
            }
            set
            {
                this.parentColumnName = value;
            }
        }

        /// <summary>
        /// 表索引
        /// </summary>
        public int TableIndex
        {
            get
            {
                return this.tableIndex;
            }
            set
            {
                this.tableIndex = value;
            }
        }
    }
}
