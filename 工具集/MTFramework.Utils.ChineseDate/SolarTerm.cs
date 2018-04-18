
namespace MTFramework.Utils.ChineseDate
{
    /// <summary>
    /// 节气
    /// </summary>
    internal struct SolarTerm
    {
        public string SolarTermName;
        public int SolarTermInfo;
        /// <summary>
        /// 节气构造函数
        /// </summary>
        /// <param name="solarTermName"></param>
        /// <param name="solarTermInfo"></param>
        public SolarTerm(string solarTermName, int solarTermInfo)
        {
            SolarTermName = solarTermName;
            SolarTermInfo = solarTermInfo;
        }
    }
}
