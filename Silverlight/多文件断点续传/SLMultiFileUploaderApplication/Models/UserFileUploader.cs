using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLMultiFileUploaderApplication.WcfService;

namespace SLMultiFileUploaderApplication.Models
{
    public class UserFileUploader
    {
        private UserFile _UserFile;
        private double _DataLength;
        private double _DataSent;
        private string _InitParams;
        private bool _FirstChunk = true;
        private bool _LastChunk = false;
        private string _ServerFilePath = string.Empty;
        private UploadFileServiceClient _Client = null;


        public UserFileUploader(UserFile file)
        {
            _UserFile = file;
            _DataLength = _UserFile.FileSize;
            _DataSent = 0;
            _Client = new UploadFileServiceClient();
            _Client.GetServerFilePathCompleted += _Client_GetServerFilePathCompleted;
            _Client.GetServerFilePathAsync(_UserFile.FileName, _FirstChunk, _LastChunk);
            _Client.StoreFileAdvancedCompleted += _Client_StoreFileAdvancedCompleted;
            _Client.CancelUploadCompleted += _Client_CancelUploadCompleted;
            _Client.AbortUploadCompleted += _Client_AbortUploadCompleted;
            _Client.ChannelFactory.Closed += ChannelFactory_Closed;
        }


        void _Client_AbortUploadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //此处事件只是放在此处没有做处理，放在此处可以处理暂停操作的处理
        }
        
        void _Client_StoreFileAdvancedCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //检查WEB服务是否存在错误
            if (e.Error != null)
            {
                //当错误时放弃上传
                _UserFile.State = UploadFileStates.Error;
            }
            else
            {
                //如果文件未取消上传并且没有的话，则继续上传
                if (!_UserFile.IsDeleted && !_UserFile.IsStop)
                    UploadAdvanced();
            }
        }

        void _Client_GetServerFilePathCompleted(object sender, GetServerFilePathCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                _ServerFilePath = e.Result;
                _UserFile.ServerFileName = _ServerFilePath;
            } 
        }

        public string ServerFilePath
        {
            get
            {
                return this._ServerFilePath;
            }
            set
            {
                this._ServerFilePath = value;
            }
        }
        /// <summary>
        /// 取消上传
        /// </summary>
        public void CancelUpload()
        {
            _Client.CancelUploadAsync(_UserFile.FileName);
        }

        void _Client_CancelUploadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //当取消上传完成后关闭Channel
            _Client.ChannelFactory.Close();
        }
        void ChannelFactory_Closed(object sender, EventArgs e)
        {
            channelIsClosed();
        }

        /// <summary>
        /// Channel被关闭
        /// </summary>
        private void channelIsClosed()
        {
            if (!_UserFile.IsDeleted)
            {
                if (UploadFinished != null)
                    UploadFinished(this, null);
            }
        }

        /// <summary>
        /// 上传完成事件处理对象声明
        /// </summary>
        public event EventHandler UploadFinished;

        public void UploadAdvanced(string initParams)
        {
            _InitParams = initParams;

            UploadAdvanced();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        private void UploadAdvanced()
        {

            byte[] buffer = new byte[4 * 4096];
            int bytesRead = _UserFile.FileStream.Read(buffer, 0, buffer.Length);

            //文件是否上传完毕?
            if (bytesRead != 0)
            {
                _DataSent += bytesRead;

                if (_DataSent == _DataLength)
                    _LastChunk = true;//是否是最后一块数据，这样WCF会在服务端根据该信息来决定是否对临时文件重命名

                //上传当前数据块
                _Client.StoreFileAdvancedAsync(_UserFile.FileName, buffer, bytesRead, _InitParams, _FirstChunk, _LastChunk);

                //在第一条消息之后一直为false
                _FirstChunk = false;

                //通知上传进度修改
                onProgressChanged();
            }
            else
            {
                //当上传完毕后
                _UserFile.FileStream.Dispose();
                _UserFile.FileStream.Close();

                _Client.ChannelFactory.Close();
            }
        }

        /// <summary>
        /// 修改进度属性
        /// </summary>
        private void onProgressChanged()
        {
            _UserFile.UploadedLength = _DataSent;//注：此处会先调用FileCollection中的同名属性，然后才是_file.BytesUploaded属性绑定
        }
       
        /// <summary>
        /// 暂停上传
        /// </summary>
        public void StopUpload()
        {
            _Client.AbortUploadAsync(_UserFile.FileName);
        }
    }
}
