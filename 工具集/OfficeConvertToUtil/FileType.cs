
namespace MTFramework.OfficeConvertToUtil
{
    /// <summary>
    /// 转换为的文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 根据目标文件扩展名判断
        /// </summary>
        Auto = 1,
        /// <summary>
        /// Windows Xps 文件
        /// </summary>
        Xps = 2,
        /// <summary>
        /// Adobe Pdf 文件
        /// </summary>
        Pdf = 3,
        /// <summary>
        /// 网页文件 Html
        /// </summary>
        Html=4,
        /// <summary>
        /// 单一网页模板文件 Mht
        /// </summary>
        Mht=5
    }
}
