using System.Collections.Generic;

namespace MTFramework.Utils.ChineseDate
{
    /// <summary>
    /// 中国节日表
    /// </summary>
    internal class HolidayTable
    {
        #region --- Ctors Begin ---

        static HolidayTable()
        {
            //公历节日表
            _SolarHoliday = new List<Holiday>()
            {
                new Holiday(1, 1, 1, "元旦"),
                new Holiday(2, 2, 0, "世界湿地日"),
                new Holiday(2, 10, 0, "国际气象节"),
                new Holiday(2, 14, 0, "情人节"),
                new Holiday(3, 1, 0, "国际海豹日"),
                new Holiday(3, 5, 0, "学雷锋纪念日"),
                new Holiday(3, 8, 0, "妇女节"), 
                new Holiday(3, 12, 0, "植树节 孙中山逝世纪念日"), 
                new Holiday(3, 14, 0, "国际警察日"),
                new Holiday(3, 15, 0, "消费者权益日"),
                new Holiday(3, 17, 0, "中国国医节 国际航海日"),
                new Holiday(3, 21, 0, "世界森林日 消除种族歧视国际日 世界儿歌日"),
                new Holiday(3, 22, 0, "世界水日"),
                new Holiday(3, 24, 0, "世界防治结核病日"),
                new Holiday(4, 1, 0, "愚人节"),
                new Holiday(4, 7, 0, "世界卫生日"),
                new Holiday(4, 22, 0, "世界地球日"),
                new Holiday(5, 1, 1, "劳动节"), 
                new Holiday(5, 2, 1, "劳动节假日"),
                new Holiday(5, 3, 1, "劳动节假日"),
                new Holiday(5, 4, 0, "青年节"), 
                new Holiday(5, 8, 0, "世界红十字日"),
                new Holiday(5, 12, 0, "国际护士节"), 
                new Holiday(5, 31, 0, "世界无烟日"), 
                new Holiday(6, 1, 0, "国际儿童节"), 
                new Holiday(6, 5, 0, "世界环境保护日"),
                new Holiday(6, 26, 0, "国际禁毒日"),
                new Holiday(7, 1, 0, "建党节 香港回归纪念 世界建筑日"),
                new Holiday(7, 11, 0, "世界人口日"),
                new Holiday(8, 1, 0, "建军节"), 
                new Holiday(8, 8, 0, "中国男子节 父亲节"),
                new Holiday(8, 15, 0, "抗日战争胜利纪念"),
                new Holiday(9, 9, 0, "毛泽东逝世纪念"), 
                new Holiday(9, 10, 0, "教师节"), 
                new Holiday(9, 18, 0, "九·一八事变纪念日"),
                new Holiday(9, 20, 0, "国际爱牙日"),
                new Holiday(9, 27, 0, "世界旅游日"),
                new Holiday(9, 28, 0, "孔子诞辰"),
                new Holiday(10, 1, 1, "国庆节 国际音乐日"),
                new Holiday(10, 2, 1, "国庆节假日"),
                new Holiday(10, 3, 1, "国庆节假日"),
                new Holiday(10, 6, 0, "老人节"), 
                new Holiday(10, 24, 0, "联合国日"),
                new Holiday(11, 10, 0, "世界青年节"),
                new Holiday(11, 12, 0, "孙中山诞辰纪念"), 
                new Holiday(12, 1, 0, "世界艾滋病日"), 
                new Holiday(12, 3, 0, "世界残疾人日"), 
                new Holiday(12, 20, 0, "澳门回归纪念"), 
                new Holiday(12, 24, 0, "平安夜"), 
                new Holiday(12, 25, 0, "圣诞节"), 
                new Holiday(12, 26, 0, "毛泽东诞辰纪念")
            };

            _WeekHoliday = new List<Holiday>
            {
                new Holiday(5, 2, 1, "母亲节"), 
                new Holiday(5, 3, 1, "全国助残日"), 
                new Holiday(6, 3, 1, "父亲节"), 
                new Holiday(9, 3, 3, "国际和平日"), 
                new Holiday(9, 4, 1, "国际聋人节"), 
                new Holiday(10, 1, 2, "国际住房日"), 
                new Holiday(10, 1, 4, "国际减轻自然灾害日"),
                new Holiday(11, 4, 5, "感恩节")
            };

            //农历节日表
            _LunarHoliday = new List<Holiday>()
            {
                new Holiday(1, 1, 1, "春节"), 
                new Holiday(1, 15, 0, "元宵节"), 
                new Holiday(5, 5, 0, "端午节"), 
                new Holiday(7, 7, 0, "七夕情人节"),
                new Holiday(7, 15, 0, "中元节 盂兰盆节"), 
                new Holiday(8, 15, 0, "中秋节"), 
                new Holiday(9, 9, 0, "重阳节"), 
                new Holiday(12, 8, 0, "腊八节"),
                new Holiday(12, 23, 0, "北方小年(扫房)"),
                new Holiday(12, 24, 0, "南方小年(掸尘)")
            };
        }

        #endregion --- Ctors End ---

        #region --- Fields Begin ---

        private static List<Holiday> _SolarHoliday;
        /// <summary>
        /// 公历节日表
        /// </summary>
        public static List<Holiday> SolarHoliday
        {
            get
            {
                return _SolarHoliday;
            }
        }

        private static List<Holiday> _WeekHoliday;
        /// <summary>
        /// 按星期的节日
        /// </summary>
        public static List<Holiday> WeekHoliday
        {
            get
            {
                return _WeekHoliday;
            }
        }

        private static List<Holiday> _LunarHoliday;
        /// <summary>
        /// 农历节日表
        /// </summary>
        public static List<Holiday> LunarHoliday
        {
            get
            {
                return _LunarHoliday;
            }
        }

        #endregion --- Fields End ---

        #region --- Functions Begin ---

        /// <summary>
        /// 查询公历节日
        /// </summary>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns></returns>
        public static Holiday QuerySolarHoliday(int month, int day)
        {
            foreach (Holiday holiday in _SolarHoliday)
            {
                if (holiday.Month == month && holiday.Day == day)
                {
                    return holiday;
                }
            }
            return new Holiday(0, 0, 0, "");
        }

        /// <summary>
        /// 查询农历节日
        /// </summary>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns></returns>
        public static Holiday QueryLunarHoliday(int month, int day)
        {

            foreach (Holiday holiday in _LunarHoliday)
            {
                if (holiday.Month == month && holiday.Day == day)
                {
                    return holiday;
                }
            }
            return new Holiday(0, 0, 0, "");
        }

        /// <summary>
        /// 查询星期节日
        /// </summary>
        /// <param name="month">月</param>
        /// <param name="weekAtMonth">当月第几周</param>
        /// <param name="weekDay">当周第几日</param>
        /// <returns></returns>
        public static Holiday QueryWeekHoliday(int month, int weekAtMonth, int weekDay)
        {
            foreach (Holiday holiday in _WeekHoliday)
            {
                if (holiday.Month == month && holiday.Day == weekAtMonth && holiday.Recess == weekDay)
                {
                    return holiday;
                }
            }
            return new Holiday(0, 0, 0, "");
        }

        #endregion --- Functions End ---
    }
}
