using System;
using System.Text;

namespace MTFramework.Utils.ChineseDate
{
    /// <summary>
    /// 中国黄历转换
    /// </summary>
    public class ChineseCalendar
    {
        #region --- Ctors Begin ---
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="date">公历日期</param>
        public ChineseCalendar(DateTime date)
        {
            ConvertDateTimeToChineseDate(date.Date);
        }

        /// <summary>
        /// 以黄历日期作参数的构造函数
        /// </summary>
        /// <param name="cYear">黄历年份</param>
        /// <param name="cMonth">黄历月份</param>
        /// <param name="cDay">黄历日数</param>
        public ChineseCalendar(int cYear, int cMonth, int cDay)
        {
            ConvertChineseDateToDateTime(cYear, cMonth, cDay);
        }

        #endregion --- Ctors End ---

        #region --- Fields Begin ---

        private const int MinYear = 1900;
        private const int MaxYear = 2050;
        private static DateTime MinDay = new DateTime(1900, 1, 30);
        private static DateTime MaxDay = new DateTime(2049, 12, 31);
        private const int GanZhiStartYear = 1864; //干支计算起始年
        private static DateTime GanZhiStartDay = new DateTime(1899, 12, 22);//起始日

        private DateTime _Date;
        /// <summary>
        /// 公历年
        /// </summary>
        private int _Year;

        /// <summary>
        /// 公历月
        /// </summary>
        private int _Month;

        /// <summary>
        /// 公历日
        /// </summary>
        private int _Day;

        /// <summary>
        /// 黄历年
        /// </summary>
        private int _cYear;

        /// <summary>
        /// 黄历月
        /// </summary>
        private int _cMonth;

        /// <summary>
        /// 黄历日
        /// </summary>
        private int _cDay;

        /// <summary>
        /// 是否闰年
        /// </summary>
        private bool _IsLeapYear;

        /// <summary>
        /// 是否有闰月
        /// </summary>
        private bool _cIsLeapYear;

        /// <summary>
        /// 是否闰月
        /// </summary>
        private bool _cIsLeapMonth;

