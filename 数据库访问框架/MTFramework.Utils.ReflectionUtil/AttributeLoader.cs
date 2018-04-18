using System;
using System.Reflection;
using MTFramework.Utils.ReflectionUtil.Properties;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 读入自定义属性信息
    /// </summary>
    public static class AttributeLoader
    {
        /// <summary>
        /// getClassAttribue
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="restrictValue">指定值</param>
        /// <returns>自定义属性</returns>
        private static Attribute getClassAttribue(Type type, ClassInfo classInfo, object restrictValue)
        {
            foreach (Attribute attribute in type.GetCustomAttributes(true))
            {
                if (attribute.GetType() == classInfo.AttributeType)
                {
                    if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                    {
                        object objB = classInfo.RestrictPropertyMember.GetValue(attribute);
                        if (!object.Equals(restrictValue, objB))
                        {
                            goto Label_004A;
                        }
                    }
                    return attribute;
                Label_004A: ;
                }
            }
            return null;
        }
        /// <summary>
        /// loadClassMemberInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="data">data</param>
        /// <param name="memberInfo">memberInfo</param>
        private static void loadClassMemberInfo(Type type, object data, ClassMemberInfo memberInfo)
        {
            Type type2 = data.GetType();
            foreach (Attribute attribute in type.GetCustomAttributes(true))
            {
                Type type3 = attribute.GetType();
                if ((type3 == memberInfo.AttributeType) || type3.IsSubclassOf(memberInfo.AttributeType))
                {
                    IMember member = ReflectionUtilities.GetMember(type2, memberInfo.MemberName);
                    if (member == null)
                    {
                        throw new ReflectionException(string.Format(Resources.NotFoundMemberInObject, memberInfo.MemberName, type2));
                    }
                    object target = null;
                    try
                    {
                        target = MethodCaller.CreateInstance(memberInfo.MapType);
                    }
                    catch (System.Exception exception)
                    {
                        throw new ReflectionException(string.Format(Resources.CreateObjectError, memberInfo.MapType), exception);
                    }
                    ObjectToObjectMapper.Map(attribute, typeof(Attribute), target, memberInfo.MapType);
                    if (member.GetCustomAttributes(typeof(Attribute_ListAttribute), true).Length > 0)
                    {
                        ListReflector.ListAddItem(member.GetValue(data), target);
                    }
                    else
                    {
                        member.SetValue(data, target);
                    }
                }
            }
        }
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="dataType">dataType</param>
        /// <returns>object</returns>
        public static object LoadData(Type type, Type dataType)
        {
            return LoadData(type, dataType, null);
        }
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="dataType">dataType</param>
        /// <param name="restrictValue">指定值</param>
        /// <returns>object</returns>
        public static object LoadData(Type type, Type dataType, object restrictValue)
        {
            if (type == null)
            {
                throw new ArgumentException(Resources.ArgumentTypeIsNullError);
            }
            if (dataType == null)
            {
                throw new ArgumentException(Resources.ArgumentTypeIsNullError);
            }
            ClassInfo classInfo = ClassInfoCache.GetClassInfo(dataType);
            object target = null;
            try
            {
                target = MethodCaller.CreateInstance(classInfo.Type);
            }
            catch (System.Exception exception)
            {
                throw new ReflectionException(string.Format(Resources.CreateObjectError, classInfo.Type), exception);
            }
            if (classInfo.HasAttribute)
            {
                ObjectToObjectMapper.Map(getClassAttribue(type, classInfo, restrictValue), typeof(Attribute), target, classInfo.ParentType);
            }
            foreach (ClassMemberInfo info2 in classInfo.ClassMemberList)
            {
                loadClassMemberInfo(type, target, info2);
            }
            foreach (ItemInfo info3 in classInfo.ItemInfoList)
            {
                loadItemInfo(type, target, info3, restrictValue);
            }
            foreach (ListInfo info4 in classInfo.ListInfoList)
            {
                loadListInfo(type, target, info4, restrictValue);
            }
            foreach (Method method in classInfo.MethodList)
            {
                loadMethodInfo(type, target, method, restrictValue);
            }
            foreach (MethodList list in classInfo.MethodListList)
            {
                loadMethodListInfo(type, target, list, restrictValue);
            }
            return target;
        }
        /// <summary>
        /// loadItemInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="data">data</param>
        /// <param name="itemInfo">itemInfo</param>
        /// <param name="restrictValue">指定值</param>
        private static void loadItemInfo(Type type, object data, ItemInfo itemInfo, object restrictValue)
        {
            ClassInfo classInfo = ClassInfoCache.GetClassInfo(itemInfo.Member.Type);
            foreach (FieldInfo info2 in ReflectionUtilities.GetAllFieldInfo(type))
            {
                foreach (Attribute attribute in info2.GetCustomAttributes(false))
                {
                    if (attribute.GetType() == classInfo.AttributeType)
                    {
                        if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                        {
                            object objB = classInfo.RestrictPropertyMember.GetValue(attribute);
                            if (!object.Equals(restrictValue, objB))
                            {
                                goto Label_00F8;
                            }
                        }
                        object target = null;
                        try
                        {
                            target = MethodCaller.CreateInstance(itemInfo.Member.Type);
                        }
                        catch (System.Exception exception)
                        {
                            throw new ReflectionException(string.Format("创建Object错误（Type:{0})", itemInfo.Member.Type), exception);
                        }
                        ObjectToObjectMapper.Map(attribute, typeof(Attribute), target, typeof(object));
                        if (itemInfo.MemberPropertyMember != null)
                        {
                            itemInfo.MemberPropertyMember.SetValue(target, MemberFactory.GetField(info2));
                        }
                        itemInfo.Member.SetValue(data, target);
                        return;
                    Label_00F8: ;
                    }
                }
            }
            foreach (PropertyInfo info3 in ReflectionUtilities.GetAllPropertyInfo(type))
            {
                foreach (Attribute attribute2 in info3.GetCustomAttributes(false))
                {
                    if (attribute2.GetType() == classInfo.AttributeType)
                    {
                        if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                        {
                            object obj4 = classInfo.RestrictPropertyMember.GetValue(attribute2);
                            if (!object.Equals(restrictValue, obj4))
                            {
                                goto Label_0200;
                            }
                        }
                        object obj5 = null;
                        try
                        {
                            obj5 = MethodCaller.CreateInstance(itemInfo.Member.Type);
                        }
                        catch (System.Exception exception2)
                        {
                            throw new ReflectionException(string.Format("创建Object错误（Type:{0})", itemInfo.Member.Type), exception2);
                        }
                        ObjectToObjectMapper.Map(attribute2, typeof(Attribute), obj5, classInfo.ParentType);
                        if (itemInfo.MemberPropertyMember != null)
                        {
                            itemInfo.MemberPropertyMember.SetValue(obj5, MemberFactory.GetProperty(info3));
                        }
                        itemInfo.Member.SetValue(data, obj5);
                        return;
                    Label_0200: ;
                    }
                }
            }
        }
        /// <summary>
        /// loadListInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="data">data</param>
        /// <param name="listInfo">listInfo</param>
        /// <param name="restrictValue">指定值</param>
        private static void loadListInfo(Type type, object data, ListInfo listInfo, object restrictValue)
        {
            Type childItemType = ReflectionUtilities.GetChildItemType(listInfo.Member.Type);
            ClassInfo classInfo = ClassInfoCache.GetClassInfo(childItemType);
            if (classInfo.AttributeType != null)
            {
                object dictionary = listInfo.Member.GetValue(data);
                foreach (FieldInfo info2 in ReflectionUtilities.GetAllFieldInfo(type))
                {
                    foreach (Attribute attribute in info2.GetCustomAttributes(false))
                    {
                        if (attribute.GetType() == classInfo.AttributeType)
                        {
                            if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                            {
                                object objB = classInfo.RestrictPropertyMember.GetValue(attribute);
                                if (!object.Equals(restrictValue, objB))
                                {
                                    goto Label_0134;
                                }
                            }
                            object target = null;
                            try
                            {
                                target = MethodCaller.CreateInstance(childItemType);
                            }
                            catch (System.Exception exception)
                            {
                                throw new ReflectionException(string.Format("创建Object错误（Type:{0})", childItemType), exception);
                            }
                            ObjectToObjectMapper.Map(attribute, typeof(Attribute), target);
                            if (listInfo.MemberPropertyMember != null)
                            {
                                listInfo.MemberPropertyMember.SetValue(target, MemberFactory.GetField(info2));
                            }
                            if (listInfo.KeyProperty != null)
                            {
                                object key = listInfo.KeyProperty.GetValue(target);
                                if (ListReflector.ContainsKey(dictionary, key))
                                {
                                    throw new ReflectionException(string.Format(Resources.DictionaryKeyDuplication, dictionary, key));
                                }
                                ListReflector.DictionaryAddItem(dictionary, key, target);
                            }
                            else
                            {
                                ListReflector.ListAddItem(dictionary, target);
                            }
                        Label_0134: ;
                        }
                    }
                }
                foreach (PropertyInfo info3 in ReflectionUtilities.GetAllPropertyInfo(type))
                {
                    foreach (Attribute attribute2 in info3.GetCustomAttributes(false))
                    {
                        if (attribute2.GetType() == classInfo.AttributeType)
                        {
                            if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                            {
                                object obj6 = classInfo.RestrictPropertyMember.GetValue(attribute2);
                                if (!object.Equals(restrictValue, obj6))
                                {
                                    goto Label_025F;
                                }
                            }
                            object obj7 = null;
                            try
                            {
                                obj7 = MethodCaller.CreateInstance(childItemType);
                            }
                            catch (System.Exception exception2)
                            {
                                throw new ReflectionException(string.Format(Resources.CreateObjectError, childItemType), exception2);
                            }
                            ObjectToObjectMapper.Map(attribute2, typeof(Attribute), obj7);
                            if (listInfo.MemberPropertyMember != null)
                            {
                                listInfo.MemberPropertyMember.SetValue(obj7, MemberFactory.GetProperty(info3));
                            }
                            if (listInfo.KeyProperty != null)
                            {
                                object obj8 = listInfo.KeyProperty.GetValue(obj7);
                                if (ListReflector.ContainsKey(dictionary, obj8))
                                {
                                    throw new ReflectionException(string.Format(Resources.DictionaryKeyDuplication, dictionary, obj8));
                                }
                                ListReflector.DictionaryAddItem(dictionary, obj8, obj7);
                            }
                            else
                            {
                                ListReflector.ListAddItem(dictionary, obj7);
                            }
                        Label_025F: ;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// loadMethodInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="data">data</param>
        /// <param name="method">method</param>
        /// <param name="restrictValue">指定值</param>
        private static void loadMethodInfo(Type type, object data, Method method, object restrictValue)
        {
            ClassInfo classInfo = ClassInfoCache.GetClassInfo(method.Member.Type);
            foreach (MethodInfo info2 in ReflectionUtilities.GetAllMethodInfo(type))
            {
                foreach (Attribute attribute in info2.GetCustomAttributes(false))
                {
                    if (attribute.GetType() == classInfo.AttributeType)
                    {
                        if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                        {
                            object objB = classInfo.RestrictPropertyMember.GetValue(attribute);
                            if (!object.Equals(restrictValue, objB))
                            {
                                goto Label_00DB;
                            }
                        }
                        object target = null;
                        try
                        {
                            target = MethodCaller.CreateInstance(method.Member.Type);
                        }
                        catch (System.Exception exception)
                        {
                            throw new ReflectionException(string.Format(Resources.CreateObjectError, method.Member.Type), exception);
                        }
                        ObjectToObjectMapper.Map(attribute, typeof(Attribute), target);
                        method.MethodInfoPropertyMember.SetValue(target, info2);
                        method.Member.SetValue(data, target);
                        return;
                    Label_00DB: ;
                    }
                }
            }
        }
        /// <summary>
        /// loadMethodListInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="data">data</param>
        /// <param name="methodList">methodList</param>
        /// <param name="restrictValue">指定值</param>
        private static void loadMethodListInfo(Type type, object data, MethodList methodList, object restrictValue)
        {
            Type childItemType = ReflectionUtilities.GetChildItemType(methodList.Member.Type);
            ClassInfo classInfo = ClassInfoCache.GetClassInfo(childItemType);
            object list = methodList.Member.GetValue(data);
            foreach (MethodInfo info2 in ReflectionUtilities.GetAllMethodInfo(type))
            {
                foreach (Attribute attribute in info2.GetCustomAttributes(false))
                {
                    if (attribute.GetType() == classInfo.AttributeType)
                    {
                        if ((restrictValue != null) && (classInfo.RestrictPropertyMember != null))
                        {
                            object objB = classInfo.RestrictPropertyMember.GetValue(attribute);
                            if (!object.Equals(restrictValue, objB))
                            {
                                goto Label_00D6;
                            }
                        }
                        object target = null;
                        try
                        {
                            target = MethodCaller.CreateInstance(childItemType);
                        }
                        catch (System.Exception exception)
                        {
                            throw new ReflectionException(string.Format(Resources.CreateObjectError, childItemType), exception);
                        }
                        ObjectToObjectMapper.Map(attribute, typeof(Attribute), target);
                        methodList.MethodInfoPropertyMember.SetValue(target, info2);
                        ListReflector.ListAddItem(list, target);
                    Label_00D6: ;
                    }
                }
            }
        }
    }
}
