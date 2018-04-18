using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// MultiTablesInfo
    /// </summary>
    internal class MultiTablesInfo
    {
        private const string MultiTablesClassAttributeMissParentColumnName = "对象不是最高级别的对象，所以必须设置的parentColumnName.(type:{0})";
        private const string MultiTablesChildListAttributeDuplicationInOneType = "MultiTables_ChildListAttribute 中有相同.(Type:{0} ItemType:{1} )";

        private Dictionary<Type, ClassInfo> classInfoPool = new Dictionary<Type, ClassInfo>();
        private string name = string.Empty;
        private Dictionary<Type, List<Type>> relationPool = new Dictionary<Type, List<Type>>();
        private Type topLevelObjectType;
        /// <summary>
        /// checkRelation
        /// </summary>
        /// <param name="mutilTablesInfo">mutilTablesInfo</param>
        private static void checkRelation(MultiTablesInfo mutilTablesInfo)
        {
            checkRelationWithType(mutilTablesInfo, mutilTablesInfo.TopLevelObjectType);
        }
        /// <summary>
        /// checkRelationWithType
        /// </summary>
        /// <param name="mutilTablesInfo">mutilTablesInfo</param>
        /// <param name="type">type</param>
        private static void checkRelationWithType(MultiTablesInfo mutilTablesInfo, Type type)
        {
            foreach (ChildListInfo info in mutilTablesInfo.ClassInfoPool[type].ChildListInfoList)
            {
                if (mutilTablesInfo.ClassInfoPool[info.ItemType].ParentColumnName == null)
                {
                    throw new ORMException(string.Format(MultiTablesClassAttributeMissParentColumnName, info.ItemType));
                }
                checkRelationWithType(mutilTablesInfo, info.ItemType);
            }
        }
        /// <summary>
        /// GetMutilTablesInfo
        /// </summary>
        /// <param name="topLevelObjectType">topLevelObjectType</param>
        /// <param name="tableName">tableName</param>
        /// <returns>MultiTablesInfo</returns>
        internal static MultiTablesInfo GetMutilTablesInfo(Type topLevelObjectType, string tableName)
        {
            MultiTablesInfo mutilTablesInfo = new MultiTablesInfo();
            mutilTablesInfo.name = tableName;
            mutilTablesInfo.topLevelObjectType = topLevelObjectType;
            setClassInfoToTableInfo(mutilTablesInfo, topLevelObjectType);
            checkRelation(mutilTablesInfo);
            return mutilTablesInfo;
        }
        /// <summary>
        /// setClassInfoToTableInfo
        /// </summary>
        /// <param name="mutilTablesInfo">mutilTablesInfo</param>
        /// <param name="type">type</param>
        private static void setClassInfoToTableInfo(MultiTablesInfo mutilTablesInfo, Type type)
        {
            if (!mutilTablesInfo.ClassInfoPool.ContainsKey(type))
            {
                ClassInfo classInfo = LoadAttributeUtil.GetClassInfo(type, mutilTablesInfo.name);
                mutilTablesInfo.ClassInfoPool.Add(type, classInfo);
                foreach (ChildListInfo info2 in classInfo.ChildListInfoList)
                {
                    mutilTablesInfo.setRelation(type, info2.ItemType);
                    setClassInfoToTableInfo(mutilTablesInfo, info2.ItemType);
                }
            }
        }
        /// <summary>
        /// setRelation
        /// </summary>
        /// <param name="parent">parent</param>
        /// <param name="child">child</param>
        private void setRelation(Type parent, Type child)
        {
            List<Type> list;
            if (this.relationPool.ContainsKey(parent))
            {
                list = this.relationPool[parent];
            }
            else
            {
                list = new List<Type>();
                this.relationPool.Add(parent, list);
            }
            if (list.Contains(child))
            {
                throw new ORMException(string.Format(MultiTablesChildListAttributeDuplicationInOneType, this.name, child));
            }
            list.Add(child);
        }
        /// <summary>
        /// ClassInfoPool
        /// </summary>
        internal Dictionary<Type, ClassInfo> ClassInfoPool
        {
            get
            {
                return this.classInfoPool;
            }
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// RelationPool
        /// </summary>
        public Dictionary<Type, List<Type>> RelationPool
        {
            get
            {
                return this.relationPool;
            }
        }
        /// <summary>
        /// TopLevelObjectType
        /// </summary>
        public Type TopLevelObjectType
        {
            get
            {
                return this.topLevelObjectType;
            }
        }
    }
}
