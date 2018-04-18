
namespace MTFramework.Utils.ChineseDate
{  /// <summary>
    /// 节日实体
    /// </summary>
    internal struct Holiday
    {
        #region --- Ctors Begin ---
        /// <summary>
        /// 节日
        /// </summary>
        /// <param name="month">月份</param>
        /// <param name="day">日数</param>
        /// <param name="recess">节日长度</param>
        /// <param name="name">节日名称</param>
        public Holiday(int month, int day, int recess, string name)
        {
            Month = month;
            Day = day;
            Recess = recess;
            HolidayName = name;
        }

        #endregion --- Ctors End ---

        #region --- Fields Begin ---
        /// <summary>
        /// 月份
        /// </summary>
        public int Month;
        /// <summary>
        /// 日数
        /// </summary>
        public int Day;
        /// <summary>
        /// 节日长度
        /// </summary>
        public int Recess;
        /// <summary>
        /// 节日名称
        /// </summary>
        public string HolidayName;

        #endregion --- Fields End ---
    }
}
