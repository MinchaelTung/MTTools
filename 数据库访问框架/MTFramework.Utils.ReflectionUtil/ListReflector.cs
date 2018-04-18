using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 反射集合
    /// </summary>
    public static class ListReflector
    {
        /// <summary>
        /// ClearItem
        /// </summary>
        /// <param name="list">list</param>
        public static void ClearItem(object list)
        {
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            MethodCaller.CallMethod(list, "Clear", new object[0]);
        }
        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="item">item</param>
        /// <returns>bool</returns>
        public static bool Contains(object list, object item)
        {
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            return (bool)MethodCaller.CallMethod(list, "Contains", new object[] { item });
        }
        /// <summary>
        /// ContainsKey
        /// </summary>
        /// <param name="dictionary">dictionary</param>
        /// <param name="key">key</param>
        /// <returns>bool</returns>
        public static bool ContainsKey(object dictionary, object key)
        {
            if (dictionary == null)
            {
                throw new ArgumentException("参数:'dictionary'不能为 Null");
            }
            return (bool)MethodCaller.CallMethod(dictionary, "ContainsKey", new object[] { key });
        }
        /// <summary>
        /// ContainsValue
        /// </summary>
        /// <param name="dictionary">dictionary</param>
        /// <param name="value">value</param>
        /// <returns>bool</returns>
        public static bool ContainsValue(object dictionary, object value)
        {
            if (dictionary == null)
            {
                throw new ArgumentException("参数:'dictionary'不能为 Null");
            }
            return (bool)MethodCaller.CallMethod(dictionary, "ContainsValue", new object[] { value });
        }
        /// <summary>
        /// DictionaryAddItem
        /// </summary>
        /// <param name="dictionary">dictionary</param>
        /// <param name="key">key</param>
        /// <param name="item">item</param>
        public static void DictionaryAddItem(object dictionary, object key, object item)
        {
            if (dictionary == null)
            {
                throw new ArgumentException("参数:'dictionary'不能为 Null");
            }
            MethodCaller.CallMethod(dictionary, "Add", new object[] { key, item });
        }
        /// <summary>
        /// ListAddItem
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="item">item</param>
        public static void ListAddItem(object list, object item)
        {
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            MethodCaller.CallMethod(list, "Add", new object[] { item });
        }
    }

}
