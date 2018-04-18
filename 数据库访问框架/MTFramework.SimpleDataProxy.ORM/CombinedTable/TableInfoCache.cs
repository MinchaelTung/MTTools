using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// TableInfoCache
    /// </summary>
    internal static class TableInfoCache
    {
        private static Dictionary<string, TableInfo> pool = new Dictionary<string, TableInfo>();
        private static readonly object syncObj = new object();
        /// <summary>
        /// GetTableInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="tableName">tableName</param>
        /// <returns></returns>
        internal static TableInfo GetTableInfo(Type type, string tableName)
        {
            lock (syncObj)
            {
                string key = string.Format("{0}/{1}", type, tableName);
                if (pool.ContainsKey(key))
                {
                    return pool[key];
                }
                TableInfo tableInfo = TableInfo.GetTableInfo(type, tableName);
                pool.Add(key, tableInfo);
                return tableInfo;
            }
        }
    }
}
