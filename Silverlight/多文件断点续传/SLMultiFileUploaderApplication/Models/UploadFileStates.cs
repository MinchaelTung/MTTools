
namespace SLMultiFileUploaderApplication.Models
{
    public enum UploadFileStates
    {
        /// <summary>
        /// 暂停
        /// </summary>
        Pending = 0,
        /// <summary>
        /// 上传中
        /// </summary>
        Uploading = 1,
        /// <summary>
        /// 结束
        /// </summary>
        Finished = 2,
        /// <summary>
        /// 移除
        /// </summary>
        Deleted = 3,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 4
    }
}