        /// <summary>
        /// 中国数字
        /// </summary>
        private static string[] ChineseNumberStrs = { "○", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        /// <summary>
        /// 月份中文
        /// </summary>
        private static string[] ChineseMonthStrs = { "出错", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

        private static string[] ChineseNumberStrs2 = { "初", "十", "廿", "卅" };

        /// <summary>
        /// 农历数据
        /// </summary>
        /// <remarks>
        /// 数据结构如下，共使用17位数据
        /// 第17位：表示闰月天数，0表示29天   1表示30天
        /// 第16位-第5位（共12位）表示12个月，其中第16位表示第一月，如果该月为30天则为1，29天为0
        /// 第4位-第1位（共4位）表示闰月是哪个月，如果当年没有闰月，则置0
        /// </remarks>
        private static int[] LunarDateArray = new int[]{
                0x04BD8,0x04AE0,0x0A570,0x054D5,0x0D260,0x0D950,0x16554,0x056A0,0x09AD0,0x055D2,
                0x04AE0,0x0A5B6,0x0A4D0,0x0D250,0x1D255,0x0B540,0x0D6A0,0x0ADA2,0x095B0,0x14977,
                0x04970,0x0A4B0,0x0B4B5,0x06A50,0x06D40,0x1AB54,0x02B60,0x09570,0x052F2,0x04970,
                0x06566,0x0D4A0,0x0EA50,0x06E95,0x05AD0,0x02B60,0x186E3,0x092E0,0x1C8D7,0x0C950,
                0x0D4A0,0x1D8A6,0x0B550,0x056A0,0x1A5B4,0x025D0,0x092D0,0x0D2B2,0x0A950,0x0B557,
                0x06CA0,0x0B550,0x15355,0x04DA0,0x0A5B0,0x14573,0x052B0,0x0A9A8,0x0E950,0x06AA0,
                0x0AEA6,0x0AB50,0x04B60,0x0AAE4,0x0A570,0x05260,0x0F263,0x0D950,0x05B57,0x056A0,
                0x096D0,0x04DD5,0x04AD0,0x0A4D0,0x0D4D4,0x0D250,0x0D558,0x0B540,0x0B6A0,0x195A6,
                0x095B0,0x049B0,0x0A974,0x0A4B0,0x0B27A,0x06A50,0x06D40,0x0AF46,0x0AB60,0x09570,
                0x04AF5,0x04970,0x064B0,0x074A3,0x0EA50,0x06B58,0x055C0,0x0AB60,0x096D5,0x092E0,
                0x0C960,0x0D954,0x0D4A0,0x0DA50,0x07552,0x056A0,0x0ABB7,0x025D0,0x092D0,0x0CAB5,
                0x0A950,0x0B4A0,0x0BAA4,0x0AD50,0x055D9,0x04BA0,0x0A5B0,0x15176,0x052B0,0x0A930,
                0x07954,0x06AA0,0x0AD50,0x05B52,0x04B60,0x0A6E6,0x0A4E0,0x0D260,0x0EA65,0x0D530,
                0x05AA0,0x076A3,0x096D0,0x04BD7,0x04AD0,0x0A4D0,0x1D0B6,0x0D250,0x0D520,0x0DD45,
                0x0B5A0,0x056D0,0x055B2,0x049B0,0x0A577,0x0A4B0,0x0AA50,0x1B255,0x06D20,0x0ADA0,
                0x14B63        
                };

        private CelestialStem _ChineseYearCelestialStem;
        private TerrestrialBranch _ChineseYearTerrestrialBranch;
        private Animal _ChineseAnimal;
        private CelestialStem _ChineseMonrhCelestialStem;
        private TerrestrialBranch _ChineseMonrhTerrestrialBranch;
        private CelestialStem _ChineseDayCelestialStem;
        private TerrestrialBranch _ChineseDayTerrestrialBranch;

        #endregion --- Fields End ---

        #region --- Display Begin ---

        /// <summary>
        /// 公历节日
        /// </summary>
        public string DateHoliday
        {
            get
            {
                return HolidayTable.QuerySolarHoliday(this._Month, this._Day).HolidayName;
            }
        }

        /// <summary>
        /// 公历按周节日
        /// </summary>
        public string WeekDayHoliday
        {
            get
            {
                int weekDay = this.convertDayOfWeek(this._Date.DayOfWeek);

                DateTime firstDate = new DateTime(this._Year, this._Month, 1);
                int firWeekDays = convertDayOfWeek(firstDate.DayOfWeek);
                int weeks = firWeekDays + this._Day;
                return HolidayTable.QueryWeekHoliday(this._Month, weeks > 7 ? weeks : 1, weekDay).HolidayName;
            }
        }

        /// <summary>
        /// 公历日期
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this._Date;
            }
        }

        /// <summary>
        /// 公历星期数
        /// </summary>
        public string ChineseWeekDay
        {
            get
            {
                return ((WeekDay)this._Date.DayOfWeek).ToString();
            }
        }

        /// <summary>
        /// 公历生肖
        /// </summary>
        public string DateAnimal
        {
            get
            {
                return ((Animal)((this._Year - 3) % 12)).ToString();
            }
        }

        /// <summary>
        /// 当前星座
        /// </summary>
        public string ConstellationString
        {
            get
            {
                Constellation constellation = Constellation.白羊座;
                int y = this._Year;
                int m = this._Month;
                int d = this._Day;
                y = m * 100 + d;

                if (((y >= 321) && (y <= 419))) { constellation = Constellation.白羊座; }
                else if ((y >= 420) && (y <= 520)) { constellation = Constellation.金牛座; }
                else if ((y >= 521) && (y <= 620)) { constellation = Constellation.双子座; }
                else if ((y >= 621) && (y <= 722)) { constellation = Constellation.巨蟹座; }
                else if ((y >= 723) && (y <= 822)) { constellation = Constellation.狮子座; }
                else if ((y >= 823) && (y <= 922)) { constellation = Constellation.处女座; }
                else if ((y >= 923) && (y <= 1022)) { constellation = Constellation.天秤座; }
                else if ((y >= 1023) && (y <= 1121)) { constellation = Constellation.天蝎座; }
                else if ((y >= 1122) && (y <= 1221)) { constellation = Constellation.射手座; }
                else if ((y >= 1222) || (y <= 119)) { constellation = Constellation.摩羯座; }
                else if ((y >= 120) && (y <= 218)) { constellation = Constellation.水瓶座; }
                else if ((y >= 219) && (y <= 320)) { constellation = Constellation.双鱼座; }
                else { constellation = Constellation.白羊座; }
                return constellation.ToString();
            }
        }

