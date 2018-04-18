using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using MTFramework.SimpleDataProxy.Interface;


namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// 多个数据库访问池
    /// </summary>
    public sealed class DataProxyPool
    {
        private static Dictionary<string, DataProxy> dicDataProxy = new Dictionary<string, DataProxy>();

        /// <summary>
        /// 设置数据库连接对象及连接字符串
        /// </summary>
        /// <param name="assemblyName">实现类文件全名 如：MT.Framework.Ado.Core.dll</param>
        /// <param name="classTypeName">实现类全名 如：MT.Framework.Ado.Core.MySql5Controller</param>
        /// <param name="dbKey">数据库Key</param>
        /// <param name="connStr">连接字符串</param>
        public static void SetConnection(string assemblyName, string classTypeName, string dbKey, string connStr)
        {
            dicDataProxy.Add(dbKey, DataProxy.SetConnection(assemblyName, classTypeName, connStr));
        }

        /// <summary>
        /// 设置数据库连接对象及连接字符串
        /// </summary>
        /// <param name="dbController">数据库操作对象类型</param>
        /// <param name="dbKey">数据库Key</param>
        /// <param name="connStr">连接字符串</param>
        public static void SetConnection<T>(T dbController, string dbKey, string connStr) where T : IDbController
        {
            dicDataProxy.Add(dbKey, DataProxy.SetConnection(dbController, connStr));
        }

        /// <summary>
        /// 网站程序获取实例
        /// <para>&lt;connectionStrings&gt;</para>  
        /// <para>&lt;add name=&quot;ConnStr&quot; connectionString=&quot;Server=10.200.2.32;UserId=root;Password=root;Database=terminalserverdb;&quot;/&gt;</para>
        /// <para>&lt;/connectionStrings&gt;</para>
        /// </summary>
        /// <param name="assemblyName">实现类文件全名 如：MT.Framework.Ado.Core.dll</param>
        /// <param name="ClassTypeName">实现类全名 如：MT.Framework.Ado.Core.MySql5Controller</param>
        /// <param name="dbKey">数据库Key</param>
        /// <param name="connectionStringsNames">DOM设置连接字符串</param>
        public static void DOMSetConnection(string assemblyName, string ClassTypeName, string dbKey, string connectionStringsNames)
        {
            dicDataProxy.Add(dbKey, DataProxy.DOMSetConnection(assemblyName, ClassTypeName, connectionStringsNames));
        }

        /// <summary>
        /// 网站程序获取实例
        /// <para>&lt;connectionStrings&gt;</para>  
        /// <para>&lt;add name=&quot;ConnStr&quot; connectionString=&quot;Server=10.200.2.32;UserId=root;Password=root;Database=terminalserverdb;&quot;/&gt;</para>
        /// <para>&lt;/connectionStrings&gt;</para>
        /// </summary>
        /// <param name="dbController">数据库操作对象类型</param>
        /// <param name="dbKey">数据库Key</param>
        /// <param name="connectionStringsNames">DOM设置连接字符串</param>
        public static void DOMSetConnection<T>(T dbController, string dbKey, string connectionStringsNames) where T : IDbController
        {
            dicDataProxy.Add(dbKey, DataProxy.DOMSetConnection(dbController, connectionStringsNames));
        }

        /// <summary>
        /// 读取数据库代理
        /// </summary>
        public static Dictionary<string, DataProxy> GetDataProxy
        {
            get
            {
                return dicDataProxy;
            }
        }
    }

    /// <summary>
    /// 单个数据库操作代理
    /// </summary>
    public class DataProxy
    {
        private static DataProxy dataProxy = null;

        private IDbController _IDb = null;

        #region --- 设置事例 Begin ---

        /// <summary>
        /// 私有构造方法
        /// </summary>
        private DataProxy()
        {

        }

        /// <summary>
        /// 获取当前对象
        /// </summary>
        public static DataProxy DataProxyOnec
        {
            get
            {
                return dataProxy;
            }
        }

        /// <summary>
        /// 设置数据库连接对象及连接字符串
        /// </summary>
        /// <param name="assemblyName">实现类文件全名 如：MT.Framework.Ado.Core.dll</param>
        /// <param name="ClassTypeName">实现类全名 如：MT.Framework.Ado.Core.MySql5Controller</param>
        /// <param name="connStr">连接字符串</param> 
        /// <returns>返回实例对象</returns>
        public static DataProxy SetConnection(string assemblyName, string ClassTypeName, string connStr)
        {
            dataProxy = new DataProxy();
            dataProxy._IDb = (IDbController)Assembly.LoadFrom(assemblyName).CreateInstance(ClassTypeName);
            dataProxy._IDb.ConnectionString = connStr;
            dataProxy.Connection();

            return dataProxy;
        }

        /// <summary>
        /// 设置数据库连接对象及连接字符串
        /// </summary>
        /// <param name="dbController">数据库操作对象类型</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns>返回实例对象</returns>
        public static DataProxy SetConnection<T>(T dbController, string connStr) where T : IDbController
        {

            dataProxy = new DataProxy();
            dataProxy._IDb = dbController;
            dataProxy._IDb.ConnectionString = connStr;
            dataProxy.Connection();

            return dataProxy;
        }

        /// <summary>
        /// 网站程序获取实例
        /// <para>&lt;connectionStrings&gt;</para>  
        /// <para>&lt;add name=&quot;ConnStr&quot; connectionString=&quot;Server=10.200.2.32;UserId=root;Password=root;Database=terminalserverdb;&quot;/&gt;</para>
        /// <para>&lt;/connectionStrings&gt;</para>
        /// </summary>
        /// <param name="assemblyName">实现类文件全名 如：MT.Framework.Ado.Core.dll</param>
        /// <param name="ClassTypeName">实现类全名 如：MT.Framework.Ado.Core.MySql5Controller</param>
        /// <param name="connectionStringsNames">DOM设置连接字符串</param>      
        /// <returns>返回实例对象</returns>        
        public static DataProxy DOMSetConnection(string assemblyName, string ClassTypeName, string connectionStringsNames)
        {
            /*
             * <connectionStrings>
             * <add name="MySQL" connectionString="Server=127.0.0.1;UserId=root;Password=root;Database=db;"/>
             * </connectionStrings>
             */

            dataProxy = new DataProxy();
            dataProxy._IDb = (IDbController)Assembly.LoadFrom(assemblyName).CreateInstance(ClassTypeName);
            dataProxy._IDb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringsNames].ConnectionString;
            dataProxy.Connection();

            return dataProxy;
        }

        /// <summary>
        /// 网站程序获取实例
        /// <para>&lt;connectionStrings&gt;</para>  
        /// <para>&lt;add name=&quot;ConnStr&quot; connectionString=&quot;Server=10.200.2.32;UserId=root;Password=root;Database=terminalserverdb;&quot;/&gt;</para>
        /// <para>&lt;/connectionStrings&gt;</para>
        /// </summary>
        /// <param name="dbController">数据库操作对象类型</param>
        /// <param name="connectionStringsNames">DOM设置连接字符串</param>
        /// <returns>返回实例对象</returns>
        public static DataProxy DOMSetConnection<T>(T dbController, string connectionStringsNames) where T : IDbController
        {
            /*
             * <connectionStrings>
             * <add name="MySQL" connectionString="Server=127.0.0.1;UserId=root;Password=root;Database=db;"/>
             * </connectionStrings>
             */

            dataProxy = new DataProxy();
            dataProxy._IDb = dbController;
            dataProxy._IDb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringsNames].ConnectionString;
            dataProxy.Connection();

            return dataProxy;
        }

        #endregion --- 设置事例 End ---

        #region --- 数据库操作对象 Begin ---

        /// <summary>
        /// 获取当前数据库连接对象
        /// </summary>
        /// <returns></returns>
        public DbConnection Connection()
        {
            return _IDb.GetDbConnection;
        }

        /// <summary>
        /// 当前数据库执行对象
        /// </summary>
        /// <returns></returns>
        public DbCommand Command()
        {
            return _IDb.GetDbCommand;
        }

        /// <summary>
        /// 获取数据库适配器
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter DataAdapter()
        {
            return _IDb.GetDbDataAdapter;
        }

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public ConnectionState DbState
        {
            get
            {
                return _IDb.GetDbConnectionState;
            }
        }

        #endregion --- 数据库操作对象 End ---

        #region --- 数据库连接 Begin ---

        /// <summary>
        /// 打开数据库链接
        /// </summary>
        /// <returns></returns>
        public bool DbOpen()
        {
            if (DbState == ConnectionState.Open)
            {
                return true;
            }
            _IDb.GetDbConnection.Open();
            return true;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void DbClose()
        {
            if (DbState == ConnectionState.Open)
            {
                _IDb.GetDbConnection.Close();
            }
        }

        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        /// <returns></returns>
        public DbConnection GetDbConnection()
        {
            return _IDb.GetDbConnection;
        }

        #endregion --- 数据库连接 End ---

        #region --- 数据查询 Begin ---

        /// <summary>
        /// 用于运行SQL语句的方法 注意：如果搜索结果为空的话返回值为0
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <returns>返回值为 Int32 值 </returns>
        public int ExecuteScalar(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    int i = 0;
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    Object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        i = (int)result;
                    }

                    return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询单个表
        /// </summary>
        /// <param name="sql">执行语句</param>   
        /// <returns>返回查询表，不成功返回 Null</returns>
        public DataTable QueryToTable(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询多个表
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>返回多个表 不成功返回 Null </returns>
        public DataTable[] QueryToTables(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    DataTable[] temps = new DataTable[ds.Tables.Count];

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        temps[i] = ds.Tables[i];
                    }

                    return temps;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询单个表
        /// </summary>
        /// <param name="sql">执行语句</param>   
        /// <returns>返回查询表，不成功返回 Null</returns>
        public SimpleDataTable QueryToSimpleDataTable(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return this.dataTableToSimpleDataTable(ds.Tables[0]);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询多个表
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>返回多个表 不成功返回 Null </returns>
        public SimpleDataTable[] QueryToSimpleDataTables(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    SimpleDataTable[] sdts = new SimpleDataTable[ds.Tables.Count];
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        sdts[i] = this.dataTableToSimpleDataTable(ds.Tables[i]);
                    }
                    return sdts;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询 DataSet
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>返回DataSet 失败返回Null </returns>
        public DataSet QueryDataSet(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return ds;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询一个值
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>返回Object 失败返回 Null</returns>
        public DataRow QueryOnec(string sql)
        {
            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = sql;
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return ds.Tables[0].Rows[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        #endregion --- 数据查询 End ---

        #region --- 执行语句 增 删 改 Begin ---

        /// <summary>
        /// 执行一条SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回影响行数</returns>
        public int ExecuteCommand(string sql)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.Transaction = tra;
                cmd.CommandText = sql;
                int res = cmd.ExecuteNonQuery();

                tra.Commit();
                return res;
            }
            catch (Exception ex)
            {
                if (tra != null)
                {
                    tra.Rollback();
                }
                throw ex;
            }
            finally
            {
                DbClose();
            }

        }

        /// <summary>
        /// 执行多条SQL语句
        /// </summary>
        /// <param name="sqlArray">sql语句</param>
        /// <returns>返回影响行数</returns>
        public void ExecuteCommand(params string[] sqlArray)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();
                for (int i = 0; i < sqlArray.Length; i++)
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.Transaction = tra;
                    cmd.CommandText = sqlArray[i];
                    cmd.ExecuteNonQuery();
                }
                tra.Commit();
            }
            catch (Exception ex)
            {
                if (tra != null)
                {
                    tra.Rollback();
                }
                throw ex;
            }
            finally
            {
                DbClose();
            }

        }

        #endregion --- 执行语句 增 删 改 End ---

        #region --- 执行存储过程 Begin ---

        /// <summary>
        /// 执行存储过程获取单个数据表
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns>返回单个结果表 失败返回Null</returns>
        public DataTable QueryExecuteProcToDataTable(Procedure proc)
        {
            try
            {

                DbOpen();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;

                foreach (var item in proc.Parameters)
                {
                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取单个数据表
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回单个结果表 失败返回Null</returns>
        public DataTable QueryExecuteProcToDataTable(string procCmd, params Parameter[] parameters)
        {
            try
            {
                DbOpen();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                }
                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取多个数据表
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns>返回多个结果表 失败返回Null</returns>
        public DataTable[] QueryExecuteProcToDataTables(Procedure proc)
        {
            try
            {
                DbOpen();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;

                foreach (var item in proc.Parameters)
                {
                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable[] temps = new DataTable[ds.Tables.Count];
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    temps[i] = ds.Tables[i];
                }
                return temps;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取多个数据表
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回多个结果表 失败返回Null</returns>
        public DataTable[] QueryExecuteProcToDataTables(string procCmd, params Parameter[] parameters)
        {
            try
            {
                DbOpen();

                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable[] temps = new DataTable[ds.Tables.Count];

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    temps[i] = ds.Tables[i];
                }

                return temps;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取单个数据表
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns>返回单个结果表 失败返回Null</returns>
        public SimpleDataTable QueryExecuteProcToSimpleDataTable(Procedure proc)
        {
            try
            {

                DbOpen();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;

                foreach (var item in proc.Parameters)
                {
                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return this.dataTableToSimpleDataTable(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取单个数据表
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回单个结果表 失败返回Null</returns>
        public SimpleDataTable QueryExecuteProcToSimpleDataTable(string procCmd, params Parameter[] parameters)
        {
            try
            {
                DbOpen();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                }
                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);


                return this.dataTableToSimpleDataTable(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取多个数据表
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns>返回多个结果表 失败返回Null</returns>
        public SimpleDataTable[] QueryExecuteProcToSimpleDataTables(Procedure proc)
        {
            try
            {
                DbOpen();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;

                foreach (var item in proc.Parameters)
                {
                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                SimpleDataTable[] sdts = new SimpleDataTable[ds.Tables.Count];
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    sdts[i] = this.dataTableToSimpleDataTable(ds.Tables[i]);
                }
                return sdts;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取多个数据表
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回多个结果表 失败返回Null</returns>
        public SimpleDataTable[] QueryExecuteProcToSimpleDataTables(string procCmd, params Parameter[] parameters)
        {
            try
            {
                DbOpen();

                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                SimpleDataTable[] sdts = new SimpleDataTable[ds.Tables.Count];
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    sdts[i] = this.dataTableToSimpleDataTable(ds.Tables[i]);
                }
                return sdts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取DataSet
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns>返回DataSet 失败返回Null</returns>
        public DataSet QueryExecuteProcToDataSet(Procedure proc)
        {
            try
            {
                DbOpen();

                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;

                foreach (var item in proc.Parameters)
                {
                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                }

                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程获取DataSet
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回DataSet 失败返回Null</returns>
        public DataSet QueryExecuteProcToDataSet(string procCmd, params Parameter[] parameters)
        {
            try
            {
                DbOpen();

                DbCommand cmd = _IDb.GetDbCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                }
                DbDataAdapter adp = _IDb.GetDbDataAdapter;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <param name="outParams">返回输出参数</param>
        /// <returns></returns>
        public int ExecuteProc(Procedure proc, out List<Parameter> outParams)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.Transaction = tra;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;

                outParams = new List<Parameter>();
                foreach (var item in proc.Parameters)
                {
                    switch (item.Direction)
                    {
                        case ParameterDirection.Output:
                            outParams.Add(item);
                            cmd.Parameters.Add(_IDb.CreateDbParameterOutput(item.Name));
                            break;
                        case ParameterDirection.ReturnValue:
                            outParams.Add(item);
                            cmd.Parameters.Add(_IDb.CreateDbParameterReturnValue());
                            break;
                        default:
                            if (item.Direction == ParameterDirection.InputOutput)
                            {
                                outParams.Add(item);
                            }
                            cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                            break;
                    }
                }
                int re = cmd.ExecuteNonQuery();
                foreach (Parameter param in outParams)
                {
                    foreach (DbParameter dbparam in cmd.Parameters)
                    {
                        if (dbparam.ParameterName.Equals(param.Name) == true)
                        {
                            param.Value = dbparam.Value;
                        }
                    }
                }
                tra.Commit();
                return re;
            }
            catch (Exception ex)
            {
                if (tra != null)
                {
                    tra.Rollback();
                }
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns></returns>
        public int ExecuteProc(Procedure proc)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.Transaction = tra;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Text;
                cmd.CommandTimeout = proc.CommandTimeOut;
                foreach (var item in proc.Parameters)
                {
                    switch (item.Direction)
                    {
                        case ParameterDirection.Output:
                            cmd.Parameters.Add(_IDb.CreateDbParameterOutput(item.Name));
                            break;
                        case ParameterDirection.ReturnValue:
                            cmd.Parameters.Add(_IDb.CreateDbParameterReturnValue());
                            break;
                        default:
                            cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                            break;
                    }
                }
                int re = cmd.ExecuteNonQuery();

                tra.Commit();
                return re;
            }
            catch (Exception ex)
            {
                if (tra != null)
                {
                    tra.Rollback();
                }
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="outParams">返回输出参数</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public int ExecuteProc(string procCmd, out List<Parameter> outParams, params Parameter[] parameters)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.Transaction = tra;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;
                outParams = new List<Parameter>();
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        switch (item.Direction)
                        {
                            case ParameterDirection.Output:
                                outParams.Add(item);
                                cmd.Parameters.Add(_IDb.CreateDbParameterOutput(item.Name));
                                break;
                            case ParameterDirection.ReturnValue:
                                outParams.Add(item);
                                cmd.Parameters.Add(_IDb.CreateDbParameterReturnValue());
                                break;
                            default:
                                if (item.Direction == ParameterDirection.InputOutput)
                                {
                                    outParams.Add(item);
                                }
                                cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                                break;
                        }
                    }
                }
                int re = cmd.ExecuteNonQuery();
                foreach (Parameter param in outParams)
                {
                    foreach (DbParameter dbparam in cmd.Parameters)
                    {
                        if (dbparam.ParameterName.Equals(param.Name) == true)
                        {
                            param.Value = dbparam.Value;
                        }
                    }
                }
                tra.Commit();
                return re;
            }
            catch (Exception ex)
            {
                if (tra != null)
                {
                    tra.Rollback();
                }
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public int ExecuteProc(string procCmd, params Parameter[] parameters)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();
                DbCommand cmd = _IDb.GetDbCommand;
                cmd.Transaction = tra;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procCmd;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        switch (item.Direction)
                        {
                            case ParameterDirection.Output:
                                cmd.Parameters.Add(_IDb.CreateDbParameterOutput(item.Name));
                                break;
                            case ParameterDirection.ReturnValue:
                                cmd.Parameters.Add(_IDb.CreateDbParameterReturnValue());
                                break;
                            default:
                                cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                                break;
                        }
                    }
                }
                int re = cmd.ExecuteNonQuery();

                tra.Commit();
                return re;
            }
            catch (Exception ex)
            {
                if (tra != null)
                {
                    tra.Rollback();
                }
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 执行多个存储过程
        /// </summary>
        /// <param name="procs">多个存储过程对象</param>
        /// <returns></returns>
        public void ExecuteProc(params Procedure[] procs)
        {
            DbTransaction tra = null;
            try
            {
                DbOpen();
                tra = _IDb.GetDbConnection.BeginTransaction();

                foreach (var proc in procs)
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.Transaction = tra;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc.Text;

                    foreach (var item in proc.Parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                    }
                    cmd.ExecuteNonQuery();
                }
                tra.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询一个值
        /// </summary>
        /// <param name="proc">存储过程</param>
        /// <returns>返回Object 失败返回 Null</returns>
        public DataRow QueryExecuteProcOnec(Procedure proc)
        {

            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandText = proc.Text;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = proc.CommandTimeOut;

                    foreach (var item in proc.Parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    return ds.Tables[0].Rows[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 查询一个值
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回Object 失败返回 Null</returns>
        public DataRow QueryExecuteProcOnec(string procCmd, params Parameter[] parameters)
        {

            try
            {
                if (DbOpen())
                {
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procCmd;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                        }
                    }
                    DbDataAdapter adp = _IDb.GetDbDataAdapter;
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    return ds.Tables[0].Rows[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 用于运行SQL语句的方法 注意：如果搜索结果为空的话返回值为0
        /// </summary>
        /// <param name="proc">存储过程</param>
        /// <param name="outParams">返回输出参数</param>
        /// <returns>返回值为 Int32 值 </returns>
        public int ExecuteScalar(Procedure proc, out List<Parameter> outParams)
        {

            try
            {
                outParams = new List<Parameter>();
                if (DbOpen())
                {
                    int i = 0;
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc.Text;
                    cmd.CommandTimeout = proc.CommandTimeOut;
                    foreach (var item in proc.Parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                    Object result = cmd.ExecuteScalar();
                    foreach (Parameter param in outParams)
                    {
                        foreach (DbParameter dbparam in cmd.Parameters)
                        {
                            if (dbparam.ParameterName.Equals(param.Name) == true)
                            {
                                param.Value = dbparam.Value;
                            }
                        }
                    }
                    if (result != null)
                    {
                        i = (int)result;
                    }

                    return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 用于运行SQL语句的方法 注意：如果搜索结果为空的话返回值为0
        /// </summary>
        /// <param name="proc">存储过程</param>
        /// <returns>返回值为 Int32 值 </returns>
        public int ExecuteScalar(Procedure proc)
        {

            try
            {
                if (DbOpen())
                {
                    int i = 0;
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc.Text;
                    cmd.CommandTimeout = proc.CommandTimeOut;
                    foreach (var item in proc.Parameters)
                    {
                        cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, ParameterDirection.Input));
                    }
                    Object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        i = (int)result;
                    }

                    return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 用于运行SQL语句的方法 注意：如果搜索结果为空的话返回值为0
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="outParams">返回输出参数</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回值为 Int32 值 </returns>
        public int ExecuteScalar(string procCmd, out List<Parameter> outParams, params Parameter[] parameters)
        {
            outParams = new List<Parameter>();
            try
            {
                if (DbOpen())
                {
                    int i = 0;
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procCmd;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            switch (item.Direction)
                            {

                                case ParameterDirection.Output:
                                    outParams.Add(item);
                                    cmd.Parameters.Add(_IDb.CreateDbParameterOutput(item.Name));
                                    break;
                                case ParameterDirection.ReturnValue:
                                    outParams.Add(item);
                                    cmd.Parameters.Add(_IDb.CreateDbParameterReturnValue());
                                    break;
                                default:
                                    if (item.Direction == ParameterDirection.InputOutput)
                                    {
                                        outParams.Add(item);
                                    }
                                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                                    break;
                            }
                        }
                    }
                    Object result = cmd.ExecuteScalar();
                    foreach (Parameter param in outParams)
                    {
                        foreach (DbParameter dbparam in cmd.Parameters)
                        {
                            if (dbparam.ParameterName.Equals(param.Name) == true)
                            {
                                param.Value = dbparam.Value;
                            }
                        }
                    }
                    if (result != null)
                    {
                        i = (int)result;
                    }

                    return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                DbClose();
            }
        }

        /// <summary>
        /// 用于运行SQL语句的方法 注意：如果搜索结果为空的话返回值为0
        /// </summary>
        /// <param name="procCmd">存储过程命令</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回值为 Int32 值 </returns>
        public int ExecuteScalar(string procCmd, params Parameter[] parameters)
        {
            try
            {
                if (DbOpen())
                {
                    int i = 0;
                    DbCommand cmd = _IDb.GetDbCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procCmd;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            switch (item.Direction)
                            {

                                case ParameterDirection.Output:
                                    cmd.Parameters.Add(_IDb.CreateDbParameterOutput(item.Name));
                                    break;
                                case ParameterDirection.ReturnValue:
                                    cmd.Parameters.Add(_IDb.CreateDbParameterReturnValue());
                                    break;
                                default:
                                    cmd.Parameters.Add(_IDb.CreateDbParameter(item.Name, item.Value, item.Direction));
                                    break;
                            }
                        }
                    }
                    Object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        i = (int)result;
                    }

                    return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                DbClose();
            }
        }

        #endregion --- 执行存储过程 End ---

        #region --- 私有辅助方法 Begin ---

        /// <summary>
        /// DataTable转换到SimpleDataTable
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>返回SimpleDataTable</returns>
        private SimpleDataTable dataTableToSimpleDataTable(DataTable dt)
        {
            SimpleDataTable sdt = new SimpleDataTable();
            foreach (DataColumn col in dt.Columns)
            {
                sdt.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow dr in dt.Rows)
            {
                SimpleDataRow sdr = sdt.CreateRow();
                foreach (SimpleDataColumn col in sdt.Columns)
                {
                    sdr[col.ColumnName] = dr[col.ColumnName];
                }
                sdt.Rows.Add(sdr);
            }
            return sdt;
        }

        #endregion --- 私有辅助方法 End ---
    }
}
