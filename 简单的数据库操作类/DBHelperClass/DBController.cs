using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Dao
{
    public class DBController
    {
        #region --- Fields  Begin ---


        private string _ConnectionString = string.Empty;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }


        #endregion --- Fields  End ---

        #region --- Ctors Begin ---
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DBController()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据连接字符串</param>
        public DBController(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion --- Ctors End ---

        #region --- Function Begin ---
        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                throw new Exception("数据库连接字符串不能为空");
            }

            return new MySqlConnection(this._ConnectionString);
        }

        /// <summary>
        /// 获取数据执行对象
        /// </summary>
        /// <returns></returns>
        public DbCommand GetDbCommand()
        {            
            return new MySqlCommand();
        }

        /// <summary>
        /// 获取数据执行对象
        /// </summary>
        /// <param name="cmdText">执行命令文本</param>
        /// <returns></returns>
        public DbCommand GetDbCommand(string cmdText)
        {
            return new MySqlCommand(cmdText);
        }

        /// <summary>
        /// 获取数据执行对象
        /// </summary>
        /// <param name="cmdText">执行命令文本</param>
        /// <param name="connection">数据连接</param>
        /// <returns></returns>
        public DbCommand GetDbCommand(string cmdText, DbConnection connection)
        {
            if (connection is MySqlConnection)
            {
                return new MySqlCommand(cmdText, (MySqlConnection)connection);
            }
            else
            {
                throw new Exception("Connection 错误");
            }
        }

        /// <summary>
        /// 数据执行参数
        /// </summary>
        /// <returns></returns>
        public DbParameter GetDbParameter()
        {
            return new MySqlParameter();
        }

        public DbDataAdapter GetDbDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        public DbDataAdapter GetDbDataAdapter(string selectText, DbConnection connection)
        {
            if (connection is MySqlConnection)
            {
                return new MySqlDataAdapter(selectText, (MySqlConnection)connection);
            }
            else
            {
                throw new Exception("connection 错误");
            }
        }

        public DbParameter GetParameter(string parameterName,object vlaue)
        {
            return new MySqlParameter(parameterName, vlaue);
        }

        #endregion --- Function End ---
    }
}
