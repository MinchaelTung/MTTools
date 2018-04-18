using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MTFramework.Utils.ReflectionUtil.Properties;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 反射辅佐工具
    /// </summary>
    public static class ReflectionUtilities
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldInfo[] GetAllFieldInfo(Type type)
        {
            return GetFieldsLimitToParentType(type, typeof(object), true);
        }

        /// <summary>
        /// 方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodInfo[] GetAllMethodInfo(Type type)
        {
            return GetMethodsLimitToParentType(type, typeof(object), true);
        }

        /// <summary>
        /// GetAllPropertyInfo
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetAllPropertyInfo(Type type)
        {
            return GetPropertiesLimitToParentType(type, typeof(object), true);
        }

        /// <summary>
        /// GetChildItemType
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public static Type GetChildItemType(Type listType)
        {
            if (listType == null)
            {
                throw new ArgumentException("参数:'listType'不能为 Null");
            }
            Type propertyType = null;
            if (listType.IsArray)
            {
                return listType.GetElementType();
            }
            DefaultMemberAttribute customAttribute = (DefaultMemberAttribute)Attribute.GetCustomAttribute(listType, typeof(DefaultMemberAttribute));
            if (customAttribute != null)
            {
                foreach (PropertyInfo info in listType.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.Name == customAttribute.MemberName)
                    {
                        propertyType = GetPropertyType(info.PropertyType);
                    }
                }
            }
            return propertyType;
        }

        /// <summary>
        /// GetFieldInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            for (Type type2 = type; type2 != null; type2 = type2.BaseType)
            {
                foreach (FieldInfo info in type2.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.Name == fieldName)
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// GetFieldList
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Field[] GetFieldList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            List<Field> list = new List<Field>();
            foreach (FieldInfo info in GetAllFieldInfo(type))
            {
                Field item = MemberFactory.GetField(info);
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetFieldsLimitLessToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFieldsLimitLessToParentType(Type type, Type parentType)
        {
            return GetFieldsLimitLessToParentType(type, parentType, false);
        }

        /// <summary>
        /// GetFieldsLimitLessToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <param name="suppressExceptions"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFieldsLimitLessToParentType(Type type, Type parentType, bool suppressExceptions)
        {
            FieldInfo[] infoArray2;
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (parentType == null)
            {
                throw new ArgumentException("参数:'parentType'不能为 Null");
            }
            if ((type != parentType) && !type.IsSubclassOf(parentType))
            {
                if (!suppressExceptions)
                {
                    throw new ArgumentException(string.Format(Resources.ParentTypeError, type, parentType));
                }
                return null;
            }
            List<FieldInfo> list = new List<FieldInfo>();
            List<string> list2 = new List<string>();
            List<Type> list3 = getTypeListLimitLessToParent(type, parentType);
        Label_0054:
            infoArray2 = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infoArray2.Length; i++)
            {
                FieldInfo item = infoArray2[i];
                if (list3.Contains(item.DeclaringType) && !list2.Contains(item.Name))
                {
                    list.Add(item);
                    list2.Add(item.Name);
                }
            }
            if (type != parentType)
            {
                type = type.BaseType;
                if (type != null)
                {
                    goto Label_0054;
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetFieldsLimitToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFieldsLimitToParentType(Type type, Type parentType)
        {
            return GetFieldsLimitToParentType(type, parentType, false);
        }

        /// <summary>
        /// GetFieldsLimitToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <param name="suppressExceptions"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFieldsLimitToParentType(Type type, Type parentType, bool suppressExceptions)
        {
            FieldInfo[] infoArray2;
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (parentType == null)
            {
                throw new ArgumentException("参数:'parentType'不能为 Null");
            }
            if ((type != parentType) && !type.IsSubclassOf(parentType))
            {
                if (!suppressExceptions)
                {
                    throw new ArgumentException(string.Format(Resources.ParentTypeError, type, parentType));
                }
                return null;
            }
            List<FieldInfo> list = new List<FieldInfo>();
            List<string> list2 = new List<string>();
            List<Type> list3 = getTypeListLimitToParent(type, parentType);
        Label_0054:
            infoArray2 = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infoArray2.Length; i++)
            {
                FieldInfo item = infoArray2[i];
                if (list3.Contains(item.DeclaringType) && !list2.Contains(item.Name))
                {
                    list.Add(item);
                    list2.Add(item.Name);
                }
            }
            if (type != parentType)
            {
                type = type.BaseType;
                if (type != null)
                {
                    goto Label_0054;
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetMember
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static IMember GetMember(Type type, string memberName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (memberName == null)
            {
                throw new ArgumentException("参数:'memberName'不能为 Null");
            }
            FieldInfo fieldInfo = GetFieldInfo(type, memberName);
            if (fieldInfo != null)
            {
                return MemberFactory.GetField(fieldInfo);
            }
            PropertyInfo propertyInfo = GetPropertyInfo(type, memberName);
            if (propertyInfo != null)
            {
                return MemberFactory.GetProperty(propertyInfo);
            }
            return null;
        }

        /// <summary>
        /// GetMemberList
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<IMember> GetMemberList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            List<IMember> list = new List<IMember>();
            list.AddRange(GetFieldList(type));
            list.AddRange(GetPropertyList(type));
            return list;
        }

        /// <summary>
        /// GetMethodInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            for (Type type2 = type; type2 != null; type2 = type2.BaseType)
            {
                foreach (MethodInfo info in type2.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    if ((info.Name == methodName) && (info.GetParameters().Length == 0))
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// GetMethodInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameterTypeList"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Type type, string methodName, Type[] parameterTypeList)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            for (Type type2 = type; type2 != null; type2 = type2.BaseType)
            {
                foreach (MethodInfo info in type2.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.Name == methodName)
                    {
                        ParameterInfo[] parameters = info.GetParameters();
                        if (parameters.Length == parameterTypeList.Length)
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (((parameterTypeList[i] != parameters[i].ParameterType) && !parameterTypeList[i].IsSubclassOf(parameters[i].ParameterType)) && !IsImplementFromInterface(parameterTypeList[i], parameters[i].ParameterType))
                                {
                                    goto Label_0097;
                                }
                            }
                            return info;
                        Label_0097: ;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// GetMethodInfoWithoutParameter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodInfoWithoutParameter(Type type, string methodName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            Type baseType = type;
            List<MethodInfo> list = new List<MethodInfo>();
            while (baseType != null)
            {
                foreach (MethodInfo info in baseType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.Name == methodName)
                    {
                        list.Add(info);
                    }
                }
                baseType = baseType.BaseType;
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetMethodsLimitToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodsLimitToParentType(Type type, Type parentType)
        {
            return GetMethodsLimitToParentType(type, parentType, false);
        }

        /// <summary>
        /// GetMethodsLimitToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <param name="suppressExceptions"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodsLimitToParentType(Type type, Type parentType, bool suppressExceptions)
        {
            MethodInfo[] infoArray2;
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (parentType == null)
            {
                throw new ArgumentException("参数:'parentType'不能为 Null");
            }
            if ((type != parentType) && !type.IsSubclassOf(parentType))
            {
                if (!suppressExceptions)
                {
                    throw new ArgumentException(string.Format(Resources.ParentTypeError, type, parentType));
                }
                return null;
            }
            List<MethodInfo> list = new List<MethodInfo>();
            List<string> list2 = new List<string>();
        Label_004C:
            infoArray2 = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infoArray2.Length; i++)
            {
                MethodInfo item = infoArray2[i];
                StringBuilder builder = new StringBuilder();
                builder.Append(item.Name);
                foreach (ParameterInfo info2 in item.GetParameters())
                {
                    builder.Append(string.Format("/{0}", info2.ParameterType));
                }
                string str = builder.ToString();
                if (!list2.Contains(str))
                {
                    list.Add(item);
                    list2.Add(str);
                }
            }
            if (type != parentType)
            {
                type = type.BaseType;
                if (type != null)
                {
                    goto Label_004C;
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetPropertiesLimitLessToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesLimitLessToParentType(Type type, Type parentType)
        {
            return GetPropertiesLimitLessToParentType(type, parentType, false);
        }

        /// <summary>
        /// GetPropertiesLimitLessToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <param name="suppressExceptions"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesLimitLessToParentType(Type type, Type parentType, bool suppressExceptions)
        {
            PropertyInfo[] infoArray2;
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (parentType == null)
            {
                throw new ArgumentException("参数:'parentType'不能为 Null");
            }
            if ((type != parentType) && !type.IsSubclassOf(parentType))
            {
                if (!suppressExceptions)
                {
                    throw new ArgumentException(string.Format(Resources.ParentTypeError, type, parentType));
                }
                return null;
            }
            List<PropertyInfo> list = new List<PropertyInfo>();
            List<string> list2 = new List<string>();
            List<Type> list3 = getTypeListLimitLessToParent(type, parentType);
        Label_0054:
            infoArray2 = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infoArray2.Length; i++)
            {
                PropertyInfo item = infoArray2[i];
                if (list3.Contains(item.DeclaringType) && !list2.Contains(item.Name))
                {
                    list.Add(item);
                    list2.Add(item.Name);
                }
            }
            if (type != parentType)
            {
                type = type.BaseType;
                if (type != null)
                {
                    goto Label_0054;
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetPropertiesLimitToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesLimitToParentType(Type type, Type parentType)
        {
            return GetPropertiesLimitToParentType(type, parentType, false);
        }

        /// <summary>
        /// GetPropertiesLimitToParentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <param name="suppressExceptions"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesLimitToParentType(Type type, Type parentType, bool suppressExceptions)
        {
            PropertyInfo[] infoArray2;
            if (type == null)
            {
                throw new ArgumentException(Resources.ArgumentTypeIsNullError);
            }
            if (parentType == null)
            {
                throw new ArgumentException("参数:'parentType'不能为 Null");
            }
            if ((type != parentType) && !type.IsSubclassOf(parentType))
            {
                if (!suppressExceptions)
                {
                    throw new ArgumentException(string.Format(Resources.ParentTypeError, type, parentType));
                }
                return null;
            }
            List<PropertyInfo> list = new List<PropertyInfo>();
            List<string> list2 = new List<string>();
            List<Type> list3 = getTypeListLimitToParent(type, parentType);
        Label_0054:
            infoArray2 = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infoArray2.Length; i++)
            {
                PropertyInfo item = infoArray2[i];
                if (list3.Contains(item.DeclaringType) && !list2.Contains(item.Name))
                {
                    list.Add(item);
                    list2.Add(item.Name);
                }
            }
            if (type != parentType)
            {
                type = type.BaseType;
                if (type != null)
                {
                    goto Label_0054;
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetPropertyInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            for (Type type2 = type; type2 != null; type2 = type2.BaseType)
            {
                foreach (PropertyInfo info in type2.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.Name == propertyName)
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// GetPropertyList
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Property[] GetPropertyList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            List<Property> list = new List<Property>();
            foreach (PropertyInfo info in GetAllPropertyInfo(type))
            {
                Property item = MemberFactory.GetProperty(info);
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// GetPropertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static Type GetPropertyType(Type propertyType)
        {
            if (propertyType == null)
            {
                throw new ArgumentException("参数:'propertyType'不能为 Null");
            }
            Type nullableType = propertyType;
            if (nullableType.IsGenericType && (nullableType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                return Nullable.GetUnderlyingType(nullableType);
            }
            return nullableType;
        }

        /// <summary>
        /// getTypeListLimitLessToParent
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        private static List<Type> getTypeListLimitLessToParent(Type type, Type parentType)
        {
            List<Type> list = new List<Type>();
            for (Type type2 = type; (type2 != null) && type2.IsSubclassOf(parentType); type2 = type2.BaseType)
            {
                list.Add(type2);
            }
            return list;
        }

        /// <summary>
        /// getTypeListLimitToParent
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        private static List<Type> getTypeListLimitToParent(Type type, Type parentType)
        {
            List<Type> list = new List<Type>();
            for (Type type2 = type; (type2 != null) && (type2.IsSubclassOf(parentType) || (type2 == parentType)); type2 = type2.BaseType)
            {
                list.Add(type2);
            }
            return list;
        }

        /// <summary>
        /// IsImplementFromInterface
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool IsImplementFromInterface(Type objectType, Type interfaceType)
        {
            foreach (Type type in objectType.GetInterfaces())
            {
                if (type == interfaceType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
