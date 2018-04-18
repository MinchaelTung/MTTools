using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Configuration;
using System.Data;

namespace Dao
{
    public class DBHelper
    {
        private DBController _DBController = null;
        //为了使用事务时更方便所以用protected让它可以被继承
        public DbConnection Connection = null;
        private DbDataReader dataReader = null;

        public DBHelper()
        {
            this._DBController = new DBController();
            this._DBController.ConnectionString = ConfigurationManager.ConnectionStrings["DbProxy"].ConnectionString;
        }

        public DBHelper(string controllerString)
        {
            this._DBController = new DBController(controllerString);
        }


        #region"Open 用于打开数据库的方法"
        /// <summary> 
        /// 用于打开SQL数据库 
        /// </summary> 
        /// <returns>返回类型为 Bool 类型</returns> 
        public bool Open()
        {
            bool answer = false;
            try
            {
                Connection = this._DBController.GetConnection();

                Connection.Open();
                answer = true;

            }
            catch (Exception e)
            {
                // Log.DisposeException(e);
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);

            }

            return answer;

        }
        #endregion

        #region"用于关闭连接"
        public void Close()
        {
            if (ConnState())
            {
                Connection.Close();
            }

            if (dataReader != null)
            {
                dataReader.Close();
            }

        }
        #endregion

        #region"ConnState 用于返回当前连接的状态"
        private bool ConnState()
        {
            if (Connection.State.ToString().Equals("Open"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region --- Parameter Begin ---

        public DbParameter GetParameter(string parameterName, object vlaue)
        {
            return this._DBController.GetParameter(parameterName,vlaue);
        }

        #endregion --- Parameter End ---


        #region"ExecuteScalar 用于返回单个结果的SQL查询语句"
        /// <summary> 
        /// 用于运行SQL语句的方法 注意：如果搜索结果为空的话返回值为0
        /// 返回值为一个int 
        /// </summary> 
        /// <param name="SQL">要运行的 SQL 语句</param> 
        /// <returns>返回值为 int 类型 </returns> 
        public int ExecuteScalar(string SQL)
        {
            try
            {
                if (Open())
                {
                    int i = 0;
                    DbCommand cmd1 = Connection.CreateCommand();
                    cmd1.CommandText = SQL;
                    Object result = cmd1.ExecuteScalar();

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
                // Log.DisposeException(e);
                Console.WriteLine(e.Message);
                throw new Exception("(ExecuteScalar方法发生错误)" + e.Message);

            }
            finally
            {
                Close();
            }
        }
        #endregion



        #region GetTable方法
        /// <summary>
        /// GetTable方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTable(string selectText, string tableName)
        {
            try
            {
                if (Open())
                {
                    DbDataAdapter adp = this._DBController.GetDbDataAdapter(selectText, this.Connection);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, tableName);
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("(GetTable方法发生错误)" + e.Message);
            }
            finally
            {
                Close();
            }
        }
        #endregion



        #region GetTableByPages方法
        /// <summary>
        /// GetTableByPages方法
        /// </summary>
        /// <param name="selectText"></param>
        /// <param name="startIndex"></param>
        /// <param name="pagesize"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTableByPages(string selectText, int startIndex, int pagesize, string tableName)
        {
            try
            {
                DbDataAdapter adp = this._DBController.GetDbDataAdapter(selectText, Connection);
                DataSet ds = new DataSet();
                adp.Fill(ds, startIndex, pagesize, tableName);
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("(GetTableByPages方法发生错误)" + e.Message);
            }
            finally
            {
                Close();
            }
        }
        #endregion



        #region"GetReader 用于返回单行结果的SQL查询语句,调用时必须手动调用CLOSE方法关闭连接及Reader对象"
        /// <summary> 
        /// 用于运行SQL语句的方法 
        /// 返回值为一个SqlDataReader 
        /// </summary> 
        /// <param name="SQL">要运行的 SQL 语句</param> 
        /// <returns>返回值为 OleDbDataReader 类型 </returns> 
        public DbDataReader GetReader(string SQL)
        {
            try
            {
                if (Open())
                {
                    DbCommand cmd1 = Connection.CreateCommand();
                    cmd1.CommandText = SQL;
                    dataReader = cmd1.ExecuteReader(CommandBehavior.CloseConnection);

                    return dataReader;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Close();
                //  Log.DisposeException(e);
                Console.WriteLine(e.Message);
                throw new Exception("(GetReader方法发生错误)" + e.Message);
            }
        }
        #endregion





        #region "GetDataSet 用于运行SQL语句的方法"
        /// <summary> 
        /// 用于运行SQL语句的方法 
        /// 返回值为一个DataSet 
        /// 注意此方法与 RunSQLcmd 的不同 
        /// </summary> 
        /// <param name="SQL">要运行的 SQL 语句</param> 
        /// <returns>返回值为 DataSet 类型 </returns> 
        public DataSet GetDataSet(string cmdText)
        {
            try
            {
                if (Open())
                {
                    DataSet MyDataSet = new DataSet();
                    DbDataAdapter sqlDb = this._DBController.GetDbDataAdapter();
                    sqlDb.SelectCommand = this._DBController.GetDbCommand(cmdText, Connection);
                    sqlDb.Fill(MyDataSet);
                    return MyDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                //  Log.DisposeException(e);
                Console.WriteLine(e.Message);
                throw new Exception("(GetDataSet方法发生错误)" + e.Message);
            }
            finally
            {
                Close();
            }
        }
        #endregion



        #region"ExecuteCommand 用于运行SQL语句的增、删、改方法"
        /// <summary> 
        /// 用于运行SQL语句的方法 
        /// 返回值为一个int(该次操作影响的行数)
        /// </summary> 
        /// <param name="SQL">要运行的 SQL 语句</param> 
        /// <returns>返回值为 bool 类型 </returns> 
        public int ExecuteCommand(string SQL)
        {
            try
            {
                if (Open())
                {
                    DbCommand cmd1 = Connection.CreateCommand();
                    cmd1.CommandText = SQL;
                    return cmd1.ExecuteNonQuery();//运行SQL语句
                }
                else
                {
                    throw new Exception("数据库连接未能打开");
                }
            }
            catch (Exception e)
            {
                //  Log.DisposeException(e);
                throw new Exception("(ExecuteCommand方法发生错误)" + e.Message);
            }
            finally
            {
                Close();
            }
        }
        #endregion


        #region
        /// <summary>
        /// 修改（添加、更新、删除）数据
        /// </summary>
        /// <param name="sql">存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>bool true=成功</returns>
        public bool Modify(String[] sql)
        {
            Open();
            DbTransaction tran;
            DbCommand cmd = Connection.CreateCommand();

            tran = Connection.BeginTransaction();
            cmd.Transaction = tran;
            try
            {
                for (int i = 0; i < sql.Length; i++)
                {
                    cmd.CommandText = sql[i];  //手动为cmd付SQL语句
                    cmd.ExecuteNonQuery();  //手动调用

                }

                tran.Commit(); //提交事务
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw new Exception("InsertAlbum发生错误，事务已回滚。" + e.Message);
            }
            finally
            {
                Close();
            }

            return true;

        }
        #endregion
    }
}
