using System.Data;
using System.Data.Common;

namespace MTFramework.SimpleDataProxy.Interface
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    public interface IDbController
    {
        /// <summary>
        /// 获取设置连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        DbConnection GetDbConnection { get; }

        /// <summary>
        /// 获取当前连接状态
        /// </summary>
        ConnectionState GetDbConnectionState { get; }

        /// <summary>
        /// 获取执行命令
        /// </summary>
        DbCommand GetDbCommand { get; }

        /// <summary>
        /// 数据源之间的适配器
        /// </summary>
        DbDataAdapter GetDbDataAdapter { get; }

        /// <summary>
        /// 创建数据输出参数对象
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns></returns>
        DbParameter CreateDbParameterOutput(string parameterName);

        /// <summary>
        /// 创建数据输入参数对象
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="vlaue">参数值</param>
        /// <param name="paramDirection">指定查询内的有关 System.Data.DataSet 的参数的类型。</param>
        /// <returns></returns>
        DbParameter CreateDbParameter(string parameterName, object vlaue, ParameterDirection paramDirection);
        
        /// <summary>
        /// 返回参数，返回变量名称，建议为 ReturnValue
        /// </summary>
        /// <returns></returns>
        DbParameter CreateDbParameterReturnValue();

    }
}
