using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料集合表成员父类属性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MultiTables_ParentAttribute : Attribute
    {
        private string multiTablesName = string.Empty;

        /// <summary>
        /// 资料集合表成员父类属性标记
        /// </summary>
        /// <param name="multiTablesName">资料集合表成员父类属性标记</param>
        public MultiTables_ParentAttribute(string multiTablesName)
        {
            this.multiTablesName = multiTablesName;
        }

        /// <summary>
        /// 资料集合表成员父类属性标记
        /// </summary>
        public string MultiTablesName
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
