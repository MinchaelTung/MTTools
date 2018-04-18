using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料集合表成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MultiTables_AfterMapDataAttribute : Attribute
    {
        private string multiTablesName = string.Empty;

        /// <summary>
        /// 资料集合表成员标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        public MultiTables_AfterMapDataAttribute(string multiTablesName)
        {
            this.multiTablesName = multiTablesName;
        }

        /// <summary>
        /// 集合表名称
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
