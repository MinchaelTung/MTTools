using System;
using System.Collections.Generic;
using System.Text;
using MTFramework.SimpleDataProxy.ORM.Common;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using System.Data;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// 数据反射工具
    /// </summary>
    public static class SimpleDataAccessORMUtility
    {
        private const string SourceAndTargetNotTheSameType = "来源对象和目标对象不一致.(source:{0}  target:{1})";
        private const string PropertyDataNotFound = "结果中没有: {0} 数据";
        private const string MarkedFieldOfMultiFieldAttributeCanNotIsNull = "在MultiFieldAttribute标记字段不能为空.";



        #region --- 对数据库资料执行删除操作 Begin ---

        /// <summary>
        /// 对数据库资料执行删除操作
        /// </summary>
        /// <param name="obj">需要删除的对象</param>
        /// <param name="db">数据库操作代理</param>
        public static void Delete(object obj, DataProxy db)
        {
            Delete(obj, db, null);
        }

        /// <summary>
        /// 对数据库资料执行删除操作
        /// </summary>
        /// <param name="obj">需要删除的对象</param>
        /// <param name="db">数据库操作代理</param>
        /// <param name="beforeExcuteEventHandler">执行存储过程之前事件</param>
        public static void Delete(object obj, DataProxy db, EventHandler<ProcedureEventArgs> beforeExcuteEventHandler)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (db == null)
            {
                throw new ArgumentException("参数:'db'不能为 Null");
            }
            Type type = obj.GetType();
            TableInfoCache.GetObjectInfo(type);
            Procedure deleteProcedure = GetDeleteProcedure(obj);

            try
            {
                if (beforeExcuteEventHandler != null)
                {
                    ProcedureEventArgs e = new ProcedureEventArgs(deleteProcedure);
                    beforeExcuteEventHandler(null, e);
                }
                db.ExecuteProc(deleteProcedure);
                foreach (ExtParameterInfo info2 in GetAccessInfo(type, AccessType.Delete).ExtParameters)
                {
                    if (info2.IsOut && isContainParameter(deleteProcedure, info2.ColumnName))
                    {
                        info2.Member.SetValue(obj, deleteProcedure[info2.ColumnName]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


        }

        #endregion --- 对数据库资料执行删除操作 End ---

        #region --- 对数据库资料执行更新操作 Begin ---

        /// <summary>
        /// 对数据库资料执行更新操作
        /// </summary>
        /// <param name="obj">需要更新的对象</param>
        /// <param name="db">数据库操作代理</param>
        public static void Update(object obj, DataProxy db)
        {
            Update(obj, db, null);
        }

        /// <summary>
        /// 对数据库资料执行更新操作
        /// </summary>
        /// <param name="obj">需要更新对象</param>
        /// <param name="db">数据库操作代理</param>
        /// <param name="beforeExcuteEventHandler">存储过程代理</param>
        public static void Update(object obj, DataProxy db, EventHandler<ProcedureEventArgs> beforeExcuteEventHandler)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (db == null)
            {
                throw new ArgumentException("参数:'db'不能为 Null");
            }
            Type type = obj.GetType();
            TableInfoCache.GetObjectInfo(type);
            Procedure updateProcedure = GetUpdateProcedure(obj);

            try
            {
                if (beforeExcuteEventHandler != null)
                {
                    ProcedureEventArgs e = new ProcedureEventArgs(updateProcedure);
                    beforeExcuteEventHandler(null, e);
                }
                db.ExecuteProc(updateProcedure);
                foreach (ExtParameterInfo info2 in GetAccessInfo(type, AccessType.Update).ExtParameters)
                {
                    if (info2.IsOut && isContainParameter(updateProcedure, info2.ColumnName))
                    {
                        info2.Member.SetValue(obj, updateProcedure[info2.ColumnName]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion --- 对数据库资料执行更新操作 End ---

        #region --- 对数据库执行添加新资料操作 Begin ---

        /// <summary>
        /// 对数据库执行添加新资料操作
        /// </summary>
        /// <param name="obj">新增对象</param>
        /// <param name="db">数据库操作代理</param>
        public static void Insert(object obj, DataProxy db)
        {
            Insert(obj, db, null);
        }
        /// <summary>
        /// 对数据库执行添加新资料操作
        /// </summary>
        /// <param name="obj">新增对象</param>
        /// <param name="db">数据库操作代理</param>
        /// <param name="beforeExcuteEventHandler">存储过程代理</param>
        public static void Insert(object obj, DataProxy db, EventHandler<ProcedureEventArgs> beforeExcuteEventHandler)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (db == null)
            {
                throw new ArgumentException("参数:'db'不能为 Null");
            }
            Type type = obj.GetType();
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            Procedure insertProcedure = GetInsertProcedure(obj);

            try
            {
                if (beforeExcuteEventHandler != null)
                {
                    ProcedureEventArgs e = new ProcedureEventArgs(insertProcedure);
                    beforeExcuteEventHandler(null, e);
                }
                db.ExecuteProc(insertProcedure);
                if (((objectInfo.IdInfo != null) && objectInfo.IdInfo.IsOut) && isContainParameter(insertProcedure, objectInfo.IdInfo.ColumnName))
                {
                    objectInfo.IdInfo.Member.SetValue(obj, insertProcedure[objectInfo.IdInfo.ColumnName]);
                }
                foreach (ExtParameterInfo info3 in GetAccessInfo(type, AccessType.Create).ExtParameters)
                {
                    if (info3.IsOut && isContainParameter(insertProcedure, info3.ColumnName))
                    {
                        info3.Member.SetValue(obj, insertProcedure[info3.ColumnName]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion --- 对数据库执行添加新资料操作 End ---

        #region --- 读取数据库资料信息操作 Begin ---

        /// <summary>
        /// 读取数据库资料信息操作
        /// </summary>
        /// <param name="obj">查询赋值对象</param>
        /// <param name="dr">简单数据表行</param>
        public static void MapData(object obj, SimpleDataRow dr)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (dr == null)
            {
                throw new ArgumentException("参数:'dr'不能为 Null");
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(obj.GetType());
            try
            {
                object obj2 = null;
                try
                {
                    obj2 = MultiTablesMapper.GetDataCellValue(dr, objectInfo.IdInfo.ColumnName, objectInfo.IdInfo.DataType);
                }
                catch (ArgumentException)
                {
                    throw new ORMException(string.Format(PropertyDataNotFound, objectInfo.IdInfo.ColumnName));
                }
                if (objectInfo.IdInfo != null)
                {
                    objectInfo.IdInfo.Member.SetValue(obj, obj2);
                }
                foreach (FieldInfo info2 in objectInfo.FieldInfoList.Values)
                {
                    object obj3 = null;
                    try
                    {
                        obj3 = MultiTablesMapper.GetDataCellValue(dr, info2.ColumnName, info2.DataType);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    info2.Member.SetValue(obj, obj3);
                }
                foreach (MultiFieldInfo info3 in objectInfo.MultiFieldInfoList.Values)
                {
                    object obj4 = null;
                    try
                    {
                        obj4 = MultiTablesMapper.GetDataCellValue(dr, info3.ColumnName, info3.DataType);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    object target = info3.Member.GetValue(obj);
                    if (target == null)
                    {
                        throw new ORMException(MarkedFieldOfMultiFieldAttributeCanNotIsNull);
                    }
                    ReflectionUtilities.GetMember(target.GetType(), info3.MemberName).SetValue(target, obj4);
                }
                foreach (AfterMapDataInfo info4 in objectInfo.AfterMapDataInfoList)
                {
                    info4.MethodInfo.Invoke(obj, new object[] { dr });
                }
            }
            catch (ArgumentException exception)
            {
                throw new ORMException(exception);
            }
        }

        /// <summary>
        /// 读取数据库资料信息操作
        /// </summary>
        /// <param name="obj">查询赋值对象</param>
        /// <param name="dr">数据表行</param>
        public static void MapData(object obj, DataRow dr)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (dr == null)
            {
                throw new ArgumentException("参数:'dr'不能为 Null");
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(obj.GetType());
            try
            {
                if (objectInfo.IdInfo != null)
                {
                    object obj2 = null;
                    try
                    {
                        obj2 = MultiTablesMapper.GetDataCellValue(dr, objectInfo.IdInfo.ColumnName, objectInfo.IdInfo.DataType);
                    }
                    catch (ArgumentException)
                    {
                        throw new ORMException(string.Format(PropertyDataNotFound, objectInfo.IdInfo.ColumnName));
                    }
                    objectInfo.IdInfo.Member.SetValue(obj, obj2);
                }
                foreach (FieldInfo info2 in objectInfo.FieldInfoList.Values)
                {
                    object obj3 = null;
                    try
                    {
                        obj3 = MultiTablesMapper.GetDataCellValue(dr, info2.ColumnName, info2.DataType);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    info2.Member.SetValue(obj, obj3);
                }
                foreach (MultiFieldInfo info3 in objectInfo.MultiFieldInfoList.Values)
                {
                    object obj4 = null;
                    try
                    {
                        obj4 = MultiTablesMapper.GetDataCellValue(dr, info3.ColumnName, info3.DataType);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    object target = info3.Member.GetValue(obj);
                    if (target == null)
                    {
                        throw new ORMException(MarkedFieldOfMultiFieldAttributeCanNotIsNull);
                    }
                    ReflectionUtilities.GetMember(target.GetType(), info3.MemberName).SetValue(target, obj4);
                }
                foreach (AfterMapDataInfo info4 in objectInfo.AfterMapDataInfoList)
                {
                    info4.MethodInfo.Invoke(obj, new object[] { dr });
                }
            }
            catch (ArgumentException exception)
            {
                throw new ORMException(exception);
            }
        }

        /// <summary>
        /// 读取数据库资料信息操作
        /// </summary>
        /// <typeparam name="T">查询赋值T泛型对象</typeparam>
        /// <param name="dr">简单数据表行</param>
        /// <returns>返回该对象</returns>
        public static T MapData<T>(SimpleDataRow dr)
        {
            T local = (T)MethodCaller.CreateInstance(typeof(T));
            MapData(local, dr);
            return local;
        }

        /// <summary>
        /// 读取数据库资料信息操作
        /// </summary>
        /// <typeparam name="T">查询赋值</typeparam>
        /// <param name="dr">数据表行</param>
        /// <returns>返回该对象</returns>
        public static T MapData<T>(DataRow dr)
        {
            T local = (T)MethodCaller.CreateInstance(typeof(T));
            MapData(local, dr);
            return local;
        }

        #endregion --- 读取数据库资料信息操作 End ---

        #region --- 复制对象 Begin ---

        /// <summary>
        /// 复制对象成员
        /// </summary>
        /// <param name="fieldInfo">成员信息</param>
        /// <param name="source">来源对象</param>
        /// <param name="target">目标对象</param>
        private static void copy(FieldInfo fieldInfo, object source, object target)
        {
            object obj2 = fieldInfo.Member.GetValue(source);
            fieldInfo.Member.SetValue(target, obj2);
        }

        /// <summary>
        /// 复制对象成员
        /// </summary>
        /// <param name="source">来源对象</param>
        /// <param name="target">目标对象</param>
        public static void Copy(object source, object target)
        {
            if (source == null)
            {
                throw new ArgumentException("参数:'source'不能为 Null");
            }
            if (target == null)
            {
                throw new ArgumentException("参数:'target'不能为 Null");
            }
            Type type = source.GetType();
            Type type2 = target.GetType();
            if (type != type2)
            {
                throw new ORMException(string.Format(SourceAndTargetNotTheSameType, type, type2));
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            copy(objectInfo.IdInfo, source, target);
            foreach (FieldInfo info2 in objectInfo.FieldInfoList.Values)
            {
                copy(info2, source, target);
            }
        }

        #endregion --- 复制对象 End ---

        #region --- 获取存储过程名称和参数 Begin ---

        /// <summary>
        /// 获取默认添加资料存储过程名称
        /// </summary>
        /// <param name="obj">需要添加的对象</param>
        /// <returns>返回该对象中的添加的存储过程</returns>
        public static Procedure GetInsertProcedure(object obj)
        {
            return GetInsertProcedure(obj, string.Empty);
        }

        /// <summary>
        /// 获取添加资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回该对象中的添加存储过程</returns>
        public static Procedure GetInsertProcedure(object obj, string accessName)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            Type type = obj.GetType();
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            Procedure procedure = new Procedure();
            AccessInfo info2 = GetAccessInfo(type, AccessType.Create, accessName);
            procedure.Text = info2.Procedure;
            if (objectInfo.IdInfo != null)
            {
                Parameter item = buildParameter(obj, objectInfo.IdInfo, objectInfo.IdInfo.IsOut);
                procedure.Parameters.Add(item);
            }
            foreach (FieldInfo info3 in objectInfo.FieldInfoList.Values)
            {
                Parameter parameter2 = buildParameter(obj, info3);
                procedure.Parameters.Add(parameter2);
            }
            foreach (MultiFieldInfo info4 in objectInfo.MultiFieldInfoList.Values)
            {
                object target = info4.Member.GetValue(obj);
                object obj3 = ReflectionUtilities.GetMember(target.GetType(), info4.MemberName).GetValue(target);
                Parameter parameter3 = new Parameter(info4.ColumnName, obj3);
                procedure.Parameters.Add(parameter3);
            }
            foreach (ExtParameterInfo info5 in info2.ExtParameters)
            {
                Parameter parameter4 = buildParameter(obj, info5, info5.IsOut);
                foreach (Parameter parameter5 in procedure.Parameters)
                {
                    if (parameter5.Name == parameter4.Name)
                    {
                        procedure.Parameters.Remove(parameter5);
                        break;
                    }
                }
                procedure.Parameters.Add(parameter4);
            }
            return procedure;
        }

        /// <summary>
        /// 获取默认更新资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>返回该对象中的更新存储过程</returns>
        public static Procedure GetUpdateProcedure(object obj)
        {
            return GetUpdateProcedure(obj, string.Empty);
        }

        /// <summary>
        /// 获取更新资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回该对象中的更新存储过程</returns>
        public static Procedure GetUpdateProcedure(object obj, string accessName)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            Type type = obj.GetType();
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            Procedure procedure = new Procedure();
            AccessInfo info2 = GetAccessInfo(type, AccessType.Update, accessName);
            procedure.Text = info2.Procedure;
            if (objectInfo.IdInfo != null)
            {
                Parameter item = buildParameter(obj, objectInfo.IdInfo);
                procedure.Parameters.Add(item);
            }
            foreach (FieldInfo info3 in objectInfo.FieldInfoList.Values)
            {
                Parameter parameter2 = buildParameter(obj, info3);
                procedure.Parameters.Add(parameter2);
            }
            foreach (MultiFieldInfo info4 in objectInfo.MultiFieldInfoList.Values)
            {
                object target = info4.Member.GetValue(obj);
                object obj3 = ReflectionUtilities.GetMember(target.GetType(), info4.MemberName).GetValue(target);
                Parameter parameter3 = new Parameter(info4.ColumnName, obj3);
                procedure.Parameters.Add(parameter3);
            }
            foreach (ExtParameterInfo info5 in info2.ExtParameters)
            {
                Parameter parameter4 = buildParameter(obj, info5, info5.IsOut);
                foreach (Parameter parameter5 in procedure.Parameters)
                {
                    if (parameter5.Name == parameter4.Name)
                    {
                        procedure.Parameters.Remove(parameter5);
                        break;
                    }
                }
                procedure.Parameters.Add(parameter4);
            }
            return procedure;
        }

        /// <summary>
        /// 获取默认删除资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>返回该对象中的删除存储过程</returns>
        public static Procedure GetDeleteProcedure(object obj)
        {
            return GetDeleteProcedure(obj, string.Empty);
        }

        /// <summary>
        /// 获取删除资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回该对象中的删除存储过程</returns>
        public static Procedure GetDeleteProcedure(object obj, string accessName)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            Type type = obj.GetType();
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            Procedure procedure = new Procedure();
            AccessInfo info2 = GetAccessInfo(type, AccessType.Delete, accessName);
            procedure.Text = info2.Procedure;
            Parameter item = buildParameter(obj, objectInfo.IdInfo);
            procedure.Parameters.Add(item);
            foreach (ExtParameterInfo info3 in info2.ExtParameters)
            {
                Parameter parameter2 = buildParameter(obj, info3, info3.IsOut);
                foreach (Parameter parameter3 in procedure.Parameters)
                {
                    if (parameter3.Name == parameter2.Name)
                    {
                        procedure.Parameters.Remove(parameter3);
                        break;
                    }
                }
                procedure.Parameters.Add(parameter2);
            }
            return procedure;
        }

        /// <summary>
        /// 获取默认读取资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>返回该对象中的读取存储过程</returns>
        public static Procedure GetFetchAllProcedure(object obj)
        {
            return GetFetchAllProcedure(obj, string.Empty);
        }

        /// <summary>
        /// 获取默认阅览资料存储过程名称
        /// </summary>
        /// <param name="type">实体对象类型</param>
        /// <returns>返回该对象中的读取存储过程</returns>
        public static Procedure GetFetchAllProcedure(Type type)
        {
            return GetFetchAllProcedure(type, string.Empty);
        }

        /// <summary>
        /// 获取阅览资料存储过程名称
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回该对象中的读取存储过程</returns>
        public static Procedure GetFetchAllProcedure(object obj, string accessName)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            AccessInfo info = GetAccessInfo(obj.GetType(), AccessType.Fetch, accessName);
            Procedure procedure = new Procedure();
            procedure.Text = info.Procedure;
            foreach (FieldInfo info2 in info.ExtParameters)
            {
                Parameter item = buildParameter(obj, info2);
                foreach (Parameter parameter2 in procedure.Parameters)
                {
                    if (parameter2.Name == item.Name)
                    {
                        procedure.Parameters.Remove(parameter2);
                        break;
                    }
                }
                procedure.Parameters.Add(item);
            }
            return procedure;
        }

        /// <summary>
        /// 获取阅览资料存储过程名称
        /// </summary>
        /// <param name="type">实体对象</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回该对象中的读取存储过程</returns>
        public static Procedure GetFetchAllProcedure(Type type, string accessName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            AccessInfo info = GetAccessInfo(type, AccessType.Fetch, accessName);
            Procedure procedure = new Procedure();
            procedure.Text = info.Procedure;
            foreach (FieldInfo info2 in info.ExtParameters)
            {
                Parameter item = buildParameter(type, info2);
                foreach (Parameter parameter2 in procedure.Parameters)
                {
                    if (parameter2.Name == item.Name)
                    {
                        procedure.Parameters.Remove(parameter2);
                        break;
                    }
                }
                procedure.Parameters.Add(item);
            }
            return procedure;
        }

        /// <summary>
        /// 构造存储过程参数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="field">成员信息</param>
        /// <returns>存储过程参数</returns>
        private static Parameter buildParameter(object obj, FieldInfo field)
        {
            return buildParameter(obj, field, false);
        }

        /// <summary>
        /// 构造存储过程参数
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="field">成员信息</param>
        /// <returns>存储过程参数</returns>
        private static Parameter buildParameter(Type type, FieldInfo field)
        {
            return buildParameter(type, field, false);
        }

        /// <summary>
        /// 构造存储过程参数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="field">成员信息</param>
        /// <param name="isOut">是否需要输出</param>
        /// <returns>存储过程参数</returns>
        private static Parameter buildParameter(object obj, FieldInfo field, bool isOut)
        {
            Parameter parameter = new Parameter();
            if (isOut)
            {
                parameter.Direction = ParameterDirection.Output;
            }
            else
            {
                parameter.Direction = ParameterDirection.Input;
            }
            parameter.Name = field.ColumnName;

            parameter.Value = field.Member.GetValue(obj);
            return parameter;
        }

        /// <summary>
        /// 构造存储过程参数
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="field">成员信息</param>
        /// <param name="isOut">是否需要输出</param>
        /// <returns>存储过程参数</returns>
        private static Parameter buildParameter(Type type, FieldInfo field, bool isOut)
        {
            Parameter parameter = new Parameter();
            if (isOut)
            {
                parameter.Direction = ParameterDirection.Output;
            }
            else
            {
                parameter.Direction = ParameterDirection.Input;
            }
            parameter.Name = field.ColumnName;

            return parameter;
        }

        /// <summary>
        /// 获取存储过程标记信息
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="accessType">执行的存储过程类型</param>
        /// <returns>返回存储过程标记信息</returns>
        public static AccessInfo GetAccessInfo(Type type, AccessType accessType)
        {
            return TableInfoCache.GetTableInfo(type).GetAccessInfo(type, accessType);
        }

        /// <summary>
        /// 获取存储过程标记信息
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="accessName">数据库名称</param>
        /// <returns>返回存储过程标记信息</returns>
        public static AccessInfo GetAccessInfo(Type type, string accessName)
        {
            return TableInfoCache.GetTableInfo(type).GetAccessInfo(type, accessName);
        }

        /// <summary>
        /// 获取存储过程标记信息
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="accessType">存储过程类型</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回存储过程标记信息</returns>
        public static AccessInfo GetAccessInfo(Type type, AccessType accessType, string accessName)
        {
            return TableInfoCache.GetTableInfo(type).GetAccessInfo(type, accessType, accessName);
        }

        /// <summary>
        /// 获取根据主键读取资料存储过程名称
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(object obj)
        {
            return GetFetchByIDProcedure(obj, string.Empty);
        }

        /// <summary>
        /// 获取根据主键读取资料存储过程名称
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(Type type)
        {
            return GetFetchByIDProcedure(type, string.Empty);
        }

        /// <summary>
        /// 获取根据主键读取资料存储过程名称
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="id">对象ID</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(object obj, object id)
        {
            return GetFetchByIDProcedure(obj, id, string.Empty);
        }

        /// <summary>
        /// 获取根据主键读取资料存储过程名称
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(object obj, string accessName)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(obj.GetType());
            Procedure fetchAllProcedure = GetFetchAllProcedure(obj, accessName);
            Parameter item = buildParameter(obj, objectInfo.IdInfo);
            fetchAllProcedure.Parameters.Add(item);
            return fetchAllProcedure;
        }

        /// <summary>
        /// 获取根据主键阅读资料存储过程名称
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="id">对象ID</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(Type type, object id)
        {
            return GetFetchByIDProcedure(type, id, string.Empty);
        }

        /// <summary>
        /// 获取根据主键阅读资料存储过程名称
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(Type type, string accessName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            Procedure fetchAllProcedure = GetFetchAllProcedure(type, accessName);
            Parameter item = buildParameter(type, objectInfo.IdInfo);
            fetchAllProcedure.Parameters.Add(item);
            return fetchAllProcedure;
        }

        /// <summary>
        /// 获取根据主键阅读资料存储过程名称
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="id">对象ID</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(object obj, object id, string accessName)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj;不能为 Null");
            }
            if (id == null)
            {
                throw new ArgumentException("参数:'id'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(obj.GetType());
            Procedure fetchAllProcedure = GetFetchAllProcedure(obj, accessName);
            Parameter item = buildParameter(obj, objectInfo.IdInfo);
            item.Value = id;
            fetchAllProcedure.Parameters.Add(item);
            return fetchAllProcedure;
        }

        /// <summary>
        /// 获取根据主键阅读资料存储过程名称
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="id">对象ID</param>
        /// <param name="accessName">存储过程名称</param>
        /// <returns>返回读取的存储过程</returns>
        public static Procedure GetFetchByIDProcedure(Type type, object id, string accessName)
        {
            if (type == null)
            {
                throw new ArgumentException("参数:'type'不能为 Null");
            }
            if (id == null)
            {
                throw new ArgumentException("参数:'id'不能为 Null");
            }
            if (accessName == null)
            {
                throw new ArgumentException("参数:'accessName'不能为 Null");
            }
            ObjectInfo objectInfo = TableInfoCache.GetObjectInfo(type);
            Procedure fetchAllProcedure = GetFetchAllProcedure(type, accessName);
            Parameter item = buildParameter(type, objectInfo.IdInfo);
            item.Value = id;
            fetchAllProcedure.Parameters.Add(item);
            return fetchAllProcedure;
        }

        /// <summary>
        /// 检查参数名称数否存在
        /// </summary>
        /// <param name="procedure">存储工程</param>
        /// <param name="parameterName">存储过程名称</param>
        /// <returns>true=存在 false=不存在</returns>
        private static bool isContainParameter(Procedure procedure, string parameterName)
        {
            foreach (Parameter parameter in procedure.Parameters)
            {
                if (parameter.Name == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion --- 获取存储过程名称 End ---

    }
}
