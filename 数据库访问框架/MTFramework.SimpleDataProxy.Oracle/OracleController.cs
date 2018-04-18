using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using MTFramework.SimpleDataProxy.Interface;
using System.Security.Cryptography;

namespace MTFramework.SimpleDataProxy.Oracle
{
    /// <summary>
    /// Oracle 数据库操作类
    /// </summary>
    public class OracleController : IDbController
    {
        private string _ConnectionString = string.Empty;
        private OracleConnection _Connection = null;

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
                    this._Connection = new OracleConnection(_ConnectionString);
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
                return new OracleDataAdapter();
            }
        }

        /// <summary>
        /// 创建数据输出参数对象
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns></returns>
        public DbParameter CreateDbParameterOutput(string parameterName)
        {
            OracleParameter param = new OracleParameter();
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
            OracleParameter param = new OracleParameter();
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
            OracleParameter param = new OracleParameter();
            param.ParameterName = "ReturnValue";
            param.Direction = ParameterDirection.ReturnValue;
            return param;
        }
    }
}