        /// <summary>
        /// 公历是否闰年
        /// </summary>
        public bool IsLeapYear
        {
            get
            {
                return this._IsLeapYear;
            }
        }

        /// <summary>
        /// 农历年份
        /// </summary>
        public int ChineseYear
        {
            get
            {
                return this._cYear;
            }
        }

        /// <summary>
        /// 农历月份
        /// </summary>
        public int ChineseMonth
        {
            get
            {
                return this._cMonth;
            }
        }

        /// <summary>
        /// 农历天数
        /// </summary>
        public int ChineseDay
        {
            get
            {
                return this._cDay;
            }
        }

        /// <summary>
        /// 中国黄历节日
        /// </summary>
        public string ChineseCalendarHoliday
        {
            get
            {
                string msg = string.Empty;
                //闰月不计算节日
                if (this._cIsLeapMonth == false)
                {
                    msg = HolidayTable.QueryLunarHoliday(this._cMonth, this._cDay).HolidayName;
                }
                //对除夕进行特别处理
                if (this._cMonth == 12)
                {
                    if (this._cDay == getChineseMonthDays(this._cYear, 12))
                    {
                        msg = "除夕";
                    }
                }
                return msg;
            }
        }

        /// <summary>
        /// 农历年份字符串
        /// </summary>
        public string ChineseYearToString
        {
            get
            {
                string yearStr = string.Empty;
                int num = this._cYear;
                do
                {
                    yearStr = ConvertNumToChineseNum(num % 10) + yearStr;
                    num = num / 10;
                } while (num != 0);

                return yearStr;
            }
        }

        /// <summary>
        /// 农历月份字符串
        /// </summary>
        public string ChineseMonthToString
        {
            get
            {
                return ChineseMonthStrs[this._cMonth];
            }
        }

        /// <summary>
        /// 农历天数字符串
        /// </summary>
        public string ChineseDayToString
        {
            get
            {
                switch (this._cDay)
                {
                    case 0:
                        return "";
                    case 10:
                        return "初十";
                    case 20:
                        return "二十";
                    case 30:
                        return "三十";
                    default:
                        return ChineseNumberStrs2[(int)this._cDay / 10] + this.ConvertNumToChineseNum(this._cDay % 10);
                }
            }
        }

        /// <summary>
        /// 农历日期字符串
        /// </summary>
        public string ChineseDateToString
        {
            get
            {
                return string.Format("{0}年{1}月{2}日", this.ChineseYearToString, this.ChineseMonthToString, this.ChineseDayToString);
            }
        }

        /// <summary>
        /// 是否农历闰月
        /// </summary>
        public bool IsChineseLeapMonth
        {
            get
            {
                return this._cIsLeapMonth;
            }
        }

        /// <summary>
        /// 农历当年是否有闰月
        /// </summary>
        public bool IsChineseLeapYear
        {
            get
            {
                return this._cIsLeapYear;
            }
        }

        /// <summary>
        /// 当前节气
        /// </summary>
        public string ChineseSolarTerm
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;

                string tempStr = string.Empty;

