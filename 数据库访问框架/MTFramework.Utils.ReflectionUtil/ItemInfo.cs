
namespace MTFramework.Utils.ReflectionUtil
{
    internal class ItemInfo
    {
        private IMember member;
        private IMember memberPropertyMember;
        private string memberPropertyName;

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
