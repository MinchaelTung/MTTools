using System.Collections.Generic;

namespace MTFramework.Utils.ChineseDate
{
    /// <summary>
    /// 中国二十四节气
    /// </summary>
    internal class SolarTerm24
    {
        private static List<SolarTerm> _SolarTermList;
        static SolarTerm24()
        {
            _SolarTermList = new List<SolarTerm>()
            {
                new SolarTerm("小寒",0),
                new SolarTerm("大寒",21208),
                new SolarTerm("立春",42467),
                new SolarTerm("雨水",63836),
                new SolarTerm("惊蛰",85337),
                new SolarTerm("春分",107014),
                new SolarTerm("清明",128867),
                new SolarTerm("谷雨",150921),
                new SolarTerm("立夏",173149),
                new SolarTerm("小满",195551),
                new SolarTerm("芒种",218072),
                new SolarTerm("夏至",240693),
                new SolarTerm("小暑",263343),
                new SolarTerm("大暑",285989),
                new SolarTerm("立秋",308563),
                new SolarTerm("处暑",331033),
                new SolarTerm("白露",353350),
                new SolarTerm("秋分",375494),
                new SolarTerm("寒露",397447),
                new SolarTerm("霜降",419210),
                new SolarTerm("立冬",440795),
                new SolarTerm("小雪",462224),
                new SolarTerm("大雪",483532),
                new SolarTerm("冬至",504758),
            };
        }

        /// <summary>
        /// 获取节气
        /// </summary>
        /// <returns></returns>
        public static List<SolarTerm> SolarTermList
        {
            get
            {
                return _SolarTermList;
            }
        }
    }
}
