using System;
using System.Collections.Generic;
using System.Reflection;
using MTFramework.Utils.ReflectionUtil.Properties;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 成员工厂
    /// </summary>
    public static class MemberFactory
    {
        private static Dictionary<FieldInfo, Field> fieldCache = new Dictionary<FieldInfo, Field>();
        private static Dictionary<PropertyInfo, Property> propertyCache = new Dictionary<PropertyInfo, Property>();
        private static object syncObj = new object();
        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="fieldInfo">字段信息</param>
        /// <returns>字段内容</returns>
        public static Field GetField(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentException(Resources.CreateObjectError);
            }
            lock (syncObj)
            {
                if (fieldCache.ContainsKey(fieldInfo))
                {
                    return fieldCache[fieldInfo];
                }
                Field field = new Field(fieldInfo);
                fieldCache.Add(fieldInfo, field);
                return field;
            }
        }
        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyOrFieldName">属性或字段名称</param>
        /// <returns>成员内容接口</returns>
        public static IMember GetMember(Type type, string propertyOrFieldName)
        {
            if (type == null)
            {
                throw new ArgumentException(Resources.ArgumentTypeIsNullError);
            }
            if (propertyOrFieldName == null)
            {
                throw new ArgumentException(Resources.ArgumentPropertyOrFieldNameIsNull);
            }
            FieldInfo fieldInfo = ReflectionUtilities.GetFieldInfo(type, propertyOrFieldName);
            if (fieldInfo != null)
            {
                return GetField(fieldInfo);
            }
            PropertyInfo propertyInfo = ReflectionUtilities.GetPropertyInfo(type, propertyOrFieldName);
            if (propertyInfo == null)
            {
                throw new ReflectionException(string.Format(Resources.KeyPropertyNotFound, type, propertyOrFieldName));
            }
            return GetProperty(propertyInfo);
        }
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns>属性内容</returns>
        public static Property GetProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException(Resources.ArgumentPropertyInfoIsNull);
            }
            lock (syncObj)
            {
                if (propertyCache.ContainsKey(propertyInfo))
                {
                    return propertyCache[propertyInfo];
                }
                Property property = new Property(propertyInfo);
                propertyCache.Add(propertyInfo, property);
                return property;
            }
        }
    }

}
