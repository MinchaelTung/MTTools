
namespace MTFramework.OfficeConvertToUtil
{
    /// <summary>
    /// Office 文档转换结果类型
    /// </summary>
    public enum OfficeConvertResult
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 路径无效
        /// </summary>
        InvalidPath = 2,
        /// <summary>
        /// 文件类型无效
        /// </summary>
        InvalidFileType = 3,
        /// <summary>
        /// 初始化Office程序错误，可能用户没有安装正版Office
        /// </summary>
        InitializeOfficeAppError = 4,
        /// <summary>
        /// 打开Office文件错误，可能不是完整的Office文件
        /// </summary>
        OpenOfficeFileError = 5,
        /// <summary>
        /// 无法引用 Office 操作，可能用户没有安装正版Office
        /// </summary>
        OfficeInteropError = 6,
        /// <summary>
        /// 无法转换为Xps文件，可能用户没有Xps查看器或者用户系统不在 Windows 7 或以上版本操作系统
        /// </summary>
        ExportToXpsError = 7,
        /// <summary>
        /// 无法转换为Pdf文件，可能用户没有安装Adobe Reader 工具
        /// </summary>
        ExportToPdfError = 8,
        /// <summary>
        /// 无法转换为目标文件
        /// </summary>
        ExportToError = 9,
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError
    }
}
