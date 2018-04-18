using System;
using System.Collections.Generic;
using MTFramework.SimpleDataProxy.ORM.Attributes;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {
        private const string CombinedTableDuplicationColumnName = "有相同的 ColumnName.(Column:{0} Type1:{1} Type2:{2})";
        private const string CombinedTableChildListAttributeDuplicationInOneType = "子类集合标记类型不符合.(Type:{0} Item Type:{1} )";

        private Dictionary<string, ColumnInfo> columnInfoList = new Dictionary<string, ColumnInfo>();
        private string name = string.Empty;
        private Dictionary<Type, ObjectInfo> objectInfoPool = new Dictionary<Type, ObjectInfo>();
        private Dictionary<Type, List<Type>> relationPool = new Dictionary<Type, List<Type>>();
        private Type topLevelObjectType;

        /// <summary>
        /// 添加列信息
        /// </summary>
        /// <param name="columnInfo">columnInfo</param>
        private void addColumnInfo(ColumnInfo columnInfo)
        {
            if (this.ColumnInfoList.ContainsKey(columnInfo.Name))
            {
                throw new ORMException(string.Format(CombinedTableDuplicationColumnName, columnInfo.Name, this.columnInfoList[columnInfo.Name].ObjectType, columnInfo.ObjectType));
            }
            this.ColumnInfoList.Add(columnInfo.Name, columnInfo);
        }

        /// <summary>
        /// 生成列信息
        /// </summary>
        /// <param name="fieldInfo">fieldInfo</param>
        /// <param name="columnType">columnType</param>
        /// <returns>ColumnInfo</returns>
        private static ColumnInfo buildColumnInfo(FieldInfo fieldInfo, ColumnType columnType)
        {
            ColumnInfo info = new ColumnInfo();
            info.Name = fieldInfo.ColumnName;
            info.ColumnType = columnType;
            return info;
        }

        /// <summary>
        /// 生成列信息集合
        /// </summary>
        private void buildColumnList()
        {
            foreach (ObjectInfo info in this.ObjectInfoPool.Values)
            {
                if (info.IdInfo != null)
                {
                    ColumnInfo columnInfo = buildColumnInfo(info.IdInfo, ColumnType.ID);
                    columnInfo.ObjectType = info.Type;
                    this.addColumnInfo(columnInfo);
                }
                foreach (FieldInfo info3 in info.FieldInfoList.Values)
                {
                    ColumnInfo info4 = buildColumnInfo(info3, ColumnType.Field);
                    info4.ObjectType = info.Type;
                    this.addColumnInfo(info4);
                }
            }
        }

        /// <summary>
        /// 获取存储过程信息
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="accessType">accessType</param>
        /// <returns>AccessInfo</returns>
        public AccessInfo GetAccessInfo(Type type, AccessType accessType)
        {
            if (this.ObjectInfoPool.ContainsKey(type))
            {
                ObjectInfo info = this.ObjectInfoPool[type];
                foreach (AccessInfo info2 in info.AccessInfoList)
                {
                    if (info2.Accesstype == accessType)
                    {
                        return info2;
                    }
                }
            }
            throw new ORMException("沒有找到指定Procedure");
        }

        /// <summary>
        /// 获取存储过程信息
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="procedureName">procedureName</param>
        /// <returns>AccessInfo</returns>
        public AccessInfo GetAccessInfo(Type type, string procedureName)
        {
            if (this.ObjectInfoPool.ContainsKey(type))
            {
                ObjectInfo info = this.ObjectInfoPool[type];
                foreach (AccessInfo info2 in info.AccessInfoList)
                {
                    if (info2.Name == procedureName)
                    {
                        return info2;
                    }
                }
            }
            throw new ORMException("沒有找到指定Procedure");
        }

        /// <summary>
        /// 获取存储过程信息
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="accessType">accessType</param>
        /// <param name="procedureName">procedureName</param>
        /// <returns>AccessInfo</returns>
        public AccessInfo GetAccessInfo(Type type, AccessType accessType, string procedureName)
        {
            if (string.IsNullOrEmpty(procedureName))
            {
                return this.GetAccessInfo(type, accessType);
            }
            if (this.ObjectInfoPool.ContainsKey(type))
            {
                ObjectInfo info = this.ObjectInfoPool[type];
                foreach (AccessInfo info2 in info.AccessInfoList)
                {
                    if ((info2.Name == procedureName) && (info2.Accesstype == accessType))
                    {
                        return info2;
                    }
                }
            }
            throw new ORMException("沒有找到指定Procedure");
        }

        /// <summary>
        /// 获取对象信息
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>ObjectInfo</returns>
        public ObjectInfo GetObjectInfo(Type type)
        {
            if (this.ObjectInfoPool.ContainsKey(type))
            {
                return this.ObjectInfoPool[type];
            }
            return null;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="topLevelObjectType">topLevelObjectType</param>
        /// <returns>TableInfo</returns>
        public static TableInfo GetTableInfo(Type topLevelObjectType)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.topLevelObjectType = topLevelObjectType;
            setObjectInfoToTableInfo(tableInfo, topLevelObjectType);
            tableInfo.buildColumnList();
            return tableInfo;
        }

        /// <summary>
        /// 设置对象信息和表信息之间关系
        /// </summary>
        /// <param name="tableInfo">tableInfo</param>
        /// <param name="type">type</param>
        private static void setObjectInfoToTableInfo(TableInfo tableInfo, Type type)
        {
            ObjectInfo objectInfo = LoadAttributeUtil.GetObjectInfo(type);
            tableInfo.ObjectInfoPool.Add(type, objectInfo);
            foreach (ChildListInfo info2 in objectInfo.ChildListInfoList)
            {
                tableInfo.setRelation(type, info2.ItemType);
                setObjectInfoToTableInfo(tableInfo, info2.ItemType);
            }
        }

        /// <summary>
        /// 设置关系
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
                throw new ORMException(string.Format(CombinedTableChildListAttributeDuplicationInOneType, this.name, child));
            }
            list.Add(child);
        }

        /// <summary>
        /// 列集合信息
        /// </summary>
        public Dictionary<string, ColumnInfo> ColumnInfoList
        {
            get
            {
                return this.columnInfoList;
            }
        }

        /// <summary>
        /// 表名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// 对象信息集合
        /// </summary>
        public Dictionary<Type, ObjectInfo> ObjectInfoPool
        {
            get
            {
                return this.objectInfoPool;
            }
        }

        /// <summary>
        /// 关系信息集合
        /// </summary>
        public Dictionary<Type, List<Type>> RelationPool
        {
            get
            {
                return this.relationPool;
            }
            set
            {
                this.relationPool = value;
            }
        }

        /// <summary>
        /// 最高级别对象类型
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
