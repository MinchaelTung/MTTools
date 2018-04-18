using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MTFramework.SimpleDataProxy.Interface;

namespace MTFramework.SimpleDataProxy.MSSQL
{
    /// <summary>
    /// MS SQL 数据库操作类
    /// </summary>
    public class MSSQLController : IDbController
    {
        private string _ConnectionString = string.Empty;
        private SqlConnection _Connection = null;

        /// <summary>
        /// 设置连接字符串
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

        /// <summary>
        /// 获取数据库连接对象
        /// </summary>       
        public DbConnection GetDbConnection
        {
            get
            {
                if (this._Connection == null && !string.IsNullOrEmpty(_ConnectionString))
                {
                    this._Connection = new SqlConnection(_ConnectionString);
                }
                return this._Connection;
            }
        }

        /// <summary>
        /// 获取当前连接状态
        /// </summary>   
        public ConnectionState GetDbConnectionState
        {
            get
            {
                return this._Connection.State;
            }
        }

        /// <summary>
        /// 获取执行命令
        /// </summary>
        public DbCommand GetDbCommand
        {
            get
            {
                DbCommand cmd = _Connection.CreateCommand();                   
                cmd.CommandTimeout = 0x1e;
                return cmd;
            }
        }

        /// <summary>
        /// 数据源之间的适配器
        /// </summary>
        public DbDataAdapter GetDbDataAdapter
        {
            get
            {
                return new SqlDataAdapter();
            }
        }

        /// <summary>
        /// 创建数据输出参数对象
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns></returns>
        public DbParameter CreateDbParameterOutput(string parameterName)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Direction = ParameterDirection.Output;
            return param;
        }

        /// <summary>
        /// 创建数据输入参数对象
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="vlaue">参数值</param>
        /// <param name="paramDirection">指定查询内的有关 System.Data.DataSet 的参数的类型。</param>
        /// <returns></returns>
        public DbParameter CreateDbParameter(string parameterName, object vlaue, ParameterDirection paramDirection)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = vlaue;
            param.Direction = paramDirection;
            return param;
        }

        /// <summary>
        /// 返回参数，返回变量名称，建议为 ReturnValue
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateDbParameterReturnValue()
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "ReturnValue";
            param.Direction = ParameterDirection.ReturnValue;
            return param;
        }
    }
}
