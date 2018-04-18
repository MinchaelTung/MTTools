using System.Collections.Generic;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// ClassInfo
    /// </summary>
    [Attribute_Class(typeof(MultiTables_ClassAttribute), RestrictPropertyName = "MultiTablesName")]
    internal class ClassInfo
    {
        [Attribute_MethodList("MethodInfo")]
        private List<AfterMapDataInfo> afterMapDataInfoList = new List<AfterMapDataInfo>();
        [Attribute_List("Member")]
        private List<ChildListInfo> childListInfoList = new List<ChildListInfo>();
        [Attribute_List("Member", "ColumnName")]
        private Dictionary<string, FieldInfo> fieldInfoList = new Dictionary<string, FieldInfo>();
        [Attribute_Item("Member")]
        private IDInfo idInfo;
        private string multiTablesName = string.Empty;
        private string parentColumnName;
        [Attribute_Item("Member")]
        private ParentInfo parentInfo;
        private int tableIndex;
        private System.Type type;
        /// <summary>
        /// AfterMapDataInfoList
        /// </summary>
        internal List<AfterMapDataInfo> AfterMapDataInfoList
        {
            get
            {
                return this.afterMapDataInfoList;
            }
            set
            {
                this.afterMapDataInfoList = value;
            }
        }
        /// <summary>
        /// ChildListInfoList
        /// </summary>
        internal List<ChildListInfo> ChildListInfoList
        {
            get
            {
                return this.childListInfoList;
            }
        }
        /// <summary>
        /// FieldInfoList
        /// </summary>
        internal Dictionary<string, FieldInfo> FieldInfoList
        {
            get
            {
                return this.fieldInfoList;
            }
        }
        /// <summary>
        /// IdInfo
        /// </summary>
        internal IDInfo IdInfo
        {
            get
            {
                return this.idInfo;
            }
            set
            {
                this.idInfo = value;
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
        /// <summary>
        /// ParentColumnName
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
        /// ParentInfo
        /// </summary>
        internal ParentInfo ParentInfo
        {
            get
            {
                return this.parentInfo;
            }
            set
            {
                this.parentInfo = value;
            }
        }
        /// <summary>
        /// TableIndex
        /// </summary>
        internal int TableIndex
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
        /// <summary>
        /// Type
        /// </summary>
        internal System.Type Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}