                for (int i = 1; i <= 24; i++)
                {
                    SolarTerm solarTerm = SolarTerm24.SolarTermList[i - 1];
                    num = 525948.76 * (this._Year - 1900) + solarTerm.SolarTermInfo;

                    newDate = baseDateAndTime.AddMinutes(num);//按分钟计算
                    if (newDate.DayOfYear == this._Date.DayOfYear)
                    {
                        tempStr = solarTerm.SolarTermName;
                        break;
                    }
                }
                return tempStr;
            }
        }

        /// <summary>
        /// 农历年的甲子
        /// </summary>
        public string ChineseACycleOfSixtyYears
        {
            get
            {
                return this._ChineseYearCelestialStem.ToString() + this._ChineseYearTerrestrialBranch.ToString();
            }
        }

        /// <summary>
        /// 农历生肖
        /// </summary>
        public string ChineseAnimal
        {
            get
            {
                return this._ChineseYearTerrestrialBranch.ToString() + this._ChineseAnimal.ToString();
            }
        }

        /// <summary>
        /// 农历月的天干地支
        /// </summary>
        public string ChineseMonthCelestialStemAndTerrestrialBranch
        {
            get
            {
                return this._ChineseMonrhCelestialStem.ToString() + this._ChineseMonrhTerrestrialBranch.ToString();
            }
        }

        /// <summary>
        /// 农历日的天干地支
        /// </summary>
        public string ChineseDayCelestialStemAndTerrestrialBranch
        {
            get
            {
                return this._ChineseDayCelestialStem.ToString() + this._ChineseDayTerrestrialBranch.ToString();
            }
        }

        /// <summary>
        /// 农历日期以天干地支显示
        /// </summary>
        public string ChineseDateForCelestialStemAndTerrestrialBranch
        {
            get
            {
                return string.Format("{0}年 {1}月 {2}日", this.ChineseACycleOfSixtyYears, this.ChineseMonthCelestialStemAndTerrestrialBranch, this.ChineseDayCelestialStemAndTerrestrialBranch);
            }
        }

        /// <summary>
        /// 显示日期信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append("公元 ").Append(this._Date.ToString("yyyy年MM月dd日"));
            sb.Append("[").Append(this.ChineseWeekDay).Append("]").Append("\n\r");
            sb.Append(this.ConstellationString).Append("\n\r");
            if (this.DateHoliday != string.Empty || this.ChineseWeekDay != string.Empty)
            {
                sb.Append(this.DateHoliday).Append(this.DateHoliday != string.Empty ? "/" : "").Append(this.ChineseWeekDay).Append("\n\r");
            }

            sb.Append("农历 ").Append(this.ChineseDateToString).Append("\n\r");
            sb.Append(this._ChineseYearCelestialStem).Append(this._ChineseYearTerrestrialBranch);
            sb.Append("[").Append(this._ChineseAnimal).Append("]年  ");
            sb.Append(this._ChineseMonrhCelestialStem).Append(this._ChineseMonrhTerrestrialBranch).Append("月  ");
            sb.Append(this._ChineseDayCelestialStem).Append(this._ChineseDayTerrestrialBranch).Append("日\n\r");

            if (this.ChineseSolarTerm != string.Empty)
            {
                sb.Append(this.ChineseSolarTerm).Append("\n\r");
            }
            if (this.ChineseCalendarHoliday != string.Empty)
            {
                sb.Append(this.ChineseCalendarHoliday).Append("\n\r");
            }

            return sb.ToString();
        }

        #endregion --- Display End ---

        #region --- Functions Begin ---

        /// <summary>
        /// 计算黄历干支生肖
        /// </summary>
        private void CelestialStemAndTerrestrialBranch()
        {
            //年
            this._ChineseYearCelestialStem = (CelestialStem)((this._cYear - 3) % 10);
            int temp = (this._cYear - 3) % 12;
            this._ChineseYearTerrestrialBranch = (TerrestrialBranch)temp;
            this._ChineseAnimal = (Animal)temp;
            //月
            //正月
            int firstMonthCelestialStemValue = 2 * (int)this._ChineseYearCelestialStem - 4 + ((((int)this._ChineseYearCelestialStem) < 5) ? 5 : -5);
            int firstMonthTerrestrialBranchValue = (int)this._ChineseYearTerrestrialBranch;
            //当前月
            this._ChineseMonrhCelestialStem = (CelestialStem)((firstMonthCelestialStemValue + this._cMonth - 1) % 10);
            this._ChineseMonrhTerrestrialBranch = (TerrestrialBranch)((firstMonthTerrestrialBranchValue + this._cMonth - 1) % 12);
            //日       
            int c = this._Year / 100;
            int y = this._Year % 100;
            int j = (this._Month % 2 == 0) ? 6 : 0;
            int rgnum = (4 * c) + (c / 4) + (5 * y) + (y / 4) + (3 * (this._Month + 1) / 5) + this._Day - 3;
            int rdnum = rgnum + 4 * c + 10 + j;
            this._ChineseDayCelestialStem = (CelestialStem)(rgnum % 10);
            this._ChineseDayTerrestrialBranch = (TerrestrialBranch)(rdnum % 12);

        }

        /// <summary>
        /// 转换一周的第几天
        /// </summary>
        /// <param name="dayOfWeek">当前一周的第几天</param>
        /// <returns></returns>
        private int convertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 阿拉伯数字转换为中国数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string ConvertNumToChineseNum(int num)
        {
            if (num >= 0 && num <= 9)
            {
                return ChineseNumberStrs[num];
            }
            else
            {
                throw new ChineseCalendarException("阿拉伯数字转换为中国数字错误：没有此阿拉伯数字，必须在0到9之间含0和9。");
            }
        }

        /// <summary>
        /// 公历检查日期
        /// </summary>
        /// <param name="date">公历日期</param>
        private void checkDate(DateTime date)
        {
            if (date < MinDay || date > MaxDay)
            {
                throw new ChineseCalendarException("超出可转换的日期，可转换日期为30/1/1900-31/12/2049");
            }
            this._Date = date;
            this._Year = this._Date.Year;
            this._Month = this._Date.Month;
            this._Day = this._Date.Day;
        }

        /// <summary>
        /// 检查是否闰年
        /// </summary>       
        private void checkLeapYear()
        {
            DateTime temp = new DateTime(this._Year, this._Month, 1).AddMonths(1);
            this._IsLeapYear = (temp.AddDays(-1)).Day == 29;
        }

        /// <summary>
        /// 农历year年闰月的天数
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        private int getChineseLeapMonthDays(int year)
        {
            if ((LunarDateArray[year - MinYear] & 0xF) != 0)
            {
                if ((LunarDateArray[year - MinYear] & 0x10000) != 0)
                {
                    return 30;
                }
                else
                {
                    return 29;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 测试某位是否为真
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bitpostion"></param>
        /// <returns></returns>
        private bool bitTest32(int num, int bitpostion)
        {

            if ((bitpostion > 31) || (bitpostion < 0))
                throw new Exception("Error Param: bitpostion[0-31]:" + bitpostion.ToString());

            int bit = 1 << bitpostion;

            if ((num & bit) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 农历year年非闰月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int getChineseMonthDays(int year, int month)
        {
            if (bitTest32((LunarDateArray[year - MinYear] & 0x0000FFFF), (16 - month)))
            {
                return 30;
            }
            else
            {
                return 29;
            }
        }

        /// <summary>
        /// 取农历年year年的天数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private int getChineseYearDays(int year)
        {
            int i = 0x8000;
            int f = 0;
            //29天 X 12个月
            int sumDay = 0x15C;
            //当前农历数据
            int info = LunarDateArray[year - MinYear] & 0x0FFFF;
            //计算12个月中有多少天为30天
            for (int m = 0; m < 12; m++)
            {
                f = info & i;
                if (f != 0)
                {
                    sumDay++;
                }
                i = i >> 1;
            }
            return sumDay + getChineseLeapMonthDays(year);
        }

        /// <summary>
        /// 公历转换为黄历
        /// </summary>
        private void ConvertDateTimeToChineseDate(DateTime date)
        {
            checkDate(date);
            checkLeapYear();
            int i = 0;
            int leap = 0;
            int temp = 0;

            //计算两天的基本差距40426
            int offset = (this._Date - ChineseCalendar.MinDay).Days;
            for (i = MinYear; i < MaxYear; i++)
            {
                //求当年农历年天数377
                temp = getChineseYearDays(i);
                if (offset - temp < 1)
                {
                    this._cYear = i;
                    break;
                }
                else
                {
                    offset = offset - temp;
                }
            }

            //计算该年闰哪个月
            leap = LunarDateArray[this._cYear - MinYear] & 0xF;

            //设定当年是否有闰月
            if (leap > 0)
            {
                _cIsLeapYear = true;
            }
            else
            {
                _cIsLeapYear = false;
            }

            _cIsLeapMonth = false;
            for (i = 1; i <= 12; i++)
            {
                //闰月
                if ((leap > 0) && (i == leap + 1) && (_cIsLeapMonth == false))
                {
                    _cIsLeapMonth = true;
                    i = i - 1;
                    temp = getChineseLeapMonthDays(_cYear); //计算闰月天数
                }
                else
                {
                    _cIsLeapMonth = false;
                    temp = getChineseMonthDays(_cYear, i);//计算非闰月天数
                }

                offset = offset - temp;
                if (offset <= 0) break;
            }

            offset = offset + temp;
            this._cMonth = i;
            this._cDay = offset;
            this.CelestialStemAndTerrestrialBranch();
        }

        /// <summary>
        /// 黄历转换公历
        /// </summary>
        /// <param name="cYear">黄历年份</param>
        /// <param name="cMonth">黄历月份</param>
        /// <param name="cDay">黄历日数</param>
        private void ConvertChineseDateToDateTime(int cYear, int cMonth, int cDay)
        {
            checkChineseDate(cYear, cMonth, cDay);
            int i = 0;
            int leap = 0;
            int temp = 0;
            int offset = 0;
            for (i = MinYear; i < this._cYear; i++)
            {
                //求当年农历年天数
                temp = getChineseYearDays(i);
                offset += temp;
            }

            //计算该年那个月是闰月
            leap = getChineseLeapMonthDays(this._cYear);
            //设置当前是否闰年
            this._IsLeapYear = leap != 0;
            //设置当前是否闰月
            this._cIsLeapMonth = (this._cMonth != leap) ? false : true;

            if (this._cIsLeapYear == false || this._cMonth < leap)
            {
                for (i = 1; i < this._cMonth; i++)
                {
                    //计算非闰月天数
                    temp = getChineseMonthDays(this._cYear, i);
                    offset += temp;
                }
                //加上当月天数
                offset += this._cDay;

            }
            else //是闰年，且计算月份大于或等于闰月
            {
                for (i = 1; i < this._cMonth; i++)
                {
                    //计算非闰月天数
                    temp = getChineseMonthDays(this._cYear, i);
                    offset += temp;
                }
                //计算月大于闰月
                if (this._cMonth > leap)
                {
                    //计算闰月天数
                    temp = getChineseLeapMonthDays(this._cYear);
                    //加上闰月天数
                    offset += temp;
                    offset += this._cDay;
                }
                else //计算月等于闰月
                {
                    //如果需要计算的是闰月，则应首先加上与闰月对应的普通月的天数
                    if (this._cIsLeapMonth == true)
                    {
                        temp = getChineseMonthDays(this._cYear, this._cMonth);
                        offset += temp;
                    }
                    offset += this._cDay;
                }
            }
            this._Date = MinDay.AddDays(offset);
            this._Year = this._Date.Year;
            this._Month = this._Date.Month;
            this._Day = this._Date.Day;
            this.checkLeapYear();
            this.CelestialStemAndTerrestrialBranch();
        }

        /// <summary>
        /// 检查黄历数据是否正确
        /// </summary>
        /// <param name="cYear"></param>
        /// <param name="cMonth"></param>
        /// <param name="cDay"></param>
        private void checkChineseDate(int cYear, int cMonth, int cDay)
        {
            if (cYear < MinYear || cYear > MaxYear)
            {
                throw new ChineseCalendarException("非法黄历日期");
            }
            if (cMonth < 1 || cMonth > 12)
            {
                throw new ChineseCalendarException("非法黄历日期");
            }
            if (cDay < 1 || cDay > 30)
            {
                throw new ChineseCalendarException("非法黄历日期");
            }
            //计算该年应该闰哪个月
            //int leapMonth = LunarDateArray[cYear - MinYear] & 0xF;
            //if (leapMonthFlag == true && cMonth != leapMonth)
            //{
            //    throw new ChineseCalendarException("非法黄历日期");
            //}
            //检查日期是否大于最大天
            if (cDay > getChineseMonthDays(cYear, cMonth))
            {
                throw new ChineseCalendarException("非法黄历日期");
            }

            this._cYear = cYear;
            this._cMonth = cMonth;
            this._cDay = cDay;
        }

        #endregion --- Functions End ---
    }
}
