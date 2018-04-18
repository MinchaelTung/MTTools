using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// TableInfo
    /// </summary>
    internal class TableInfo
    {
        private const string CombinedTableDuplicationColumnName = "有相同的 ColumnName.(Column:{0} Type1:{1} Type2:{2})";
        private const string CombinedTableChildListAttributeDuplicationInOneType = "子类集合标记类型不符合.(Type:{0} Item Type:{1} )";

        private Dictionary<string, ColumnInfo> columnInfoList = new Dictionary<string, ColumnInfo>();
        private string name = string.Empty;
        private Dictionary<Type, ObjectInfo> objectInfoPool = new Dictionary<Type, ObjectInfo>();
        private Dictionary<Type, List<Type>> relationPool = new Dictionary<Type, List<Type>>();
        private Type topLevelObjectType;
        /// <summary>
        /// addColumnInfo
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
        /// buildColumnInfo
        /// </summary>
        /// <param name="fieldInfo">fieldInfo</param>
        /// <param name="columnType">columnType</param>
        /// <returns></returns>
        private static ColumnInfo buildColumnInfo(FieldInfo fieldInfo, ColumnType columnType)
        {
            ColumnInfo info = new ColumnInfo();
            info.Name = fieldInfo.ColumnName;
            info.ColumnType = columnType;
            return info;
        }
        /// <summary>
        /// buildColumnList
        /// </summary>
        private void buildColumnList()
        {
            foreach (ObjectInfo info in this.ObjectInfoPool.Values)
            {
                ColumnInfo columnInfo = buildColumnInfo(info.IdInfo, ColumnType.ID);
                columnInfo.ObjectType = info.Type;
                this.addColumnInfo(columnInfo);
                foreach (FieldInfo info3 in info.FieldInfoList.Values)
                {
                    columnInfo = buildColumnInfo(info3, ColumnType.Field);
                    columnInfo.ObjectType = info.Type;
                    this.addColumnInfo(columnInfo);
                }
            }
        }
        /// <summary>
        /// GetTableInfo
        /// </summary>
        /// <param name="topLevelObjectType">topLevelObjectType</param>
        /// <param name="tableName">tableName</param>
        /// <returns></returns>
        internal static TableInfo GetTableInfo(Type topLevelObjectType, string tableName)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.name = tableName;
            tableInfo.topLevelObjectType = topLevelObjectType;
            setObjectInfoToTableInfo(tableInfo, topLevelObjectType);
            tableInfo.buildColumnList();
            return tableInfo;
        }
        /// <summary>
        /// setObjectInfoToTableInfo
        /// </summary>
        /// <param name="tableInfo">tableInfo</param>
        /// <param name="type">type</param>
        private static void setObjectInfoToTableInfo(TableInfo tableInfo, Type type)
        {
            ObjectInfo objectInfo = LoadAttributeUtil.GetObjectInfo(type, tableInfo.name);
            tableInfo.ObjectInfoPool.Add(type, objectInfo);
            foreach (ChildListInfo info2 in objectInfo.ChildListInfoList)
            {
                tableInfo.setRelation(type, info2.ItemType);
                setObjectInfoToTableInfo(tableInfo, info2.ItemType);
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
                throw new ORMException(string.Format(CombinedTableChildListAttributeDuplicationInOneType, this.name, child));
            }
            list.Add(child);
        }
        /// <summary>
        /// ColumnInfoList
        /// </summary>
        internal Dictionary<string, ColumnInfo> ColumnInfoList
        {
            get
            {
                return this.columnInfoList;
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
        /// ObjectInfoPool
        /// </summary>
        internal Dictionary<Type, ObjectInfo> ObjectInfoPool
        {
            get
            {
                return this.objectInfoPool;
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
            set
            {
                this.relationPool = value;
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
