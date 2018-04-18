using System;
using System.Collections.Generic;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 对象信息
    /// </summary>
    [Attribute_ClassMember(typeof(AccessAttribute), typeof(Attribute), "accessInfoList", typeof(AccessInfo))]
    public class ObjectInfo
    {
        [Attribute_List]
        private List<AccessInfo> accessInfoList = new List<AccessInfo>();
        [Attribute_MethodList("MethodInfo")]
        private List<AfterMapDataInfo> afterMapDataInfoList = new List<AfterMapDataInfo>();
        [Attribute_List("Member")]
        private List<ChildListInfo> childListInfoList = new List<ChildListInfo>();
        [Attribute_List("Member")]
        private List<ExtParameterInfo> extParameterInfoList = new List<ExtParameterInfo>();
        [Attribute_List("Member", "ColumnName")]
        private Dictionary<string, FieldInfo> fieldInfoList = new Dictionary<string, FieldInfo>();
        [Attribute_Item("Member")]
        private IDInfo idInfo;
        [Attribute_List("Member", "ColumnName")]
        private Dictionary<string, MultiFieldInfo> multiFieldInfoList = new Dictionary<string, MultiFieldInfo>();
        private System.Type type;

        /// <summary>
        /// 操作数据语句列表
        /// </summary>
        public List<AccessInfo> AccessInfoList
        {
            get
            {
                return this.accessInfoList;
            }
            set
            {
                this.accessInfoList = value;
            }
        }

        /// <summary>
        /// 数据信息列表
        /// </summary>
        public List<AfterMapDataInfo> AfterMapDataInfoList
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
        /// 子级数据集合信息
        /// </summary>
        public List<ChildListInfo> ChildListInfoList
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
        /// 参数信息
        /// </summary>
        public List<ExtParameterInfo> ExtParameterInfoList
        {
            get
            {
                return this.extParameterInfoList;
            }
            set
            {
                this.extParameterInfoList = value;
            }
        }

        /// <summary>
        /// 成员信息
        /// </summary>
        public Dictionary<string, FieldInfo> FieldInfoList
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
        /// 主键信息
        /// </summary>
        public IDInfo IdInfo
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
        /// 集合成员信息
        /// </summary>
        public Dictionary<string, MultiFieldInfo> MultiFieldInfoList
        {
            get
            {
                return this.multiFieldInfoList;
            }
            set
            {
                this.multiFieldInfoList = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public System.Type Type
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
