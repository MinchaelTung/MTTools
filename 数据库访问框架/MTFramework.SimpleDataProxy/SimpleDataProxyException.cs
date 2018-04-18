using System;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// 数据库访问操作异常
    /// </summary>
    [Serializable]
    public class SimpleDataProxyException : SystemException
    {
         private int errorCode;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// 数据库访问操作异常类
        /// </summary>
        public SimpleDataProxyException()
        {
        }

        /// <summary>
        /// 数据库访问操作异常
        /// </summary>
        /// <param name="e">异常</param>
        public SimpleDataProxyException(Exception e)
            : base("?", e)
        {

        }

        /// <summary>
        /// 数据库访问操作异常
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="errorCode">错误代码</param>
        public SimpleDataProxyException(Exception e, int errorCode)
            : base("?", e)
        {

            this.errorCode = errorCode;
        }

        /// <summary>
        /// 数据库访问操作异常
        /// </summary>
        /// <param name="message">错误信息</param>
        public SimpleDataProxyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 数据库访问操作异常
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="e">异常</param>
        public SimpleDataProxyException(string message, Exception e)
            : base(message, e)
        {

        }

        /// <summary>
        /// 数据库访问操作异常
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="e">异常</param>
        /// <param name="errorCode">错误代码</param>
        public SimpleDataProxyException(string message, Exception e, int errorCode)
            : base(message, e)
        {

            this.errorCode = errorCode;
        }

        /// <summary>
        /// 数据库访问操作异常
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="errorCode">错误代码</param>
        public SimpleDataProxyException(string message, int errorCode)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
