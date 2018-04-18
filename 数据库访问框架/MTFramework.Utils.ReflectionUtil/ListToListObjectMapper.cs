using System;
using System.Collections;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// ListToListObjectMapper
    /// </summary>
    public static class ListToListObjectMapper
    {
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="targetList">targetList</param>
        public static void Map(IList sourceList, IList targetList)
        {
            Map(sourceList, (Type)null, targetList);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="targetList">targetList</param>
        public static void Map(object[] sourceList, IList targetList)
        {
            Map(sourceList, (Type)null, targetList);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="targetList">targetList</param>
        /// <param name="targetType">targetType</param>
        public static void Map(IList sourceList, IList targetList, Type targetType)
        {
            Map(sourceList, null, targetList, targetType);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="parentType">parentType</param>
        /// <param name="targetList">targetList</param>
        public static void Map(IList sourceList, Type parentType, IList targetList)
        {
            Type childItemType = ReflectionUtilities.GetChildItemType(targetList.GetType());
            Map(sourceList, parentType, targetList, childItemType);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="targetList">targetList</param>
        /// <param name="targetType">targetType</param>
        public static void Map(object[] sourceList, IList targetList, Type targetType)
        {
            Map(sourceList, null, targetList, targetType);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="parentType">parentType</param>
        /// <param name="targetList">targetList</param>
        public static void Map(object[] sourceList, Type parentType, IList targetList)
        {
            Type childItemType = ReflectionUtilities.GetChildItemType(targetList.GetType());
            Map(sourceList, parentType, targetList, childItemType);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="sourceList">sourceList</param>
        /// <param name="parentType">parentType</param>
        /// <param name="targetList">targetList</param>
        /// <param name="targetType">targetType</param>
        public static void Map(IList sourceList, Type parentType, IList targetList, Type targetType)
        {
            if (sourceList == null)
            {
                throw new ArgumentException("参数:'sourceList'不能为 Null");
            }
            if (targetList == null)
            {
                throw new ArgumentException("参数:'targetList'不能为 Null");
            }
            if (targetType == null)
            {
                throw new ArgumentException("参数:'targetType'不能为 Null");
            }
            targetList.Clear();
            foreach (object obj2 in sourceList)
            {
                object target = null;
                try
                {
                    target = MethodCaller.CreateInstance(targetType);
                }
                catch (Exception exception)
                {
                    throw new ReflectionException(string.Format("创建Object错误（Type:{0})", targetType), exception);
                }
                if (parentType == null)
                {
                    ObjectToObjectMapper.Map(obj2, obj2.GetType(), target);
                }
                else
                {
                    ObjectToObjectMapper.Map(obj2, parentType, target);
                }
                targetList.Add(target);
            }
        }
    }
}
