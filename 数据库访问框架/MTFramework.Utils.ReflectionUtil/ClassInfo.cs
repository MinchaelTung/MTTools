using System.Collections.Generic;

namespace MTFramework.Utils.ReflectionUtil
{
    internal class ClassInfo
    {
        private System.Type attributeType;
        private List<ClassMemberInfo> classMemberList = new List<ClassMemberInfo>();
        private bool hasAttribute;
        private List<ItemInfo> itemInfoList = new List<ItemInfo>();
        private List<ListInfo> listInfoList = new List<ListInfo>();
        private List<Method> methodList = new List<Method>();
        private List<MethodList> methodListList = new List<MethodList>();
        private System.Type parentType;
        private IMember restrictPropertyMember;
        private string restrictPropertyName;
        private System.Type type;

        internal System.Type AttributeType
        {
            get
            {
                return this.attributeType;
            }
            set
            {
                this.attributeType = value;
            }
        }

        internal List<ClassMemberInfo> ClassMemberList
        {
            get
            {
                return this.classMemberList;
            }
        }

        public bool HasAttribute
        {
            get
            {
                return this.hasAttribute;
            }
            set
            {
                this.hasAttribute = value;
            }
        }

        internal List<ItemInfo> ItemInfoList
        {
            get
            {
                return this.itemInfoList;
            }
        }

        internal List<ListInfo> ListInfoList
        {
            get
            {
                return this.listInfoList;
            }
        }

        internal List<Method> MethodList
        {
            get
            {
                return this.methodList;
            }
        }

        internal List<MethodList> MethodListList
        {
            get
            {
                return this.methodListList;
            }
        }

        internal System.Type ParentType
        {
            get
            {
                return this.parentType;
            }
            set
            {
                this.parentType = value;
            }
        }

        public IMember RestrictPropertyMember
        {
            get
            {
                return this.restrictPropertyMember;
            }
            set
            {
                this.restrictPropertyMember = value;
            }
        }

        public string RestrictPropertyName
        {
            get
            {
                return this.restrictPropertyName;
            }
            set
            {
                this.restrictPropertyName = value;
            }
        }

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
