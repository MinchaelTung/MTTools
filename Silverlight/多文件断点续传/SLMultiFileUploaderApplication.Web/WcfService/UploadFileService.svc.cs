using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Hosting;

namespace SLMultiFileUploaderApplication.Web.WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“UploadFileService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 UploadFileService.svc 或 UploadFileService.svc.cs，然后开始调试。
    public class UploadFileService : IUploadFileService
    {
        private string _tempExtension = "_temp";
        public void StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = fileName + _tempExtension;

            //当上传文件的第一批数据时，先清空以往的相同文件名的文件（同名文件可能为上传失败造成）
            if (firstChunk)
            {
                ////删除临时文件
                //if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                //    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

                //删除目标文件
                if (File.Exists(uploadFolder + @"\" + fileName))
                    File.Delete(uploadFolder + @"\" + fileName);

            }


            FileStream fs = File.Exists(uploadFolder + @"\" + tempFileName) ==true? File.Open(uploadFolder + @"\" + tempFileName, FileMode.Append):File.Create(uploadFolder + @"\" + tempFileName);
            fs.Write(data, 0, dataLength);
            fs.Close();

            if (lastChunk)
            {
                //将临时文件重命名为原来的文件名称
                File.Move(uploadFolder + @"\" + tempFileName, uploadFolder + @"\" + fileName);

                //Finish stuff....
                FinishedFileUpload(fileName, parameters);
            }

        }

        public void CancelUpload(string filename)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = filename + _tempExtension;

            if (File.Exists(uploadFolder + @"\" + tempFileName))
                File.Delete(uploadFolder + @"\" + tempFileName);
        }

        public void AbortUpload(string filename)
        {
           
        }

        public string GetServerFilePath(string fileName, bool firstChunk, bool lastChunk)
        {
            string uploadFolder = GetUploadFolder();
            //string tempFileName = fileName + _tempExtension;
            string serverFileName = uploadFolder + @"\" + fileName;
            return serverFileName;
        }


        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="fileName"></param>
        protected void DeleteUploadedFile(string fileName)
        {
            string uploadFolder = GetUploadFolder();

            if (File.Exists(uploadFolder + @"\" + fileName))
                File.Delete(uploadFolder + @"\" + fileName);
        }

        protected virtual void FinishedFileUpload(string fileName, string parameters)
        {
        }

        protected virtual string GetUploadFolder()
        {
            return @HostingEnvironment.ApplicationPhysicalPath + "Upload";
        }

    }
}
