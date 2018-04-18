using System.Collections.Generic;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// ObjectInfo
    /// </summary>
    internal class ObjectInfo
    {
        [Attribute_MethodList("MethodInfo")]
        private List<AfterMapDataInfo> afterMapDataInfoList = new List<AfterMapDataInfo>();
        [Attribute_List("Member")]
        private List<ChildListInfo> childListInfoList = new List<ChildListInfo>();
        [Attribute_List("Member", "ColumnName")]
        private Dictionary<string, FieldInfo> fieldInfoList = new Dictionary<string, FieldInfo>();
        [Attribute_Item("Member")]
        private IDInfo idInfo;
        [Attribute_Item("Member")]
        private ParentInfo parentInfo;
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
            set
            {
                this.childListInfoList = value;
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
            set
            {
                this.fieldInfoList = value;
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
