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
using System.IO;
using System.ComponentModel;

namespace SLMultiFileUploaderApplication.Models
{
    /// <summary>
    /// 用户上传文件信息类
    /// </summary>
    public class UserFile : INotifyPropertyChanged
    {
        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _FileName;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return this._FileName;
            }
            set
            {
                this._FileName = value;
                this.NotifyPropertyChanged("FileName");
            }
        }


        private bool _IsDeleted = false;
        /// <summary>
        /// 是否取消该文件
        /// </summary>
        public bool IsDeleted
        {
            get
            {
                return this._IsDeleted;
            }
            set
            {
                this._IsDeleted = value;
                if (this._IsDeleted)
                {
                    this.cancelUpload();
                }
                this.NotifyPropertyChanged("IsDeleted");
            }
        }



        private Stream _FileStream;
        /// <summary>
        /// 文件上传的流信息
        /// </summary>
        public Stream FileStream
        {
            get
            {
                return this._FileStream;
            }
            set
            {
                this._FileStream = value;
                if (this._FileStream != null)
                {
                    this._FileSize = this._FileStream.Length;
                }
            }
        }


        private UploadFileStates _State = UploadFileStates.Pending;
        /// <summary>
        /// 文件上传状态
        /// </summary>
        public UploadFileStates State
        {
            get
            {
                return this._State;
            }
            set
            {
                this._State = value;
                this.NotifyPropertyChanged("State");
            }
        }


        private double _UploadedLength = 0.0;
        /// <summary>
        /// 已经上传字节数
        /// </summary>
        public double UploadedLength
        {
            get
            {
                return this._UploadedLength;
            }
            set
            {
                this._UploadedLength = value;

                this.NotifyPropertyChanged("UploadedLength");

                UploadedPercentage = (int)((this._UploadedLength * 100) / this.FileSize);
            }
        }


        private double _FileSize = 0;
        /// <summary>
        /// 文件大小
        /// </summary>
        public double FileSize
        {
            get
            {
                return this._FileSize;
            }
        }

        private int _UploadedPercentage = 0;
        /// <summary>
        /// 已上传文件的百分比
        /// </summary>
        public int UploadedPercentage
        {
            get
            {
                return this._UploadedPercentage;
            }
            private set
            {
                this._UploadedPercentage = value;
                this.NotifyPropertyChanged("UploadedPercentage");
            }
        }


        private bool _IsStop;
        /// <summary>
        /// 是否暂停文件上传
        /// </summary>
        public bool IsStop
        {
            get
            {
                return this._IsStop;
            }
            set
            {
                this._IsStop = value;
                if (this._IsStop)
                {
                    this.stopUpload();
                }
                else
                {
                    this.rebuildUpload();
                }
                this.NotifyPropertyChanged("IsStop");
            }
        }


        private string _ServerFileName;
        /// <summary>
        /// 上传后的文件路径及名称
        /// </summary>
        public string ServerFileName
        {
            get
            {
                return this._ServerFileName;
            }
            set
            {
                this._ServerFileName = value;
                this.NotifyPropertyChanged("ServerFileName");
            }
        }

        #region --- Functions Begin ---

        /// <summary>
        /// 上传文件操作类
        /// </summary>
        private UserFileUploader _UserFileUploader;

        /// <summary>
        /// 上传当前文件
        /// </summary>
        /// <param name="initParams"></param>
        public void Upload(string initParams)
        {
            this.State = UploadFileStates.Uploading;
            _UserFileUploader = new UserFileUploader(this);
            _UserFileUploader.UploadAdvanced(initParams);
            _UserFileUploader.UploadFinished += _UserFileUploader_UploadFinished;

        }

        void _UserFileUploader_UploadFinished(object sender, EventArgs e)
        {
            this.ServerFileName = _UserFileUploader.ServerFilePath;
            _UserFileUploader = null;
            this.State = UploadFileStates.Finished;
        }

        /// <summary>
        /// 取消上传
        /// </summary>
        private void cancelUpload()
        {
            if (_UserFileUploader != null && this.State == UploadFileStates. Uploading)
            {
                _UserFileUploader.CancelUpload();
            }
        }

        /// <summary>
        /// 继续上传
        /// </summary>
        private void rebuildUpload()
        {
            if (_UserFileUploader != null)
            {
                _UserFileUploader.UploadAdvanced("yourparameters");
            }   
        }

        /// <summary>
        /// 停止上传
        /// </summary>
        private void stopUpload()
        {
            if (_UserFileUploader != null && this.State == UploadFileStates.Pending)
            {
                _UserFileUploader.StopUpload();
            }
        }

        #endregion --- Functions End ---
    }
}
