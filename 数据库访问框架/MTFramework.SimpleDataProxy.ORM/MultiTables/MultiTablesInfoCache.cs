using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// MultiTablesInfoCache
    /// </summary>
    internal class MultiTablesInfoCache
    {
        private static Dictionary<string, MultiTablesInfo> pool = new Dictionary<string, MultiTablesInfo>();
        private static readonly object _syncObj = new object();
        /// <summary>
        /// GetMultiTablesInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <returns>MultiTablesInfo</returns>
        internal static MultiTablesInfo GetMultiTablesInfo(Type type, string multiTablesName)
        {
            lock (_syncObj)
            {
                string key = string.Format("{0}/{1}", type, multiTablesName);
                if (pool.ContainsKey(key))
                {
                    return pool[key];
                }
                MultiTablesInfo mutilTablesInfo = MultiTablesInfo.GetMutilTablesInfo(type, multiTablesName);
                pool.Add(key, mutilTablesInfo);
                return mutilTablesInfo;
            }
        }
    }
}
