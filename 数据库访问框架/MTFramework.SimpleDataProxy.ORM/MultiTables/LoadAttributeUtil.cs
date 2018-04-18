using System;
using System.Reflection;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// LoadAttributeUtil
    /// </summary>
    internal static class LoadAttributeUtil
    {
        private const string MultiTablesFieldAttributeDuplicationInOneType = "MultiTables_FieldAttribute 有相同.(type:{0},columnName:{1})";
        private const string MultiTablesIDAttributeDuplicationInOneType = "(type:{0}) 只能有一个 MutilTables_IDAttribute.";
        private const string MultiTablesParentAttributeDuplicationInOneType = "(Type:{0}) 只能有一个 MultiTables_ParentAttribute.";
        /// <summary>
        /// GetClassInfo
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <returns>ClassInfo</returns>
        internal static ClassInfo GetClassInfo(Type type, string multiTablesName)
        {
            ClassInfo info = (ClassInfo)AttributeLoader.LoadData(type, typeof(ClassInfo), multiTablesName);
            info.Type = type;
            return info;
        }

        /// <summary>
        /// loadChildListInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="fi">System.Reflection.FieldInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadChildListInfo(MultiTables_ChildListAttribute attribute, ClassInfo classInfo, System.Reflection.FieldInfo fi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                ChildListInfo item = new ChildListInfo();
                item.MultiTablesName = multiTablesName;
                item.ItemType = attribute.ItemType;
                item.ItemTableIndex = attribute.ItemTableIndex;
                item.Member = MemberFactory.GetField(fi);
                classInfo.ChildListInfoList.Add(item);
            }
        }
        /// <summary>
        /// loadChildListInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="pi">PropertyInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadChildListInfo(MultiTables_ChildListAttribute attribute, ClassInfo classInfo, PropertyInfo pi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                ChildListInfo item = new ChildListInfo();
                item.MultiTablesName = multiTablesName;
                item.ItemType = attribute.ItemType;
                item.ItemTableIndex = attribute.ItemTableIndex;
                item.Member = MemberFactory.GetProperty(pi);
                classInfo.ChildListInfoList.Add(item);
            }
        }
        /// <summary>
        /// loadFieldInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="fi">System.Reflection.FieldInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadFieldInfo(MultiTables_FieldAttribute attribute, ClassInfo classInfo, System.Reflection.FieldInfo fi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                if (classInfo.FieldInfoList.ContainsKey(attribute.ColumnName))
                {
                    FieldInfo info = classInfo.FieldInfoList[attribute.ColumnName];
                    if (info.Member.ReflectedType == fi.ReflectedType)
                    {
                        throw new ORMException(string.Format(MultiTablesFieldAttributeDuplicationInOneType, fi.ReflectedType, attribute.ColumnName));
                    }
                    if (fi.ReflectedType.IsSubclassOf(classInfo.IdInfo.Member.ReflectedType))
                    {
                        return;
                    }
                    classInfo.FieldInfoList.Remove(attribute.ColumnName);
                }
                FieldInfo info2 = new FieldInfo();
                info2.ColumnName = attribute.ColumnName;
                info2.MultiTablesName = multiTablesName;
                info2.DataType = attribute.DataType;
                info2.Member = MemberFactory.GetField(fi);
                classInfo.FieldInfoList.Add(info2.ColumnName, info2);
            }
        }
        /// <summary>
        /// loadFieldInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="pi">PropertyInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadFieldInfo(MultiTables_FieldAttribute attribute, ClassInfo classInfo, PropertyInfo pi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                if (classInfo.FieldInfoList.ContainsKey(attribute.ColumnName))
                {
                    FieldInfo info = classInfo.FieldInfoList[attribute.ColumnName];
                    if (info.Member.ReflectedType == pi.ReflectedType)
                    {
                        throw new ORMException(string.Format(MultiTablesFieldAttributeDuplicationInOneType, pi.ReflectedType, attribute.ColumnName));
                    }
                    if (pi.ReflectedType.IsSubclassOf(classInfo.IdInfo.Member.ReflectedType))
                    {
                        return;
                    }
                    classInfo.FieldInfoList.Remove(attribute.ColumnName);
                }
                FieldInfo info2 = new FieldInfo();
                info2.ColumnName = attribute.ColumnName;
                info2.MultiTablesName = multiTablesName;
                info2.DataType = attribute.DataType;
                info2.Member = MemberFactory.GetProperty(pi);
                classInfo.FieldInfoList.Add(info2.ColumnName, info2);
            }
        }
        /// <summary>
        /// loadIDInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="fi">System.Reflection.FieldInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadIDInfo(MultiTables_IDAttribute attribute, ClassInfo classInfo, System.Reflection.FieldInfo fi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                if (classInfo.IdInfo != null)
                {
                    if (classInfo.IdInfo.Member.ReflectedType == fi.ReflectedType)
                    {
                        throw new ORMException(string.Format(MultiTablesIDAttributeDuplicationInOneType, fi.ReflectedType));
                    }
                    if (fi.ReflectedType.IsSubclassOf(classInfo.IdInfo.Member.ReflectedType))
                    {
                        return;
                    }
                }
                IDInfo info = new IDInfo();
                info.ColumnName = attribute.ColumnName;
                info.MultiTablesName = multiTablesName;
                info.DataType = attribute.DataType;
                info.Member = MemberFactory.GetField(fi);
                classInfo.IdInfo = info;
            }
        }
        /// <summary>
        /// loadIDInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="pi">PropertyInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadIDInfo(MultiTables_IDAttribute attribute, ClassInfo classInfo, PropertyInfo pi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                if (classInfo.IdInfo != null)
                {
                    if (classInfo.IdInfo.Member.ReflectedType == pi.ReflectedType)
                    {
                        throw new ORMException(string.Format(MultiTablesIDAttributeDuplicationInOneType, pi.ReflectedType));
                    }
                    if (pi.ReflectedType.IsSubclassOf(classInfo.IdInfo.Member.ReflectedType))
                    {
                        return;
                    }
                }
                IDInfo info = new IDInfo();
                info.ColumnName = attribute.ColumnName;
                info.MultiTablesName = multiTablesName;
                info.DataType = attribute.DataType;
                info.Member = MemberFactory.GetProperty(pi);
                classInfo.IdInfo = info;
            }
        }
        /// <summary>
        /// loadParentInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="fi">System.Reflection.FieldInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadParentInfo(MultiTables_ParentAttribute attribute, ClassInfo classInfo, System.Reflection.FieldInfo fi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                if (classInfo.ParentInfo != null)
                {
                    if (classInfo.ParentInfo.Member.ReflectedType == fi.ReflectedType)
                    {
                        throw new ORMException(string.Format(MultiTablesParentAttributeDuplicationInOneType, fi.ReflectedType));
                    }
                    if (fi.ReflectedType.IsSubclassOf(classInfo.ParentInfo.Member.ReflectedType))
                    {
                        return;
                    }
                }
                ParentInfo info = new ParentInfo();
                info.MultiTablesName = multiTablesName;
                info.Member = MemberFactory.GetField(fi);
                classInfo.ParentInfo = info;
            }
        }
        /// <summary>
        /// loadParentInfo
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <param name="classInfo">classInfo</param>
        /// <param name="pi">PropertyInfo</param>
        /// <param name="multiTablesName">multiTablesName</param>
        private static void loadParentInfo(MultiTables_ParentAttribute attribute, ClassInfo classInfo, PropertyInfo pi, string multiTablesName)
        {
            if (attribute.MultiTablesName == multiTablesName)
            {
                if (classInfo.ParentInfo != null)
                {
                    if (classInfo.ParentInfo.Member.ReflectedType == pi.ReflectedType)
                    {
                        throw new ORMException(string.Format(MultiTablesParentAttributeDuplicationInOneType, pi.ReflectedType));
                    }
                    if (pi.ReflectedType.IsSubclassOf(classInfo.ParentInfo.Member.ReflectedType))
                    {
                        return;
                    }
                }
                ParentInfo info = new ParentInfo();
                info.MultiTablesName = multiTablesName;
                info.Member = MemberFactory.GetProperty(pi);
                classInfo.ParentInfo = info;
            }
        }
    }

}
