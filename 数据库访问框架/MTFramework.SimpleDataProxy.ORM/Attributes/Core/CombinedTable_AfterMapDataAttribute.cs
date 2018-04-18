using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 集合填充表标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CombinedTable_AfterMapDataAttribute : Attribute
    {
        private string tableName = string.Empty;

        /// <summary>
        /// 实例化集合填充表标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        public CombinedTable_AfterMapDataAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        /// <summary>
        /// 表名称
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
