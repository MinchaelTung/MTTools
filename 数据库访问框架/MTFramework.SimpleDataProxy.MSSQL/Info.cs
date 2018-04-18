
namespace MTFramework.SimpleDataProxy.MSSQL
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public static class CompanyInfo
    {
        /// <summary>
        /// 公司名稱
        /// </summary>
        public const string CompanyName = "Michael.Tung";

        /// <summary>
        /// 著作權
        /// </summary>
        public const string Copyright = "Copyright © Michael.Tung 2011";
    }

    /// <summary>
    /// 產品信息
    /// </summary>
    public static class ProductsInfo
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public const string Name = "MSSQL 数据库操作";

        /// <summary>
        /// 發布版本
        /// </summary>
        public const string Version = "1.0.0.0";

        /// <summary>
        /// 配置环境
        /// </summary>
#if(Net45)
        public const string Configuration = ".Net Framework 4.5";
#elif(Net40)
        public const string Configuration =".Net Framework 4.0";
#elif(Net35)
        public const string Configuration = ".Net Framework 3.5";
#else
        public const string Configuration =".Net Framework 2.0";
#endif
    }

    /// <summary>
    /// 構建版本
    /// </summary>
    public static class SetupInfo
    {
        /// <summary>
        /// 構建版本
        /// </summary>
        public const string FileVersion = "2012.08.13.01";
    }
}
