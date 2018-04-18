using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SLMultiFileUploaderApplication.Web.WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IUploadFileService”。
    [ServiceContract]
    public interface IUploadFileService
    {
        [OperationContract]
        void StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk);
       
        [OperationContract]
        void CancelUpload(string filename);

        [OperationContract]
        void AbortUpload(string filename);

        [OperationContract]
        string GetServerFilePath(string fileName, bool firstChunk, bool lastChunk);
    }
}
