using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// 资料集合表成员标记
    /// </summary>
    [Attribute_Class(typeof(MultiTables_AfterMapDataAttribute), RestrictPropertyName = "MultiTablesName")]
    internal class AfterMapDataInfo
    {
        private System.Reflection.MethodInfo methodInfo;
        private string multiTablesName = string.Empty;
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
