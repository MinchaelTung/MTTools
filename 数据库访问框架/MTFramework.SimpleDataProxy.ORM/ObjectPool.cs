using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// ObjectPool
    /// </summary>
    internal class ObjectPool
    {
        private const string CombinedTableObjectDuplication = "有在表中的行具有相同ID.(Type:{0} ID:{1})";

        private Dictionary<Type, Dictionary<object, object>> objectPool = new Dictionary<Type, Dictionary<object, object>>();
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        internal void Add(Type type, object key, object value)
        {
            Dictionary<object, object> dictionary;
            if (this.objectPool.ContainsKey(type))
            {
                dictionary = this.objectPool[type];
            }
            else
            {
                dictionary = new Dictionary<object, object>();
                this.objectPool.Add(type, dictionary);
            }
            if (dictionary.ContainsKey(key))
            {
                throw new ORMException(string.Format(CombinedTableObjectDuplication, type, key));
            }
            dictionary.Add(key, value);
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="key">key</param>
        /// <returns>object</returns>
        internal object Get(Type type, object key)
        {
            if (this.objectPool.ContainsKey(type))
            {
                Dictionary<object, object> dictionary = this.objectPool[type];
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
            }
            return null;
        }
    }
}
