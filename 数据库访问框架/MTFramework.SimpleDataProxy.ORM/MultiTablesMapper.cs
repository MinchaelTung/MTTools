using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using MTFramework.Utils.ReflectionUtil;
namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// 表反射赋值方法
    /// </summary>
    public class MultiTablesMapper
    {
        private const string CombinedTableAfterMapDataAttributeParameterTooMany = "方法参数太多.(type:{0} 方法名称:{1})";
        private const string ParentObjectMissChildListAttribute = "父并没有定义子对象MultiTables_ChildListAttribute.(type:{0} subType:{1} tableIndex:{2})";
        private const string MultiTablesAbsentID = "(type:{0}) 没有定义 ID.";
        private const string MultiTablesAbsentParentID = "(type:{0}) 没有定义 Parent ID .";
        private const string MultiTablesParentNotFound = "(type:{0} parentID:{1}) 没有parentID.";
        private const string PropertyDataTypeConvertFail = "Property:{0} 数据不能转换成 {1}";

        private Dictionary<string, MTFramework.SimpleDataProxy.ORM.MultiTables.FieldInfo> columnListForUsing;
        private MTFramework.SimpleDataProxy.ORM.MultiTables.MultiTablesInfo multiTablesInfo;
        private ObjectPool objectPool;
        private Type topLevelObjectType;

        /// <summary>
        /// 表反射赋值
        /// </summary>
        /// <param name="multiTablesName">表名称</param>
        /// <param name="topLevelObjectType">最高级别的对象类型</param>
        private MultiTablesMapper(string multiTablesName, Type topLevelObjectType)
        {
            this.topLevelObjectType = topLevelObjectType;
            this.multiTablesInfo = MTFramework.SimpleDataProxy.ORM.MultiTables.MultiTablesInfoCache.GetMultiTablesInfo(topLevelObjectType, multiTablesName);
            this.objectPool = new ObjectPool();
        }

        /// <summary>
        /// afterMapDataInfo
        /// </summary>
        /// <param name="classInfo">classInfo</param>
        /// <param name="dr">SimpleDataRow</param>
        /// <param name="obj">object</param>
        private void afterMapDataInfo(MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, SimpleDataRow dr, object obj)
        {
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.AfterMapDataInfo info in classInfo.AfterMapDataInfoList)
            {
                ParameterInfo[] parameters = info.MethodInfo.GetParameters();
                if (parameters.Length > 1)
                {
                    throw new ORMException(string.Format(CombinedTableAfterMapDataAttributeParameterTooMany, info.MethodInfo.DeclaringType, info.MethodInfo.Name));
                }
                try
                {
                    if (parameters.Length == 0)
                    {
                        info.MethodInfo.Invoke(obj, null);
                    }
                    else if (parameters[0].ParameterType == typeof(SimpleDataRow))
                    {
                        info.MethodInfo.Invoke(obj, new object[] { dr });
                    }
                    continue;
                }
                catch (System.Exception exception)
                {
                    throw new ReflectionException(string.Format("Type:{0}调用{1}方法发生错误", classInfo.Type, info.MethodInfo.Name), exception);
                }
            }
        }

        /// <summary>
        /// afterMapDataInfo
        /// </summary>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="dr">DataRow</param>
        /// <param name="obj">object</param>
        private void afterMapDataInfo(MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, DataRow dr, object obj)
        {
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.AfterMapDataInfo info in classInfo.AfterMapDataInfoList)
            {
                ParameterInfo[] parameters = info.MethodInfo.GetParameters();
                if (parameters.Length > 1)
                {
                    throw new ORMException(string.Format(CombinedTableAfterMapDataAttributeParameterTooMany, info.MethodInfo.DeclaringType, info.MethodInfo.Name));
                }
                try
                {
                    if (parameters.Length == 0)
                    {
                        info.MethodInfo.Invoke(obj, null);
                    }
                    else if (parameters[0].ParameterType == typeof(DataRow))
                    {
                        info.MethodInfo.Invoke(obj, new object[] { dr });
                    }
                    continue;
                }
                catch (System.Exception exception)
                {
                    throw new ReflectionException(string.Format("Type:{0}调用{1}方法发生错误", classInfo.Type, info.MethodInfo.Name), exception);
                }
            }
        }

        /// <summary>
        /// afterMapDataInfo
        /// </summary>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="dr">IDataReader</param>
        /// <param name="obj">object</param>
        private void afterMapDataInfo(MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, IDataReader dr, object obj)
        {
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.AfterMapDataInfo info in classInfo.AfterMapDataInfoList)
            {
                ParameterInfo[] parameters = info.MethodInfo.GetParameters();
                if (parameters.Length > 1)
                {
                    throw new ORMException(string.Format(CombinedTableAfterMapDataAttributeParameterTooMany, info.MethodInfo.DeclaringType, info.MethodInfo.Name));
                }
                if (parameters.Length == 0)
                {
                    info.MethodInfo.Invoke(obj, null);
                }
                else if (parameters[0].ParameterType == typeof(IDataReader))
                {
                    try
                    {
                        info.MethodInfo.Invoke(obj, new object[] { dr });
                        continue;
                    }
                    catch (System.Exception exception)
                    {
                        throw new ReflectionException(string.Format("Type:{0}调用{1}方法发生错误", classInfo.Type, info.MethodInfo.Name), exception);
                    }
                }
            }
        }

        /// <summary>
        /// getChildListInfo
        /// </summary>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="parentType">parentType</param>
        /// <returns>MTFramework.SimpleDataProxy.ORM.MultiTables.ChildListInfo</returns>
        private MTFramework.SimpleDataProxy.ORM.MultiTables.ChildListInfo getChildListInfo(MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, Type parentType)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo info = this.multiTablesInfo.ClassInfoPool[parentType];
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.ChildListInfo info2 in info.ChildListInfoList)
            {
                if (info2.ItemType == classInfo.Type)
                {
                    return info2;
                }
            }
            throw new ORMException(string.Format(ParentObjectMissChildListAttribute, parentType, classInfo.Type, classInfo.TableIndex));
        }

        /// <summary>
        /// getColumnListForUsing
        /// </summary>
        /// <param name="dt">SimpleDataTable</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        private void getColumnListForUsing(SimpleDataTable dt, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo)
        {
            this.columnListForUsing = new Dictionary<string, MultiTables.FieldInfo>();
            bool flag = false;
            bool flag2 = false;
            foreach (SimpleDataColumn column in dt.Columns)
            {
                if (column.ColumnName == classInfo.IdInfo.ColumnName)
                {
                    flag = true;
                    this.columnListForUsing.Add(classInfo.IdInfo.ColumnName, classInfo.IdInfo);
                }
                else if ((classInfo.ParentInfo != null) && (column.ColumnName == classInfo.ParentColumnName))
                {
                    flag2 = true;
                }
            }
            if (!flag)
            {
                throw new ORMException(string.Format(MultiTablesAbsentID, classInfo.Type));
            }
            if ((classInfo.ParentInfo != null) && !flag2)
            {
                throw new ORMException(string.Format(MultiTablesAbsentParentID, classInfo.Type));
            }
            foreach (SimpleDataColumn column2 in dt.Columns)
            {
                if (classInfo.FieldInfoList.ContainsKey(column2.ColumnName))
                {
                    this.columnListForUsing.Add(column2.ColumnName, classInfo.FieldInfoList[column2.ColumnName]);
                }
            }
        }

        /// <summary>
        /// getColumnListForUsing
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        private void getColumnListForUsing(DataTable dt, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo)
        {
            this.columnListForUsing = new Dictionary<string, MultiTables.FieldInfo>();
            if (!dt.Columns.Contains(classInfo.IdInfo.ColumnName))
            {
                throw new ORMException(string.Format(MultiTablesAbsentID, classInfo.Type));
            }
            this.columnListForUsing.Add(classInfo.IdInfo.ColumnName, classInfo.IdInfo);
            if ((classInfo.ParentInfo != null) && !dt.Columns.Contains(classInfo.ParentColumnName))
            {
                throw new ORMException(string.Format(MultiTablesAbsentParentID, classInfo.Type));
            }
            foreach (SimpleDataColumn column in dt.Columns)
            {
                if (classInfo.FieldInfoList.ContainsKey(column.ColumnName))
                {
                    this.columnListForUsing.Add(column.ColumnName, classInfo.FieldInfoList[column.ColumnName]);
                }
            }
        }

        /// <summary>
        /// getColumnListForUsing
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        private void getColumnListForUsing(IDataReader dr, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo)
        {
            this.columnListForUsing = new Dictionary<string, MultiTables.FieldInfo>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                string name = dr.GetName(i);
                dictionary.Add(name, null);
            }
            if (!dictionary.ContainsKey(classInfo.IdInfo.ColumnName))
            {
                throw new ORMException(string.Format(MultiTablesAbsentID, classInfo.Type));
            }
            this.columnListForUsing.Add(classInfo.IdInfo.ColumnName, classInfo.IdInfo);
            if ((classInfo.ParentInfo != null) && !dictionary.ContainsKey(classInfo.ParentColumnName))
            {
                throw new ORMException(string.Format(MultiTablesAbsentParentID, classInfo.Type));
            }
            foreach (string str2 in dictionary.Keys)
            {
                if (classInfo.FieldInfoList.ContainsKey(str2))
                {
                    this.columnListForUsing.Add(str2, classInfo.FieldInfoList[str2]);
                }
            }
        }

        /// <summary>
        /// map 泛型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dataTables">dataTables</param>
        /// <param name="list">泛型集合</param>
        private void map<T>(IList<SimpleDataTable> dataTables, IList<T> list)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[this.multiTablesInfo.TopLevelObjectType];
            this.mapDataTabelToTopLevelObjectList(dataTables[classInfo.TableIndex], classInfo, list);
            this.mapChildren(dataTables, this.topLevelObjectType);
        }

        /// <summary>
        /// map
        /// </summary>
        /// <param name="dataTables">dataTables</param>
        /// <param name="list">IList</param>
        private void map(IList<SimpleDataTable> dataTables, IList list)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[this.multiTablesInfo.TopLevelObjectType];
            this.mapDataTabelToTopLevelObjectList(dataTables[classInfo.TableIndex], classInfo, list);
            this.mapChildren(dataTables, this.topLevelObjectType);
        }

        /// <summary>
        /// map 泛型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dataTables">dataTables</param>
        /// <param name="list">泛型集合</param>
        private void map<T>(IList<DataTable> dataTables, IList<T> list)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[this.multiTablesInfo.TopLevelObjectType];
            this.mapDataTabelToTopLevelObjectList(dataTables[classInfo.TableIndex], classInfo, list);
            this.mapChildren(dataTables, this.topLevelObjectType);
        }

        /// <summary>
        /// map
        /// </summary>
        /// <param name="dataTables">数据表集合</param>
        /// <param name="list">IList</param>
        private void map(IList<DataTable> dataTables, IList list)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[this.multiTablesInfo.TopLevelObjectType];
            this.mapDataTabelToTopLevelObjectList(dataTables[classInfo.TableIndex], classInfo, list);
            this.mapChildren(dataTables, this.topLevelObjectType);
        }

        /// <summary>
        /// map 泛型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dr">IDataReader</param>
        /// <param name="list">泛型集合</param>
        private void map<T>(IDataReader dr, IList<T> list)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[this.multiTablesInfo.TopLevelObjectType];
            this.mapDataReaderToTopLevelObjectList(dr, classInfo, list);
            this.mapChildren(dr, this.topLevelObjectType);
        }

        /// <summary>
        /// map 泛型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dataTables">dataTables</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <param name="list">泛型集合</param>
        public static void Map<T>(IList<SimpleDataTable> dataTables, string multiTablesName, IList<T> list)
        {
            if (dataTables == null)
            {
                throw new ArgumentException("参数:'dataTables'不能为 Null");
            }
            if (multiTablesName == null)
            {
                throw new ArgumentException("参数:'multiTablesName'不能为 Null");
            }
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            new MultiTablesMapper(multiTablesName, typeof(T)).map<T>(dataTables, list);
        }

        /// <summary>
        /// map 泛型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dataTables">dataTables</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <param name="list">泛型集合</param>
        public static void Map<T>(IList<DataTable> dataTables, string multiTablesName, IList<T> list)
        {
            if (dataTables == null)
            {
                throw new ArgumentException("参数:'dataTables'不能为 Null");
            }
            if (multiTablesName == null)
            {
                throw new ArgumentException("参数:'multiTablesName'不能为 Null");
            }
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            new MultiTablesMapper(multiTablesName, typeof(T)).map<T>(dataTables, list);
        }

        /// <summary>
        /// map 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr">IDataReader</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <param name="list">泛型集合</param>
        public static void Map<T>(IDataReader dr, string multiTablesName, IList<T> list)
        {
            new MultiTablesMapper(multiTablesName, typeof(T)).map<T>(dr, list);
        }

        /// <summary>
        /// map
        /// </summary>
        /// <param name="dataTables">dataTables</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <param name="list">list</param>
        /// <param name="topLevelObjectType">topLevelObjectType</param>
        public static void Map(IList<SimpleDataTable> dataTables, string multiTablesName, IList list, Type topLevelObjectType)
        {
            if (dataTables == null)
            {
                throw new ArgumentException("参数:'dataTables'不能为 Null");
            }
            if (multiTablesName == null)
            {
                throw new ArgumentException("参数:'multiTablesName'不能为 Null");
            }
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            if (topLevelObjectType == null)
            {
                throw new ArgumentException("参数:'topLevelObjectType'不能为 Null");
            }
            new MultiTablesMapper(multiTablesName, topLevelObjectType).map(dataTables, list);
        }

        /// <summary>
        /// map
        /// </summary>
        /// <param name="dataTables">dataTables</param>
        /// <param name="multiTablesName">multiTablesName</param>
        /// <param name="list">list</param>
        /// <param name="topLevelObjectType">topLevelObjectType</param>
        public static void Map(IList<DataTable> dataTables, string multiTablesName, IList list, Type topLevelObjectType)
        {
            if (dataTables == null)
            {
                throw new ArgumentException("参数:'dataTables'不能为 Null");
            }
            if (multiTablesName == null)
            {
                throw new ArgumentException("参数:'multiTablesName'不能为 Null");
            }
            if (list == null)
            {
                throw new ArgumentException("参数:'list'不能为 Null");
            }
            if (topLevelObjectType == null)
            {
                throw new ArgumentException("参数:'topLevelObjectType'不能为 Null");
            }
            new MultiTablesMapper(multiTablesName, topLevelObjectType).map(dataTables, list);
        }

        /// <summary>
        /// mapChildren
        /// </summary>
        /// <param name="dataTables">dataTables</param>
        /// <param name="type">type</param>
        private void mapChildren(IList<SimpleDataTable> dataTables, Type type)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo info = this.multiTablesInfo.ClassInfoPool[type];
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.ChildListInfo info2 in info.ChildListInfoList)
            {
                MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[info2.ItemType];
                int tableIndex = classInfo.TableIndex;
                if (info2.ItemTableIndex.HasValue)
                {
                    tableIndex = info2.ItemTableIndex.Value;
                }
                if (dataTables.Count > tableIndex)
                {
                    this.mapDataTabelToSubObject(dataTables[tableIndex], classInfo, type);
                    this.mapChildren(dataTables, classInfo.Type);
                }
            }
        }

        /// <summary>
        /// mapChildren
        /// </summary>
        /// <param name="dataTables">dataTables</param>
        /// <param name="type">type</param>
        private void mapChildren(IList<DataTable> dataTables, Type type)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo info = this.multiTablesInfo.ClassInfoPool[type];
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.ChildListInfo info2 in info.ChildListInfoList)
            {
                MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[info2.ItemType];
                int tableIndex = classInfo.TableIndex;
                if (info2.ItemTableIndex.HasValue)
                {
                    tableIndex = info2.ItemTableIndex.Value;
                }
                if (dataTables.Count > tableIndex)
                {
                    this.mapDataTabelToSubObject(dataTables[tableIndex], classInfo, type);
                    this.mapChildren(dataTables, classInfo.Type);
                }
            }
        }

        /// <summary>
        /// mapChildren
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="type">Type</param>
        private void mapChildren(IDataReader dr, Type type)
        {
            MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo info = this.multiTablesInfo.ClassInfoPool[type];
            foreach (MTFramework.SimpleDataProxy.ORM.MultiTables.ChildListInfo info2 in info.ChildListInfoList)
            {
                MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo = this.multiTablesInfo.ClassInfoPool[info2.ItemType];
                int tableIndex = classInfo.TableIndex;
                if (info2.ItemTableIndex.HasValue)
                {
                    int local1 = info2.ItemTableIndex.Value;
                }
                if (dr.NextResult())
                {
                    this.mapDataReaderToSubObject(dr, classInfo, type);
                    this.mapChildren(dr, classInfo.Type);
                }
            }
        }

        /// <summary>
        /// mapDataReaderToSubObject
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="parentType">parentType</param>
        private void mapDataReaderToSubObject(IDataReader dr, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, Type parentType)
        {
            this.getColumnListForUsing(dr, classInfo);
            while (dr.Read())
            {
                object key = dr[classInfo.IdInfo.ColumnName];
                object result = Activator.CreateInstance(classInfo.Type, true);
                this.setObjectValue(dr, classInfo, result);
                this.objectPool.Add(classInfo.Type, key, result);
                object parent = this.objectPool.Get(parentType, dr[classInfo.ParentColumnName]);
                if (parent == null)
                {
                    throw new ORMException(string.Format(MultiTablesParentNotFound, classInfo.Type, dr[classInfo.ParentColumnName]));
                }
                if (classInfo.ParentInfo != null)
                {
                    setObjectParent(classInfo, result, parent);
                }
                this.afterMapDataInfo(classInfo, dr, result);
                ListReflector.ListAddItem(this.getChildListInfo(classInfo, parentType).Member.GetValue(parent), result);
            }
        }

        /// <summary>
        /// mapDataReaderToSubObject
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="topLevelObjectList">topLevelObjectList</param>
        private void mapDataReaderToTopLevelObjectList(IDataReader dr, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object topLevelObjectList)
        {
            this.getColumnListForUsing(dr, classInfo);
            Type type = topLevelObjectList.GetType();
            MethodInfo method = type.GetMethod("Add", new Type[] { classInfo.Type });
            if (method == null)
            {
                throw new ReflectionException(string.Format("Type:{0}没有找到 Add 方法", type));
            }
            while (dr.Read())
            {
                object key = dr[classInfo.IdInfo.ColumnName];
                object result = Activator.CreateInstance(classInfo.Type, true);
                this.setObjectValue(dr, classInfo, result);
                this.objectPool.Add(classInfo.Type, key, result);
                this.afterMapDataInfo(classInfo, dr, result);
                try
                {
                    method.Invoke(topLevelObjectList, new object[] { result });
                }
                catch (System.Exception exception)
                {
                    throw new ReflectionException(string.Format("Type:{0}调用 Add 方法发生错误", type), exception);
                }
            }
        }

        /// <summary>
        /// mapDataReaderToSubObject
        /// </summary>
        /// <param name="dt">SimpleDataTable</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="parentType">parentType</param>
        private void mapDataTabelToSubObject(SimpleDataTable dt, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, Type parentType)
        {
            this.getColumnListForUsing(dt, classInfo);
            foreach (SimpleDataRow row in dt.Rows)
            {
                object key = row[classInfo.IdInfo.ColumnName];
                object result = Activator.CreateInstance(classInfo.Type, true);
                this.setObjectValue(row, classInfo, result);
                this.objectPool.Add(classInfo.Type, key, result);
                object parent = this.objectPool.Get(parentType, row[classInfo.ParentColumnName]);
                if (classInfo.ParentInfo != null)
                {
                    setObjectParent(classInfo, result, parent);
                }
                this.afterMapDataInfo(classInfo, row, result);
                ListReflector.ListAddItem(this.getChildListInfo(classInfo, parentType).Member.GetValue(parent), result);
            }
        }

        /// <summary>
        /// mapDataReaderToSubObject
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="parentType">parentType</param>
        private void mapDataTabelToSubObject(DataTable dt, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, Type parentType)
        {
            this.getColumnListForUsing(dt, classInfo);
            foreach (DataRow row in dt.Rows)
            {
                object key = row[classInfo.IdInfo.ColumnName];
                object result = Activator.CreateInstance(classInfo.Type, true);
                this.setObjectValue(row, classInfo, result);
                this.objectPool.Add(classInfo.Type, key, result);
                object parent = this.objectPool.Get(parentType, row[classInfo.ParentColumnName]);
                if (parent == null)
                {
                    throw new ORMException(string.Format(MultiTablesParentNotFound, classInfo.Type, row[classInfo.ParentColumnName]));
                }
                if (classInfo.ParentInfo != null)
                {
                    setObjectParent(classInfo, result, parent);
                }
                this.afterMapDataInfo(classInfo, row, result);
                ListReflector.ListAddItem(this.getChildListInfo(classInfo, parentType).Member.GetValue(parent), result);
            }
        }

        /// <summary>
        /// mapDataReaderToSubObject
        /// </summary>
        /// <param name="dt">SimpleDataTable</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="topLevelObjectList">topLevelObjectList</param>
        private void mapDataTabelToTopLevelObjectList(SimpleDataTable dt, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object topLevelObjectList)
        {
            this.getColumnListForUsing(dt, classInfo);
            Type type = topLevelObjectList.GetType();
            MethodInfo method = type.GetMethod("Add", new Type[] { classInfo.Type });
            if (method == null)
            {
                throw new ReflectionException(string.Format("Type:{0}没有找到 Add 方法", type));
            }
            foreach (SimpleDataRow row in dt.Rows)
            {
                object key = row[classInfo.IdInfo.ColumnName];
                object result = Activator.CreateInstance(classInfo.Type, true);
                this.setObjectValue(row, classInfo, result);
                this.objectPool.Add(classInfo.Type, key, result);
                this.afterMapDataInfo(classInfo, row, result);
                try
                {
                    method.Invoke(topLevelObjectList, new object[] { result });
                    continue;
                }
                catch (System.Exception exception)
                {
                    throw new ReflectionException(string.Format("Type:{0}调用 Add 方法发生错误", type), exception);
                }
            }
        }

        /// <summary>
        /// mapDataReaderToSubObject
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo</param>
        /// <param name="topLevelObjectList">topLevelObjectList</param>
        private void mapDataTabelToTopLevelObjectList(DataTable dt, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object topLevelObjectList)
        {
            this.getColumnListForUsing(dt, classInfo);
            Type type = topLevelObjectList.GetType();
            MethodInfo method = type.GetMethod("Add", new Type[] { classInfo.Type });
            if (method == null)
            {
                throw new ReflectionException(string.Format("Type:{0}没有找到 Add 方法", type));
            }
            foreach (DataRow row in dt.Rows)
            {
                object key = row[classInfo.IdInfo.ColumnName];
                object result = Activator.CreateInstance(classInfo.Type, true);
                this.setObjectValue(row, classInfo, result);
                this.objectPool.Add(classInfo.Type, key, result);
                this.afterMapDataInfo(classInfo, row, result);
                try
                {
                    method.Invoke(topLevelObjectList, new object[] { result });
                    continue;
                }
                catch (System.Exception exception)
                {
                    throw new ReflectionException(string.Format("Type:{0}调用 Add 方法发生错误", type), exception);
                }
            }
        }

        /// <summary>
        /// setObjectParent
        /// </summary>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="result">result</param>
        /// <param name="parent">parent</param>
        private static void setObjectParent(MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object result, object parent)
        {
            if (classInfo.ParentInfo != null)
            {
                classInfo.ParentInfo.Member.SetValue(result, parent);
            }
        }

        #region --- 设置对象的值 Begin ---

        /// <summary>
        /// 设置对象的值
        /// </summary>
        /// <param name="dr">SimpleDataRow</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="result">result</param>
        private void setObjectValue(SimpleDataRow dr, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object result)
        {
            foreach (MultiTables.FieldInfo info in this.columnListForUsing.Values)
            {
                setValue(dr, result, info);
            }
        }

        /// <summary>
        /// 设置对象的值
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="result">result</param>
        private void setObjectValue(DataRow dr, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object result)
        {
            foreach (MultiTables.FieldInfo info in this.columnListForUsing.Values)
            {
                setValue(dr, result, info);
            }
        }

        /// <summary>
        /// 设置对象的值
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="classInfo">MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo</param>
        /// <param name="result">result</param>
        private void setObjectValue(IDataReader dr, MTFramework.SimpleDataProxy.ORM.MultiTables.ClassInfo classInfo, object result)
        {
            foreach (MultiTables.FieldInfo info in this.columnListForUsing.Values)
            {
                setValue(dr, result, info);
            }
        }

        #endregion --- 设置对象的值 End ---

        #region --- 设置资料数据值 Begin ---

        /// <summary>
        /// 设置资料数据值
        /// </summary>
        /// <param name="dr">SimpleDataRow</param>
        /// <param name="result">result</param>
        /// <param name="fieldInfo">MultiTables.FieldInfo</param>
        private static void setValue(SimpleDataRow dr, object result, MultiTables.FieldInfo fieldInfo)
        {
            object obj2 = GetDataCellValue(dr, fieldInfo.ColumnName, fieldInfo.DataType);
            fieldInfo.Member.SetValue(result, obj2);
        }

        /// <summary>
        /// 设置资料数据值
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="result">result</param>
        /// <param name="fieldInfo">MultiTables.FieldInfo</param>
        private static void setValue(DataRow dr, object result, MultiTables.FieldInfo fieldInfo)
        {
            object obj2 = GetDataCellValue(dr, fieldInfo.ColumnName, fieldInfo.DataType);
            fieldInfo.Member.SetValue(result, obj2);
        }

        /// <summary>
        /// 设置资料数据值
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="result">result</param>
        /// <param name="fieldInfo">MultiTables.FieldInfo</param>
        private static void setValue(IDataReader dr, object result, MultiTables.FieldInfo fieldInfo)
        {
            object obj2 = GetDataCellValue(dr, fieldInfo.ColumnName, fieldInfo.DataType);
            fieldInfo.Member.SetValue(result, obj2);
        }

        #endregion --- 设置资料数据值 End ---

        #region --- 获取数据资料值 Begin ---

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="dataType">dataType</param>
        /// <returns>object</returns>
        public static object convertDataValue(object value, DbType dataType)
        {
            if (value != DBNull.Value)
            {
                switch (dataType)
                {
                    case DbType.AnsiString:
                        return Convert.ToString(value);
                    case DbType.AnsiStringFixedLength:
                        return Convert.ToString(value);
                    case DbType.Binary:
                        return (byte[])value;
                    case DbType.Boolean:
                        return Convert.ToBoolean(value);
                    case DbType.Byte:
                        return Convert.ToByte(value);
                    case DbType.Currency:
                        return Convert.ToDecimal(value);
                    case DbType.Date:
                        return Convert.ToDateTime(value);
                    case DbType.DateTime:
                        return Convert.ToDateTime(value);
                    case DbType.DateTime2:
                        return Convert.ToDateTime(value);
                    case DbType.DateTimeOffset:
                        return Convert.ToDateTime(value);
                    case DbType.Decimal:
                        return Convert.ToDecimal(value);
                    case DbType.Double:
                        return Convert.ToDouble(value);
                    case DbType.Guid:
                        return (Guid)value;
                    case DbType.Int16:
                        return Convert.ToInt16(value);
                    case DbType.Int32:
                        return Convert.ToInt32(value);
                    case DbType.Int64:
                        return Convert.ToInt64(value);
                    case DbType.Object:
                        return value;
                    case DbType.SByte:
                        return Convert.ToSByte(value);
                    case DbType.Single:
                        return (float)value;
                    case DbType.String:
                        return Convert.ToString(value);
                    case DbType.StringFixedLength:
                        return Convert.ToString(value);
                    case DbType.Time:
                        return Convert.ToDateTime(value);
                    case DbType.UInt16:
                        return Convert.ToUInt16(value);
                    case DbType.UInt32:
                        return Convert.ToUInt32(value);
                    case DbType.UInt64:
                        return Convert.ToUInt64(value);
                    case DbType.VarNumeric:
                        return Convert.ToDecimal(value);
                    case DbType.Xml:
                        return Convert.ToString(value);
                    default:
                        return value;
                }
            }

            return null;

        }

        /// <summary>
        /// 获取数据资料值
        /// </summary>
        /// <param name="dr">SimpleDataRow</param>
        /// <param name="propName">propName</param>
        /// <param name="dataType">dataType</param>
        /// <returns>object</returns>
        public static object GetDataCellValue(SimpleDataRow dr, string propName, DbType dataType)
        {
            object obj3;
            object obj2 = dr[propName];
            try
            {
                obj3 = convertDataValue(obj2, dataType);
            }
            catch (System.Exception)
            {

                throw new ORMException(string.Format(PropertyDataTypeConvertFail, propName, dataType.ToString()));
            }
            return obj3;
        }

        /// <summary>
        /// 获取数据资料值
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="propName">propName</param>
        /// <param name="dataType">dataType</param>
        /// <returns>object</returns>
        public static object GetDataCellValue(DataRow dr, string propName, DbType dataType)
        {
            object obj3;
            object obj2 = dr[propName];
            try
            {
                obj3 = convertDataValue(obj2, dataType);
            }
            catch (System.Exception)
            {
                throw new ORMException(string.Format(PropertyDataTypeConvertFail, propName, dataType.ToString()));
            }
            return obj3;
        }

        /// <summary>
        /// 获取数据资料值
        /// </summary>
        /// <param name="dr">IDataReader</param>
        /// <param name="propName">propName</param>
        /// <param name="dataType">dataType</param>
        /// <returns>object</returns>
        public static object GetDataCellValue(IDataReader dr, string propName, DbType dataType)
        {
            object obj3;
            object obj2 = dr[propName];
            try
            {
                obj3 = convertDataValue(obj2, dataType);
            }
            catch (System.Exception)
            {
                throw new ORMException(string.Format(PropertyDataTypeConvertFail, propName, dataType.ToString()));
            }
            return obj3;
        }

        #endregion --- 获取数据资料值 End ---
    }

}
