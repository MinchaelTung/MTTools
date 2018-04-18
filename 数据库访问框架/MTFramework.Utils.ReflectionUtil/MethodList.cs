
namespace MTFramework.Utils.ReflectionUtil
{
    internal class MethodList
    {
        private IMember member;
        private IMember methodInfoPropertyMember;
        private string methodInfoPropertyName = string.Empty;

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

        internal IMember MethodInfoPropertyMember
        {
            get
            {
                return this.methodInfoPropertyMember;
            }
            set
            {
                this.methodInfoPropertyMember = value;
            }
        }

        internal string MethodInfoPropertyName
        {
            get
            {
                return this.methodInfoPropertyName;
            }
            set
            {
                this.methodInfoPropertyName = value;
            }
        }
    }

}
