using System;
using System.Collections.Generic;
using System.Reflection;
using MTFramework.Utils.ReflectionUtil.Properties;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 对象到对象之间的映射关系
    /// </summary>
    public static class ObjectToObjectMapper
    {
        /// <summary>
        /// changPrefix
        /// </summary>
        /// <param name="fieldName">fieldName</param>
        /// <returns>string</returns>
        private static string changPrefix(string fieldName)
        {
            string str = fieldName;
            if (str.StartsWith("_"))
            {
                return str.Substring(1);
            }
            return ("_" + str);
        }
        /// <summary>
        /// findFieldInfo
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段信息</returns>
        private static FieldInfo findFieldInfo(object target, string fieldName)
        {
            Type baseType = target.GetType();
            FieldInfo info = null;
            while (baseType != null)
            {
                info = findFieldInfoInType(baseType, fieldName);
                if (info != null)
                {
                    return info;
                }
                baseType = baseType.BaseType;
            }
            return null;
        }
        /// <summary>
        /// findFieldInfoInType
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段信息</returns>
        private static FieldInfo findFieldInfoInType(Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                field = type.GetField(changPrefix(fieldName), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            return field;
        }
        /// <summary>
        /// getCoreName
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>string</returns>
        private static string getCoreName(string memberName)
        {
            string str = memberName;
            if (memberName.StartsWith("_"))
            {
                str = str.Substring(1);
            }
            return str.ToLower();
        }
        /// <summary>
        /// getMemberListLimitToParentType
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="parent">parent</param>
        /// <param name="suppressExceptions">suppressExceptions</param>
        /// <returns>返回成员内容接口的集合</returns>
        private static Dictionary<string, IMember> getMemberListLimitToParentType(Type objectType, Type parent, bool suppressExceptions)
        {
            Dictionary<string, IMember> dictionary = new Dictionary<string, IMember>();
            FieldInfo[] infoArray = ReflectionUtilities.GetFieldsLimitToParentType(objectType, parent, suppressExceptions);
            if (infoArray != null)
            {
                foreach (FieldInfo info in infoArray)
                {
                    string key = getCoreName(info.Name);
                    if (!dictionary.ContainsKey(key))
                    {
                        IMember field = MemberFactory.GetField(info);
                        dictionary.Add(key, field);
                    }
                }
            }
            PropertyInfo[] infoArray2 = ReflectionUtilities.GetPropertiesLimitToParentType(objectType, parent, suppressExceptions);
            if (infoArray2 != null)
            {
                foreach (PropertyInfo info2 in infoArray2)
                {
                    string str2 = getCoreName(info2.Name);
                    if (!dictionary.ContainsKey(str2))
                    {
                        IMember property = MemberFactory.GetProperty(info2);
                        dictionary.Add(str2, property);
                    }
                }
            }
            return dictionary;
        }
        /// <summary>
        /// getPropertyOrFieldType
        /// </summary>
        /// <param name="propertyType">propertyType</param>
        /// <returns>Type</returns>
        private static Type getPropertyOrFieldType(Type propertyType)
        {
            Type nullableType = propertyType;
            if (nullableType.IsGenericType && (nullableType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                return Nullable.GetUnderlyingType(nullableType);
            }
            return nullableType;
        }
        /// <summary>
        /// setMemberValue
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="targetMember">targetMember</param>
        /// <param name="value">value</param>
        private static void setMemberValue(object target, IMember targetMember, object value)
        {
            if (value == null)
            {
                targetMember.SetValue(target, value);
            }
            else
            {
                Type enumType = getPropertyOrFieldType(targetMember.Type);
                Type o = getPropertyOrFieldType(value.GetType());
                if (enumType.Equals(o))
                {
                    targetMember.SetValue(target, value);
                }
                else if (enumType.Equals(typeof(Guid)))
                {
                    targetMember.SetValue(target, new Guid(value.ToString()));
                }
                else if (enumType.IsEnum && o.Equals(typeof(string)))
                {
                    targetMember.SetValue(target, Enum.Parse(enumType, value.ToString()));
                }
                else
                {
                    try
                    {
                        targetMember.SetValue(target, Convert.ChangeType(value, enumType));
                    }
                    catch (System.Exception)
                    {
                        targetMember.SetValue(target, value);
                    }
                }
            }
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="target">target</param>
        public static void Map(object source, object target)
        {
            Map(source, target, false, new string[0]);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="target">target</param>
        /// <param name="ignoreList">ignoreList</param>
        public static void Map(object source, object target, params string[] ignoreList)
        {
            Map(source, target, false, ignoreList);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="target">target</param>
        /// <param name="targetParentType">targetParentType</param>
        public static void Map(object source, object target, Type targetParentType)
        {
            Map(source, source.GetType(), target, targetParentType, false, new string[0]);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="sourceParentType">sourceParentType</param>
        /// <param name="target">target</param>
        public static void Map(object source, Type sourceParentType, object target)
        {
            Map(source, sourceParentType, target, target.GetType(), false, new string[0]);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="target">target</param>
        /// <param name="suppressExceptions">suppressExceptions</param>
        /// <param name="ignoreList">ignoreList</param>
        public static void Map(object source, object target, bool suppressExceptions, params string[] ignoreList)
        {
            Map(source, source.GetType(), target, target.GetType(), suppressExceptions, ignoreList);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="target">target</param>
        /// <param name="targetParentType">targetParentType</param>
        /// <param name="ignoreList">ignoreList</param>
        public static void Map(object source, object target, Type targetParentType, params string[] ignoreList)
        {
            Map(source, source.GetType(), target, targetParentType, false, ignoreList);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="sourceParentType">sourceParentType</param>
        /// <param name="target">target</param>
        /// <param name="targetParentType">targetParentType</param>
        public static void Map(object source, Type sourceParentType, object target, Type targetParentType)
        {
            Map(source, sourceParentType, target, targetParentType, false, new string[0]);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="sourceParentType">sourceParentType</param>
        /// <param name="target">target</param>
        /// <param name="ignoreList">ignoreList</param>
        public static void Map(object source, Type sourceParentType, object target, params string[] ignoreList)
        {
            Map(source, sourceParentType, target, target.GetType(), false, ignoreList);
        }
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="sourceParentType">sourceParentType</param>
        /// <param name="target">target</param>
        /// <param name="targetParentType">targetParentType</param>
        /// <param name="suppressExceptions">suppressExceptions</param>
        /// <param name="ignoreList">ignoreList</param>
        public static void Map(object source, Type sourceParentType, object target, Type targetParentType, bool suppressExceptions, params string[] ignoreList)
        {
            if (source == null)
            {
                throw new ArgumentException("参数:'source'不能为 Null");
            }
            if (target == null)
            {
                throw new ArgumentException("参数:'target'不能为 Null");
            }
            List<string> list = new List<string>();
            foreach (string str in ignoreList)
            {
                string item = getCoreName(str);
                list.Add(item);
            }
            if (sourceParentType == null)
            {
                sourceParentType = source.GetType();
            }
            if (targetParentType == null)
            {
                targetParentType = target.GetType();
            }
            Dictionary<string, IMember> dictionary = getMemberListLimitToParentType(source.GetType(), sourceParentType, suppressExceptions);
            Dictionary<string, IMember> dictionary2 = getMemberListLimitToParentType(target.GetType(), targetParentType, suppressExceptions);
            foreach (IMember member in dictionary.Values)
            {
                if (member.CanRead)
                {
                    string str3 = getCoreName(member.Name);
                    if (!list.Contains(str3) && dictionary2.ContainsKey(str3))
                    {
                        IMember targetMember = dictionary2[str3];
                        if (targetMember.CanWrite)
                        {
                            object obj2 = member.GetValue(source);
                            try
                            {
                                setMemberValue(target, targetMember, obj2);
                                continue;
                            }
                            catch (System.Exception exception)
                            {
                                if (!suppressExceptions)
                                {
                                    throw new ArgumentException(string.Format("{0} ({1})", Resources.FieldCopyFailed, member.Name), exception);
                                }
                                continue;
                            }
                        }
                    }
                }
            }
        }

    }
}
