using System;
using System.Collections.Generic;
using System.Data;
using MTFramework.SimpleDataProxy.ORM.Common;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// 作者：Michael Tung (佟卓威)
    /// 时间：2010/10/4 10:01:21
    /// 公司: D-Man
    /// 版权：2010-2020
    /// CLR版本：4.0.30319.1
    /// 唯一标识：8316b98c-d162-45e7-9740-5292e69737a2
    /// MT.Framework.Data.WebDataAccessProxy.ORM.FetchDataMethod 说明：反射填充实体对象工具
    /// </summary>
    public class FetchDataMethod
    {
        private const string PropertyDataNotFound = "结果中没有: {0} 数据";
        private const string MarkedFieldOfMultiFieldAttributeCanNotIsNull = "在MultiFieldAttribute标记字段不能为空.";

        #region --- Ctors Begin ---
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FetchDataMethod()
        {
        }

        #endregion --- Ctors End ---

        #region --- Functions Begin ---

        /// <summary>
        /// 读取数据库资料信息操作
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="dr">SimpleDataRow</param>
        public static void MapData(object obj, SimpleDataRow dr)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'object'不能为 Null");
            }
            if (dr == null)
            {
                throw new ArgumentException("参数:'SimpleDataRow'不能为 Null");
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
        /// <param name="obj">object</param>
        /// <param name="dr">DataRow</param>
        public static void MapData(object obj, DataRow dr)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'object'不能为 Null");
            }
            if (dr == null)
            {
                throw new ArgumentException("参数:'DataRow'不能为 Null");
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
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dr">SimpleDataRow</param>
        /// <returns>返回泛型</returns>
        public static T MapData<T>(SimpleDataRow dr)
            where T : new()
        {
            T local = (T)MethodCaller.CreateInstance(typeof(T));
            MapData(local, dr);
            return local;
        }

        /// <summary>
        /// 读取数据库资料信息操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dr">DataRow</param>
        /// <returns>返回泛型</returns>
        public static T MapData<T>(DataRow dr)
            where T : new()
        {
            T local = (T)MethodCaller.CreateInstance(typeof(T));
            MapData(local, dr);
            return local;
        }

        /// <summary>
        /// 读取数据库资料填充实体集合
        /// </summary>
        /// <typeparam name="T">实体集合</typeparam>
        /// <typeparam name="V">实体类</typeparam>
        /// <param name="tList">实体集合对象</param>
        /// <param name="td">SimpleDataTable</param>
        public static void MapData<T, V>(T tList, SimpleDataTable td)
            where T : IList<V>
            where V : new()
        {
            foreach (SimpleDataRow dr in td.Rows)
            {
                V item = FetchDataMethod.MapData<V>(dr);
                tList.Add(item);
            }

        }


        /// <summary>
        /// 读取数据库资料填充实体集合
        /// </summary>
        /// <typeparam name="T">实体集合</typeparam>
        /// <typeparam name="V">实体类</typeparam>
        /// <param name="tlist">实体集合对象</param>
        /// <param name="td">DataTable</param>
        public static void MapData<T, V>(T tlist, DataTable td)
            where T : IList<V>
            where V : new()
        {
            foreach (DataRow dr in td.Rows)
            {
                V item = FetchDataMethod.MapData<V>(dr);
                tlist.Add(item);
            }
        }


        #endregion --- Functions End ---
    }

}
