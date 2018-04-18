using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// ParentInfo
    /// </summary>
    [Attribute_Class(typeof(CombinedTable_ParentAttribute))]
    internal class ParentInfo
    {
        private string foreighKeyColumnName = string.Empty;
        private IMember member;
        private string tableName = string.Empty;
        /// <summary>
        /// ForeighKeyColumnName
        /// </summary>
        public string ForeighKeyColumnName
        {
            get
            {
                return this.foreighKeyColumnName;
            }
            set
            {
                this.foreighKeyColumnName = value;
            }
        }
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
        /// TableName
        /// </summary>
        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }
    }
}
