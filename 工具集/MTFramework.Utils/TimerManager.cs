using System;
using System.Threading;

namespace MTFramework.Utils
{
    /// <summary>
    /// 时间管理器
    /// </summary>
    public sealed class TimerManager
    {
        private static Timer _Timer;

        private static bool _IsStarted;

        private static DateTime _DateTime;

        static TimerManager()
        {
            TimerManager._Timer = null;
            TimerManager._DateTime = DateTime.Now;
            TimerManager._IsStarted = false;
        }

        /// <summary>
        /// 检查时间管理器是否在正常工作
        /// </summary>
        public static bool IsStarted
        {
            get
            {
                return TimerManager._IsStarted;
            }
        }

        /// <summary>
        /// 检查时间管理器处于静止中异常!
        /// </summary>
        private static void CheckIsNotStarted()
        {
            if (!TimerManager._IsStarted)
            {
                throw new TimerException("时间管理器处于静止中异常!");
            }
        }

        /// <summary>
        /// 检查时间管理器处于运行中异常!
        /// </summary>
        private static void CheckIsStarted()
        {
            if (TimerManager._IsStarted)
            {
                throw new TimerException("时间管理器处于运行中异常!");
            }
        }

        /// <summary>
        /// 关闭时间管理器
        /// </summary>
        public static void Dispose()
        {
            TimerManager.CheckIsNotStarted();
            TimerManager._IsStarted = false;
            TimerManager._Timer.Dispose();
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="datetime">时间参数</param>
        /// <returns>返回格式后的时间</returns>
        private static DateTime FormatDateTime(DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Kind);
        }
        /// <summary>
        /// 时间器运行回调函数
        /// </summary>
        /// <param name="state">回调方法要使用的信息的对象</param>
        private static void RunTimer(object state)
        {
            TimerManager._DateTime = TimerManager._DateTime.AddSeconds(1);
        }

        /// <summary>
        /// 启动时间管理器
        /// </summary>
        /// <param name="datetime">输入当前需要管理的时间</param>
        public static void StartManager(DateTime datetime)
        {
            TimerManager.CheckIsStarted();
            TimerManager._IsStarted = true;
            TimerManager._DateTime = TimerManager.FormatDateTime(datetime);
            TimerManager._Timer = new Timer(new TimerCallback(TimerManager.RunTimer), null, 0x3e7 - datetime.Millisecond, 0x3e8);
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static DateTime CurrentDateTime
        {
            get
            {
                TimerManager.CheckIsNotStarted();
                return TimerManager._DateTime;
            }
        }

        /// <summary>
        /// 获取当前日子结束的时间
        /// </summary>
        public static DateTime DayEnd
        {
            get
            {
                TimerManager.CheckIsNotStarted();
                return new DateTime(TimerManager._DateTime.Year, TimerManager._DateTime.Month, TimerManager._DateTime.Day, 0x17, 0x3b, 0x3b, TimerManager._DateTime.Kind);
            }
        }

        /// <summary>
        /// 获取当前日子开始的时间
        /// </summary>
        public static DateTime DayStart
        {
            get
            {
                TimerManager.CheckIsNotStarted();
                return new DateTime(TimerManager._DateTime.Year, TimerManager._DateTime.Month, TimerManager._DateTime.Day, 0, 0, 0, TimerManager._DateTime.Kind);
            }
        }
    }


    /// <summary>
    /// 时间管理器错误异常类型
    /// </summary>
    public sealed class TimerException : Exception
    {
        /// <summary>
        /// 时间轴管理器异常
        /// </summary>
        /// <param name="message">错误信息</param>
        public TimerException(string message)
            : base(message)
        {
        }
    }
}
