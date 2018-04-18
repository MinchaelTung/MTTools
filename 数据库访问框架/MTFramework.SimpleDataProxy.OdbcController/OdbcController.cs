using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using MTFramework.SimpleDataProxy.Interface;

namespace MTFramework.SimpleDataProxy.Odbc
{
    /// <summary>
    /// Odbc 数据源连接操作
    /// </summary>
    public class OdbcController : IDbController
    {
        private string _ConnectionString = string.Empty;
        private OdbcConnection _Connection = null;

        /// <summary>
        /// 获取设置连接字符串
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
                    this._Connection = new OdbcConnection(_ConnectionString);
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
                return new OdbcDataAdapter();
            }
        }

        /// <summary>
        /// 创建数据输出参数对象
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns></returns>
        public DbParameter CreateDbParameterOutput(string parameterName)
        {
            OdbcParameter param = new OdbcParameter();
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
            OdbcParameter param = new OdbcParameter();
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
            OdbcParameter param = new OdbcParameter();
            param.ParameterName = "ReturnValue";
            param.Direction = ParameterDirection.ReturnValue;
            return param;
        }
    }

}
