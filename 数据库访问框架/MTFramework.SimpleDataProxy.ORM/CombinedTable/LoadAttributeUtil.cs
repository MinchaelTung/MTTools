using System;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// LoadAttributeUtil
    /// </summary>
    internal static class LoadAttributeUtil
    {
        /// <summary>
        /// GetObjectInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="tableName">tableName</param>
        /// <returns></returns>
        internal static ObjectInfo GetObjectInfo(Type type, string tableName)
        {
            ObjectInfo info = (ObjectInfo)AttributeLoader.LoadData(type, typeof(ObjectInfo));
            info.Type = type;
            return info;
        }
    }
}
