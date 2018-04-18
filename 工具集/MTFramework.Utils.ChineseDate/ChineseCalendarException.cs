using System;

namespace MTFramework.Utils.ChineseDate
{
    /// <summary>
    /// 中国日期异常
    /// </summary>
    public class ChineseCalendarException : Exception
    {
        /// <summary>
        /// 中国日期异常
        /// </summary>
        /// <param name="msg">错误信息</param>
        public ChineseCalendarException(string msg)
            : base(msg)
        {

        }
    }
}
