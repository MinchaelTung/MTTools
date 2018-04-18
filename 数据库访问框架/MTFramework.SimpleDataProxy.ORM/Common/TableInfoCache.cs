using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 表信息缓存
    /// </summary>
    public static class TableInfoCache
    {
        private static Dictionary<string, TableInfo> pool = new Dictionary<string, TableInfo>();
        private static readonly object syncObj = new object();

        /// <summary>
        /// 读取对象信息
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>ObjectInfo</returns>
        public static ObjectInfo GetObjectInfo(Type type)
        {
            TableInfo tableInfo = GetTableInfo(type);
            if ((tableInfo != null) && tableInfo.ObjectInfoPool.ContainsKey(type))
            {
                return tableInfo.ObjectInfoPool[type];
            }
            return null;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>TableInfo</returns>
        public static TableInfo GetTableInfo(Type type)
        {
            string fullName = type.FullName;
            lock (syncObj)
            {
                if (pool.ContainsKey(fullName))
                {
                    return pool[fullName];
                }
                TableInfo tableInfo = TableInfo.GetTableInfo(type);
                pool.Add(fullName, tableInfo);
                return tableInfo;
            }
        }
    }
}
