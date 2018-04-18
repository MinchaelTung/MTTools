using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// AfterMapDataInfo
    /// </summary>
    [Attribute_Class(typeof(CombinedTable_AfterMapDataAttribute))]
    internal class AfterMapDataInfo
    {
        private System.Reflection.MethodInfo methodInfo;
        private string tableName = string.Empty;
        /// <summary>
        /// MethodInfo
        /// </summary>
        internal System.Reflection.MethodInfo MethodInfo
        {
            get
            {
                return this.methodInfo;
            }
            set
            {
                this.methodInfo = value;
            }
        }
        /// <summary>
        /// TableName
        /// </summary>
        internal string TableName
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
