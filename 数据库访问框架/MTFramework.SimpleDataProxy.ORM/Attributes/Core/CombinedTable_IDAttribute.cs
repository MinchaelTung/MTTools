using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 集合表主键标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class CombinedTable_IDAttribute : CombinedTable_FieldAttribute
    {
        /// <summary>
        /// 实例化集合表的主键标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列表名称</param>
        public CombinedTable_IDAttribute(string tableName, string columnName)
            : base(tableName, columnName)
        {
        }

        /// <summary>
        /// 实例化集合表的主键标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列表名称</param>
        /// <param name="dataType">列的数据类型</param>
        public CombinedTable_IDAttribute(string tableName, string columnName, DbType dataType)
            : base(tableName, columnName, dataType)
        {
        }
    }
}
