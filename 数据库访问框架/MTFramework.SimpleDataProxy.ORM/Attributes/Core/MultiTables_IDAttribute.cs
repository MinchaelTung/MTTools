using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料集合表主键成员标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MultiTables_IDAttribute : MultiTables_FieldAttribute
    {
        /// <summary>
        /// 资料集合表主键成员标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        /// <param name="columnName">列名称</param>
        public MultiTables_IDAttribute(string multiTablesName, string columnName)
            : base(multiTablesName, columnName)
        {
        }

        /// <summary>
        /// 资料集合表主键成员标记
        /// </summary>
        /// <param name="multiTablesName">集合表名称</param>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        public MultiTables_IDAttribute(string multiTablesName, string columnName, DbType dataType)
            : base(multiTablesName, columnName, dataType)
        {
        }
    }
}
