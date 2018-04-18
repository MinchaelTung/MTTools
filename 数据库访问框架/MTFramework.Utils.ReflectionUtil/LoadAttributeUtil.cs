using System;
using System.Reflection;

namespace MTFramework.Utils.ReflectionUtil
{
    internal static class LoadAttributeUtil
    {
        internal static ClassInfo GetClassInfo(Type dataType)
        {
            ClassInfo classInfo = new ClassInfo();
            classInfo.Type = dataType;
            foreach (object obj2 in dataType.GetCustomAttributes(true))
            {
                if (obj2 is Attribute_ClassAttribute)
                {
                    Attribute_ClassAttribute attribute = (Attribute_ClassAttribute)obj2;
                    classInfo.AttributeType = attribute.AttributeType;
                    classInfo.RestrictPropertyName = attribute.RestrictPropertyName;
                    if (!string.IsNullOrEmpty(classInfo.RestrictPropertyName))
                    {
                        classInfo.RestrictPropertyMember = MemberFactory.GetMember(classInfo.AttributeType, classInfo.RestrictPropertyName);
                    }
                    classInfo.HasAttribute = true;
                    if (attribute.ParentType == null)
                    {
                        classInfo.ParentType = classInfo.Type;
                    }
                    else
                    {
                        classInfo.ParentType = attribute.ParentType;
                    }
                }
                else if (obj2 is Attribute_ClassMemberAttribute)
                {
                    loadClassMemberInfo((Attribute_ClassMemberAttribute)obj2, classInfo);
                }
            }
            foreach (FieldInfo info2 in ReflectionUtilities.GetAllFieldInfo(dataType))
            {
                foreach (Attribute attribute2 in info2.GetCustomAttributes(false))
                {
                    if (attribute2 is Attribute_ItemAttribute)
                    {
                        loadItemInfo((Attribute_ItemAttribute)attribute2, classInfo, info2);
                    }
                    else if (attribute2 is Attribute_ListAttribute)
                    {
                        loadListInfo((Attribute_ListAttribute)attribute2, classInfo, info2);
                    }
                    else if (attribute2 is Attribute_MethodAttribute)
                    {
                        loadMethodInfo((Attribute_MethodAttribute)attribute2, classInfo, info2);
                    }
                    else if (attribute2 is Attribute_MethodListAttribute)
                    {
                        loadMethodListInfo((Attribute_MethodListAttribute)attribute2, classInfo, info2);
                    }
                }
            }
            foreach (PropertyInfo info3 in ReflectionUtilities.GetAllPropertyInfo(dataType))
            {
                foreach (Attribute attribute3 in info3.GetCustomAttributes(false))
                {
                    if (attribute3 is Attribute_ItemAttribute)
                    {
                        loadItemInfo((Attribute_ItemAttribute)attribute3, classInfo, info3);
                    }
                    else if (attribute3 is Attribute_ListAttribute)
                    {
                        loadListInfo((Attribute_ListAttribute)attribute3, classInfo, info3);
                    }
                    else if (attribute3 is Attribute_MethodAttribute)
                    {
                        loadMethodInfo((Attribute_MethodAttribute)attribute3, classInfo, info3);
                    }
                    else if (attribute3 is Attribute_MethodListAttribute)
                    {
                        loadMethodListInfo((Attribute_MethodListAttribute)attribute3, classInfo, info3);
                    }
                }
            }
            return classInfo;
        }

        private static void loadClassMemberInfo(Attribute_ClassMemberAttribute attribute, ClassInfo classInfo)
        {
            ClassMemberInfo item = new ClassMemberInfo();
            item.AttributeType = attribute.AttributeType;
            item.ParentType = attribute.ParentType;
            item.MemberName = attribute.MemberName;
            item.MapType = attribute.MapType;
            classInfo.ClassMemberList.Add(item);
        }

        private static void loadItemInfo(Attribute_ItemAttribute attribute, ClassInfo classInfo, FieldInfo fi)
        {
            ItemInfo item = new ItemInfo();
            item.Member = MemberFactory.GetField(fi);
            item.MemberPropertyName = attribute.MemberPropertyName;
            if (item.MemberPropertyName != null)
            {
                item.MemberPropertyMember = MemberFactory.GetMember(fi.FieldType, item.MemberPropertyName);
            }
            classInfo.ItemInfoList.Add(item);
        }

