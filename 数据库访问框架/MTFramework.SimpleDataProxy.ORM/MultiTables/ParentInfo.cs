using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// ParentInfo
    /// </summary>
    [Attribute_Class(typeof(MultiTables_ParentAttribute), RestrictPropertyName = "MultiTablesName")]
    internal class ParentInfo
    {
        private IMember member;
        private string multiTablesName = string.Empty;
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
