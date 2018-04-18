using System;
using System.Runtime.InteropServices;

namespace MTFramework.Utils
{
    /// <summary>
    /// 简单日期类
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Auto)]
    public struct SmartDate : IComparable
    {
        private const string SmartDateT = "t";
        private const string SmartDateToday = "today";
        private const string SmartDateY = "y";
        private const string SmartDateYesterday = "yesterday";
        private const string SmartDateTom = "tom";
        private const string SmartDateTomorrow = "tomorrow";
        private const string ValueNotSmartDateException = "该对象不是SmartDate类型";
        private const string StringToDateException = "输入的字符串不是日期类型";

        private DateTime date;
        private bool initialized;
        private SmartDate.EmptyValue emptyValue;
        private string format;
        private static string defaultFormat;

        /// <summary>
        /// 获取简单日期文本
        /// </summary>
        public string Text
        {
            get
            {
                return SmartDate.DateToString(this.Date, this.FormatString, this.emptyValue);
            }
            set
            {
                this.Date = SmartDate.StringToDate(value, this.emptyValue);
            }
        }
        /// <summary>
        /// 是否为空日期值
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (this.emptyValue == SmartDate.EmptyValue.MinDate)
                {
                    return this.Date.Equals(DateTime.MinValue);
                }
                return this.Date.Equals(DateTime.MaxValue);
            }
        }
        /// <summary>
        /// 日期格式化规则
        /// </summary>
        public string FormatString
        {
            get
            {
                if (this.format == null)
                {
                    this.format = SmartDate.defaultFormat;
                }
                return this.format;
            }
            set
            {
                this.format = value;
            }
        }
        /// <summary>
        /// 空值是否为最少日期
        /// </summary>
        public bool EmptyIsMin
        {
            get
            {
                return this.emptyValue == SmartDate.EmptyValue.MinDate;
            }
        }
        /// <summary>
        /// 数据库的值
        /// </summary>
        public object DBValue
        {
            get
            {
                if (this.IsEmpty)
                {
                    return DBNull.Value;
                }
                return this.Date;
            }
        }
        /// <summary>
        /// 获取时间类
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (!this.initialized)
                {
                    this.date = DateTime.MinValue;
                    this.initialized = true;
                }
                return this.date;
            }
            set
            {
                this.date = value;
                this.initialized = true;
            }
        }
        /// <summary>
        /// 字符串转换为日期
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <param name="emptyValue">日期空值标识</param>
        /// <param name="result">返回时间结果</param>
        /// <returns>转换是否成功</returns>
        private static bool TryStringToDate(string value, SmartDate.EmptyValue emptyValue, ref DateTime result)
        {
            DateTime time1;
            if (string.IsNullOrEmpty(value))
            {
                if (emptyValue == SmartDate.EmptyValue.MinDate)
                {
                    result = DateTime.MinValue;
                    return true;
                }
                result = DateTime.MaxValue;
                return true;
            }
            if (DateTime.TryParse(value, out time1))
            {
                result = time1;
                return true;
            }
            string text1 = value.Trim().ToLower();
            if (((text1 == SmartDateT) || (text1 == SmartDateToday)) || (text1 == ".?"))
            {
                result = DateTime.Now;
                return true;
            }
            if (((text1 == SmartDateY) || (text1 == SmartDateYesterday)) || (text1 == "-?"))
            {
                result = DateTime.Now.AddDays(-1);
                return true;
            }
            if (((text1 != SmartDateTom) && (text1 != SmartDateTomorrow)) && (text1 != "+?"))
            {
                return false;
            }
            result = DateTime.Now.AddDays(1);
            return true;
        }
        /// <summary>
        /// 字符串转换为简单日期
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="result">返回简单日期</param>
        /// <returns>转换是否成功</returns>
        public static bool TryParse(string value, ref SmartDate result)
        {
            return SmartDate.TryParse(value, SmartDate.EmptyValue.MinDate, ref result);
        }
        /// <summary>
        /// 字符串转换为简单日期
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyValue">日期空值标识</param>
        /// <param name="result">返回简单日期</param>
        /// <returns>转换是否成功</returns>
        public static bool TryParse(string value, SmartDate.EmptyValue emptyValue, ref SmartDate result)
        {
            DateTime time1 = DateTime.MinValue;
            if (SmartDate.TryStringToDate(value, emptyValue, ref time1))
            {
                result = new SmartDate(time1, emptyValue);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 简单日期转换为字符串
        /// </summary>
        /// <param name="format">格式化规则</param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return SmartDate.DateToString(this.Date, format, this.emptyValue);
        }
        /// <summary>
        /// 返回表示当前 MTFramework.Utils.SmartDate 的 System.String。
        /// </summary>
        /// <returns>返回 MTFramework.Utils.SmartDate 的 System.String 值</returns>
        public override string ToString()
        {
            return this.Text;
        }
        /// <summary>
        /// 比较简单日期
        /// </summary>
        /// <param name="value">简单日期对象</param>
        /// <returns></returns>
        public int CompareTo(object value)
        {
            if (!(value is SmartDate))
            {
                throw new ArgumentException(ValueNotSmartDateException);
            }
            return this.CompareTo((SmartDate)value);
        }
        /// <summary>
        /// 从此实例中减去指定持续时间
        /// </summary>
        /// <param name="value">从此实例中减去指定持续时间</param>
        /// <returns>System.DateTime，它等于此实例所表示的日期和时间减去 value 所表示的时间间隔</returns>
        public DateTime Subtract(TimeSpan value)
        {
            if (this.IsEmpty)
            {
                return this.Date;
            }
            return this.Date.Subtract(value);
        }
        /// <summary>
        /// 从此实例中减去指定的日期和时间
        /// </summary>
        /// <param name="value">System.DateTime 的一个实例</param>
        /// <returns>System.TimeSpan 间隔，它等于此实例所表示的日期和时间减去 value 所表示的日期和时间</returns>
        public TimeSpan Subtract(DateTime value)
        {
            if (this.IsEmpty)
            {
                return TimeSpan.Zero;
            }
            return this.Date.Subtract(value);
        }
        /// <summary>
        /// 日期字符串转换为日期
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyValue">日期空值标识</param>
        /// <returns>日期对象</returns>
        public static DateTime StringToDate(string value, SmartDate.EmptyValue emptyValue)
        {
            DateTime time1 = DateTime.MinValue;
            if (!SmartDate.TryStringToDate(value, emptyValue, ref time1))
            {
                throw new ArgumentException(StringToDateException);
            }
            return time1;
        }
        /// <summary>
        /// 日期字符串转换为日期
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyIsMin">如果转换为空返回最少值</param>
        /// <returns></returns>
        public static DateTime StringToDate(string value, bool emptyIsMin)
        {
            return SmartDate.StringToDate(value, SmartDate.GetEmptyValue(emptyIsMin));
        }
        /// <summary>
        /// 设置空值数据
        /// </summary>
        /// <param name="emptyValue">日期空值标识</param>
        private void SetEmptyDate(SmartDate.EmptyValue emptyValue)
        {
            if (emptyValue == SmartDate.EmptyValue.MinDate)
            {
                this.Date = DateTime.MinValue;
            }
            else
            {
                this.Date = DateTime.MaxValue;
            }
        }
        /// <summary>
        /// 设置默认的日期格式化规则
        /// </summary>
        /// <param name="formatString">日期格式化规则</param>
        public static void SetDefaultFormatString(string formatString)
        {
            SmartDate.defaultFormat = formatString;
        }
        /// <summary>
        /// 简单日期转换
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyValue">日期空值标识</param>
        /// <returns>简单日期</returns>
        public static SmartDate Parse(string value, SmartDate.EmptyValue emptyValue)
        {
            return new SmartDate(value, emptyValue);
        }
        /// <summary>
        /// 简单日期转换
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyIsMin">如果转换为空返回最少值</param>
        /// <returns>简单日期</returns>
        public static SmartDate Parse(string value, bool emptyIsMin)
        {
            return new SmartDate(value, emptyIsMin);
        }
        /// <summary>
        /// 简单日期转换
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <returns>简单日期</returns>
        public static SmartDate Parse(string value)
        {
            return new SmartDate(value);
        }
        /// <summary>
        /// 简单日期计算
        /// </summary>
        /// <param name="start">简单日期</param>
        /// <param name="span">时间间隔</param>
        /// <returns>计算结果</returns>
        public static SmartDate operator +(SmartDate start, TimeSpan span)
        {
            return new SmartDate(start.Add(span), start.EmptyIsMin);
        }
        /// <summary>
        /// 简单日期计算
        /// </summary>
        /// <param name="start">简单日期</param>
        /// <param name="finish">简单日期</param>
        /// <returns>计算结果</returns>
        public static SmartDate operator +(SmartDate start, SmartDate finish)
        {
            return new SmartDate(start.Add(finish.Date.TimeOfDay));
        }
        /// <summary>
        /// 简单日期计算
        /// </summary>
        /// <param name="start">简单日期</param>
        /// <param name="span">时间间隔</param>
        /// <returns>简单日期计算结果</returns>
        public static SmartDate operator -(SmartDate start, TimeSpan span)
        {
            return new SmartDate(start.Subtract(span), start.EmptyIsMin);
        }
        /// <summary>
        /// 简单日期计算
        /// </summary>
        /// <param name="start">简单日期</param>
        /// <param name="finish">减去的简单日期</param>
        /// <returns>时间间隔</returns>
        public static TimeSpan operator -(SmartDate start, SmartDate finish)
        {
            return start.Subtract(finish.Date);
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期字符串</param>
        /// <returns>比较结果</returns>
        public static bool operator <=(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">简单日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator <=(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">时间对象</param>
        /// <returns>比较结果</returns>
        public static bool operator <=(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期字符串</param>
        /// <returns>比较结果</returns>
        public static bool operator <(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">简单日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator <(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">时间对象</param>
        /// <returns>比较结果</returns>
        public static bool operator <(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期字符串</param>
        /// <returns>比较结果</returns>
        public static bool operator !=(SmartDate obj1, string obj2)
        {
            return !obj1.Equals(obj2);
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">简单日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator !=(SmartDate obj1, SmartDate obj2)
        {
            return !obj1.Equals(obj2);
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator !=(SmartDate obj1, DateTime obj2)
        {
            return !obj1.Equals(obj2);
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期字符串</param>
        /// <returns>比较结果</returns>
        public static bool operator >=(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">简单日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator >=(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator >=(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期字符串</param>
        /// <returns>比较结果</returns>
        public static bool operator >(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">简单日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator >(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator >(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期字符串</param>
        /// <returns>比较结果</returns>
        public static bool operator ==(SmartDate obj1, string obj2)
        {
            return obj1.Equals(obj2);
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">简单日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator ==(SmartDate obj1, SmartDate obj2)
        {
            return obj1.Equals(obj2);
        }
        /// <summary>
        /// 简单日期比较
        /// </summary>
        /// <param name="obj1">简单日期对象</param>
        /// <param name="obj2">日期对象</param>
        /// <returns>比较结果</returns>
        public static bool operator ==(SmartDate obj1, DateTime obj2)
        {
            return obj1.Equals(obj2);
        }
        /// <summary>
        /// 获取对象的哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Date.GetHashCode();
        }
        /// <summary>
        /// 获取简单日期空值数据
        /// </summary>
        /// <param name="emptyIsMin">空置类型</param>
        /// <returns></returns>
        private static SmartDate.EmptyValue GetEmptyValue(bool emptyIsMin)
        {
            if (emptyIsMin)
            {
                return SmartDate.EmptyValue.MinDate;
            }
            return SmartDate.EmptyValue.MaxDate;
        }
        /// <summary>
        /// 简单日期对象比较
        /// </summary>
        /// <param name="obj">简单日期对象</param>
        /// <returns>比较结果</returns>
        public override bool Equals(object obj)
        {
            if (obj is SmartDate)
            {
                SmartDate date1 = (SmartDate)obj;
                if (this.IsEmpty && date1.IsEmpty)
                {
                    return true;
                }
                return this.Date.Equals(date1.Date);
            }
            if (obj is DateTime)
            {
                return this.Date.Equals((DateTime)obj);
            }
            if (obj is string)
            {
                return this.CompareTo(obj.ToString()) == 0;
            }
            return false;
        }
        /// <summary>
        /// 日期的字符串表示方式
        /// </summary>
        /// <param name="value">日期</param>
        /// <param name="formatString">格式化规则</param>
        /// <param name="emptyValue">日期空值标识</param>
        /// <returns>日期字符串</returns>
        public static string DateToString(DateTime value, string formatString, SmartDate.EmptyValue emptyValue)
        {
            if (emptyValue == SmartDate.EmptyValue.MinDate)
            {
                if (value == DateTime.MinValue)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (value == DateTime.MaxValue)
                {
                    return string.Empty;
                }
            }
            return string.Format("{0:?" + formatString + "}?", value);
        }
        /// <summary>
        /// 日期的字符串表示方式
        /// </summary>
        /// <param name="value">日期</param>
        /// <param name="formatString">格式化规则</param>
        /// <param name="emptyIsMin">日期空值标识</param>
        /// <returns>日期字符串</returns>
        public static string DateToString(DateTime value, string formatString, bool emptyIsMin)
        {
            return SmartDate.DateToString(value, formatString, SmartDate.GetEmptyValue(emptyIsMin));
        }
        /// <summary>
        /// 日期的字符串表示方式
        /// </summary>
        /// <param name="value">日期</param>
        /// <param name="formatString">格式化规则</param>
        /// <returns>日期字符串</returns>
        public static string DateToString(DateTime value, string formatString)
        {
            return SmartDate.DateToString(value, formatString, true);
        }
        /// <summary>
        /// 日期比较
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <returns>比较结果</returns>
        public int CompareTo(string value)
        {
            return this.Date.CompareTo(SmartDate.StringToDate(value, this.emptyValue));
        }
        /// <summary>
        /// 日期比较
        /// </summary>
        /// <param name="value">简单日期</param>
        /// <returns>比较结果</returns>
        public int CompareTo(SmartDate value)
        {
            if (this.IsEmpty && value.IsEmpty)
            {
                return 0;
            }
            return this.date.CompareTo(value.Date);
        }
        /// <summary>
        /// 日期比较
        /// </summary>
        /// <param name="value">日期</param>
        /// <returns>比较结果</returns>
        public int CompareTo(DateTime value)
        {
            return this.Date.CompareTo(value);
        }
        /// <summary>
        /// 递增时间间隔
        /// </summary>
        /// <param name="value">时间间隔</param>
        /// <returns>递增后的日期</returns>
        public DateTime Add(TimeSpan value)
        {
            if (this.IsEmpty)
            {
                return this.Date;
            }
            return this.Date.Add(value);
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyValue">日期空值标识</param>
        public SmartDate(string value, SmartDate.EmptyValue emptyValue)
        {
            this.emptyValue = emptyValue;
            this.format = null;
            this.initialized = true;
            this.date = DateTime.MinValue;
            this.Text = value;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value">日期字符串</param>
        /// <param name="emptyIsMin">日期空值标识</param>
        public SmartDate(string value, bool emptyIsMin)
        {
            this.emptyValue = SmartDate.GetEmptyValue(emptyIsMin);
            this.format = null;
            this.initialized = true;
            this.date = DateTime.MinValue;
            this.Text = value;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value">日期字符串</param>
        public SmartDate(string value)
        {
            this.emptyValue = SmartDate.EmptyValue.MinDate;
            this.format = null;
            this.initialized = true;
            this.date = DateTime.MinValue;
            this.Text = value;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="emptyValue">日期空值标识</param>
        public SmartDate(SmartDate.EmptyValue emptyValue)
        {
            this.emptyValue = emptyValue;
            this.format = null;
            this.initialized = false;
            this.date = DateTime.MinValue;
            this.SetEmptyDate(emptyValue);
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value">日期</param>
        /// <param name="emptyValue">日期空值标识</param>
        public SmartDate(DateTime value, SmartDate.EmptyValue emptyValue)
        {
            this.emptyValue = emptyValue;
            this.format = null;
            this.initialized = false;
            this.date = DateTime.MinValue;
            this.Date = value;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value">日期</param>
        /// <param name="emptyIsMin">日期空值标识</param>
        public SmartDate(DateTime value, bool emptyIsMin)
        {
            this.emptyValue = SmartDate.GetEmptyValue(emptyIsMin);
            this.format = null;
            this.initialized = false;
            this.date = DateTime.MinValue;
            this.Date = value;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value">日期</param>
        public SmartDate(DateTime value)
        {
            this.emptyValue = SmartDate.EmptyValue.MinDate;
            this.format = null;
            this.initialized = false;
            this.date = DateTime.MinValue;
            this.Date = value;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="emptyIsMin">日期空值标识</param>
        public SmartDate(bool emptyIsMin)
        {
            this.emptyValue = SmartDate.GetEmptyValue(emptyIsMin);
            this.format = null;
            this.initialized = false;
            this.date = DateTime.MinValue;
            this.SetEmptyDate(this.emptyValue);
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        static SmartDate()
        {
            SmartDate.defaultFormat = "d?";
        }

        /// <summary>
        /// 日期空值标识
        /// </summary>
        public enum EmptyValue
        {
            /// <summary>
            /// 最少日期
            /// </summary>
            MinDate = 0,
            /// <summary>
            /// 最大日期
            /// </summary>
            MaxDate = 1
        }
    }
}