        private static void loadItemInfo(Attribute_ItemAttribute attribute, ClassInfo classInfo, PropertyInfo pi)
        {
            ItemInfo item = new ItemInfo();
            item.Member = MemberFactory.GetProperty(pi);
            item.MemberPropertyName = attribute.MemberPropertyName;
            if (item.MemberPropertyName != null)
            {
                item.MemberPropertyMember = MemberFactory.GetMember(pi.PropertyType, item.MemberPropertyName);
            }
            classInfo.ItemInfoList.Add(item);
        }

        private static void loadListInfo(Attribute_ListAttribute attribute, ClassInfo classInfo, FieldInfo fi)
        {
            ListInfo item = new ListInfo();
            item.KeyPropertyName = attribute.KeyPropertyName;
            item.ItemType = ReflectionUtilities.GetChildItemType(fi.FieldType);
            if (item.KeyPropertyName != null)
            {
                item.KeyProperty = MemberFactory.GetMember(item.ItemType, item.KeyPropertyName);
            }
            item.Member = MemberFactory.GetField(fi);
            item.MemberPropertyName = attribute.MemberPropertyName;
            if (item.MemberPropertyName != null)
            {
                item.MemberPropertyMember = MemberFactory.GetMember(ReflectionUtilities.GetChildItemType(fi.FieldType), item.MemberPropertyName);
            }
            classInfo.ListInfoList.Add(item);
        }

        private static void loadListInfo(Attribute_ListAttribute attribute, ClassInfo classInfo, PropertyInfo pi)
        {
            ListInfo item = new ListInfo();
            item.KeyPropertyName = attribute.KeyPropertyName;
            item.ItemType = ReflectionUtilities.GetChildItemType(pi.PropertyType);
            if (item.KeyPropertyName != null)
            {
                item.KeyProperty = MemberFactory.GetMember(item.ItemType, item.KeyPropertyName);
            }
            item.Member = MemberFactory.GetProperty(pi);
            item.MemberPropertyName = attribute.MemberPropertyName;
            if (item.MemberPropertyName != null)
            {
                item.MemberPropertyMember = MemberFactory.GetMember(ReflectionUtilities.GetChildItemType(pi.PropertyType), item.MemberPropertyName);
            }
            classInfo.ListInfoList.Add(item);
        }

        private static void loadMethodInfo(Attribute_MethodAttribute attribute, ClassInfo classInfo, FieldInfo fi)
        {
            Method item = new Method();
            item.MethodInfoPropertyName = attribute.MethodInfoPropertyName;
            if (item.MethodInfoPropertyName != null)
            {
                item.MethodInfoPropertyMember = MemberFactory.GetMember(fi.FieldType, item.MethodInfoPropertyName);
            }
            item.Member = MemberFactory.GetField(fi);
            classInfo.MethodList.Add(item);
        }

        private static void loadMethodInfo(Attribute_MethodAttribute attribute, ClassInfo classInfo, PropertyInfo pi)
        {
            Method item = new Method();
            item.MethodInfoPropertyName = attribute.MethodInfoPropertyName;
            if (item.MethodInfoPropertyName != null)
            {
                item.MethodInfoPropertyMember = MemberFactory.GetMember(pi.PropertyType, item.MethodInfoPropertyName);
            }
            item.Member = MemberFactory.GetProperty(pi);
            classInfo.MethodList.Add(item);
        }

        private static void loadMethodListInfo(Attribute_MethodListAttribute attribute, ClassInfo classInfo, FieldInfo fi)
        {
            MethodList item = new MethodList();
            item.MethodInfoPropertyName = attribute.MethodInfoPropertyName;
            if (item.MethodInfoPropertyName != null)
            {
                item.MethodInfoPropertyMember = MemberFactory.GetMember(ReflectionUtilities.GetChildItemType(fi.FieldType), item.MethodInfoPropertyName);
            }
            item.Member = MemberFactory.GetField(fi);
            classInfo.MethodListList.Add(item);
        }

        private static void loadMethodListInfo(Attribute_MethodListAttribute attribute, ClassInfo classInfo, PropertyInfo pi)
        {
            MethodList item = new MethodList();
            item.MethodInfoPropertyName = attribute.MethodInfoPropertyName;
            if (item.MethodInfoPropertyName != null)
            {
                item.MethodInfoPropertyMember = MemberFactory.GetMember(ReflectionUtilities.GetChildItemType(pi.PropertyType), item.MethodInfoPropertyName);
            }
            item.Member = MemberFactory.GetProperty(pi);
            classInfo.MethodListList.Add(item);
        }
    }

}
