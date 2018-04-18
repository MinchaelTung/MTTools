using System;

namespace MTFramework.Utils.ReflectionUtil
{
    internal class ListInfo
    {
        private Type itemType;
        private IMember keyProperty;
        private string keyPropertyName;
        private IMember member;
        private IMember memberPropertyMember;
        private string memberPropertyName;

        internal Type ItemType
        {
            get
            {
                return this.itemType;
            }
            set
            {
                this.itemType = value;
            }
        }

        internal IMember KeyProperty
        {
            get
            {
                return this.keyProperty;
            }
            set
            {
                this.keyProperty = value;
            }
        }

        internal string KeyPropertyName
        {
            get
            {
                return this.keyPropertyName;
            }
            set
            {
                this.keyPropertyName = value;
            }
        }

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

        public IMember MemberPropertyMember
        {
            get
            {
                return this.memberPropertyMember;
            }
            set
            {
                this.memberPropertyMember = value;
            }
        }

        internal string MemberPropertyName
        {
            get
            {
                return this.memberPropertyName;
            }
            set
            {
                this.memberPropertyName = value;
            }
        }
    }
}
