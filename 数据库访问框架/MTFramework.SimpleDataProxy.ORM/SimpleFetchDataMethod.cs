using System;
using System.Collections.Generic;
using System.Text;

namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// 自动对象数据填充方法
    /// </summary>
    public class SimpleFetchDataMethod
    {
        private static DataProxy _DataProxy = null;
        /// <summary>
        /// 连接数据库名称
        /// </summary>
        public static DataProxy DataProxy
        {
            set { _DataProxy = value; }
        }

        /// <summary>
        /// 填充对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>返回对象实例</returns>
        public static T Fetch<T>() where T : new()
        {
            T t = new T();
            Procedure procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T));
                        
            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            if (dt.Rows.Count != 0)
            {
                SimpleDataAccessORMUtility.MapData(t, dt.Rows[0]);
            }
            else
            {
                throw new Exception("Null");
            }
            return t;
        }

        /// <summary>
        /// 填充对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="accessName">操作名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回对象实例</returns>
        public static T Fetch<T>(string accessName, params Parameter[] parameters) where T : new()
        {
            T t = new T();
            Procedure procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T), accessName);

            foreach (Parameter paramete in parameters)
            {
                procedure.Parameters.Add(paramete);
            }
                      
            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            if (dt.Rows.Count != 0)
            {
                SimpleDataAccessORMUtility.MapData(t, dt.Rows[0]);
            }
            else
            {
                throw new Exception("Null");
            }
            return t;
        }

        /// <summary>
        /// 填充对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="accessName">操作名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回对象实例</returns>
        public static T Fetch<T>(string accessName, List<Parameter> parameters) where T : new()
        {
            T t = new T();
            Procedure procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T), accessName);

            foreach (Parameter paramete in parameters)
            {
                procedure.Parameters.Add(paramete);
            }
            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            if (dt.Rows.Count != 0)
            {
                SimpleDataAccessORMUtility.MapData(t, dt.Rows[0]);
            }
            else
            {
                throw new Exception("Null");
            }
            return t;
        }

        /// <summary>
        /// 填充对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回对象实例</returns>
        public static T Fetch<T>(params Parameter[] parameters) where T : new()
        {
            T t = new T();
            Procedure procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T));

            foreach (Parameter paramete in parameters)
            {
                procedure.Parameters.Add(paramete);
            }
            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            if (dt.Rows.Count != 0)
            {
                SimpleDataAccessORMUtility.MapData(t, dt.Rows[0]);
            }
            else
            {
                throw new Exception("Null");
            }
            return t;
        }

        /// <summary>
        /// 填充对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回对象实例</returns>
        public static T Fetch<T>(List<Parameter> parameters) where T : new()
        {
            T t = new T();
            Procedure procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T));

            foreach (Parameter paramete in parameters)
            {
                procedure.Parameters.Add(paramete);
            }

            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            if (dt.Rows.Count != 0)
            {
                SimpleDataAccessORMUtility.MapData(t, dt.Rows[0]);
            }
            else
            {
                throw new Exception("Null");
            }
            return t;
        }

        /// <summary>
        /// 填充对象集合
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <typeparam name="V">集合Item类型</typeparam>
        /// <param name="tList">装载结果对象</param>
        public static void Fetch<T, V>(T tList)
            where T : IList<V>
            where V : new()
        {
            Procedure procedure;
            try
            {
                procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T));
            }
            catch (Exception ex)
            {
                if (ex.Source == "MT.Framework.Data.ORM")
                {
                    procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(V));
                }
                else
                {
                    throw ex;
                }
            }
            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            foreach (SimpleDataRow dr in dt.Rows)
            {
                V v = new V();
                SimpleDataAccessORMUtility.MapData(v, dr);
                tList.Add(v);
            }
        }


        /// <summary>
        /// 填充对象集合
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <typeparam name="V">集合Item类型</typeparam>
        /// <param name="tList">装载结果对象</param>
        /// <param name="accessName">操作名称</param>
        public static void Fetch<T, V>(T tList, string accessName)
            where T : IList<V>
            where V : new()
        {
            Procedure procedure;
            try
            {
                procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T), accessName);
            }
            catch (Exception ex)
            {
                if (ex.Source == "MT.Framework.Data.ORM")
                {
                    procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(V), accessName);
                }
                else
                {
                    throw ex;
                }
            }

            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            foreach (SimpleDataRow dr in dt.Rows)
            {
                V v = new V();
                SimpleDataAccessORMUtility.MapData(v, dr);
                tList.Add(v);
            }
        }

        /// <summary>
        /// 填充对象集合
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <typeparam name="V">集合Item类型</typeparam>
        /// <param name="tList">装载结果对象</param>>
        /// <param name="parameters">存储过程参数</param>
        public static void Fetch<T, V>(T tList, params Parameter[] parameters)
            where T : IList<V>
            where V : new()
        {
            Procedure procedure;
            try
            {
                procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T));
            }
            catch (Exception ex)
            {
                if (ex.Source == "MT.Framework.Data.ORM")
                    procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(V));
                else
                    throw ex;
            }

            foreach (Parameter parameter in parameters)
            {
                procedure.Parameters.Add(parameter);
            }

            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            foreach (SimpleDataRow dr in dt.Rows)
            {
                V v = new V();
                SimpleDataAccessORMUtility.MapData(v, dr);
                tList.Add(v);
            }
        }

        /// <summary>
        /// 填充对象集合
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <typeparam name="V">集合Item类型</typeparam>
        /// <param name="tList">装载结果对象</param>
        /// <param name="parameters">存储过程参数</param>
        public static void Fetch<T, V>(T tList, List<Parameter> parameters)
            where T : IList<V>
            where V : new()
        {
            Procedure procedure;
            try
            {
                procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T));
            }
            catch (Exception ex)
            {
                if (ex.Source == "MT.Framework.Data.ORM")
                    procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(V));
                else
                    throw ex;
            }

            foreach (Parameter parameter in parameters)
            {
                procedure.Parameters.Add(parameter);
            }

            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            foreach (SimpleDataRow dr in dt.Rows)
            {
                V v = new V();
                SimpleDataAccessORMUtility.MapData(v, dr);
                tList.Add(v);
            }
        }

        /// <summary>
        /// 填充对象集合
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <typeparam name="V">集合Item类型</typeparam>
        /// <param name="tList">装载结果对象</param>
        /// <param name="accessName">操作名称</param>
        /// <param name="parameters">存储过程参数</param>
        public static void Fetch<T, V>(T tList, string accessName, params Parameter[] parameters)
            where T : IList<V>
            where V : new()
        {
            Procedure procedure;
            try
            {
                procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T), accessName);
            }
            catch (Exception ex)
            {
                if (ex.Source == "MT.Framework.Data.ORM")
                {
                    procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(V), accessName);
                }
                else
                {
                    throw ex;
                }
            }

            foreach (Parameter parameter in parameters)
            {
                procedure.Parameters.Add(parameter);
            }

            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            foreach (SimpleDataRow dr in dt.Rows)
            {
                V v = new V();
                SimpleDataAccessORMUtility.MapData(v, dr);
                tList.Add(v);
            }
        }

        /// <summary>
        /// 填充对象集合
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <typeparam name="V">集合Item类型</typeparam>
        /// <param name="tList">装载结果对象</param>
        /// <param name="accessName">操作名称</param>
        /// <param name="parameters">存储过程参数</param>
        public static void Fetch<T, V>(T tList, string accessName, List<Parameter> parameters)
            where T : IList<V>
            where V : new()
        {
            Procedure procedure;
            try
            {
                procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(T), accessName);
            }
            catch (Exception ex)
            {
                if (ex.Source == "MT.Framework.Data.ORM")
                {
                    procedure = SimpleDataAccessORMUtility.GetFetchAllProcedure(typeof(V), accessName);
                }
                else
                {
                    throw ex;
                }
            }

            foreach (Parameter parameter in parameters)
            {
                procedure.Parameters.Add(parameter);
            }

            SimpleDataTable dt = _DataProxy.QueryExecuteProcToSimpleDataTable(procedure);
            foreach (SimpleDataRow dr in dt.Rows)
            {
                V v = new V();
                SimpleDataAccessORMUtility.MapData(v, dr);
                tList.Add(v);
            }
        }
    }
}
