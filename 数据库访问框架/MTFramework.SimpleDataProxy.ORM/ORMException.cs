using System;

namespace MTFramework.SimpleDataProxy.ORM
{
    /// <summary>
    /// ORM异常
    /// </summary>
    [Serializable]
    public class ORMException : SystemException
    {
        /// <summary>
        /// ORM异常
        /// </summary>
        public ORMException()
        {
        }

        /// <summary>
        /// ORM异常
        /// </summary>
        /// <param name="inner">内部异常</param>
        public ORMException(Exception inner)
            : this(inner.Message, inner)
        {
        }

        /// <summary>
        /// ORM异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public ORMException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// ORM异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="inner">内部异常</param>
        public ORMException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
