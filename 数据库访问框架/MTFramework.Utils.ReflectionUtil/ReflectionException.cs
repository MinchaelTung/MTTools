using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 反射工具异常
    /// </summary>
    [Serializable]
    public class ReflectionException : SystemException
    {
        /// <summary>
        /// 反射工具异常
        /// </summary>
        public ReflectionException()
        {
        }

        /// <summary>
        /// 反射工具异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public ReflectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 反射工具异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="inner">内部异常对象</param>
        public ReflectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
