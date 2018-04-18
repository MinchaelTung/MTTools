using System;
using System.Collections.Generic;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// Class信息缓存
    /// </summary>
    internal static class ClassInfoCache
    {
        private static Dictionary<Type, ClassInfo> pool = new Dictionary<Type, ClassInfo>();
        private static readonly object syncObj = new object();
        /// <summary>
        /// 获取Class信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static ClassInfo GetClassInfo(Type type)
        {
            lock (syncObj)
            {
                if (pool.ContainsKey(type))
                {
                    return pool[type];
                }
                ClassInfo classInfo = LoadAttributeUtil.GetClassInfo(type);
                pool.Add(type, classInfo);
                return classInfo;
            }
        }
    }
}
