using System;

namespace MTFramework.Utils.ReflectionUtil
{
    internal class ClassMemberInfo
    {
        private Type attributeType;
        private Type mapType;
        private string memberName;
        private Type parentType;

        public Type AttributeType
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

        public Type MapType
        {
            get
            {
                return this.mapType;
            }
            set
            {
                this.mapType = value;
            }
        }

        internal string MemberName
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

        public Type ParentType
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
    }

}
