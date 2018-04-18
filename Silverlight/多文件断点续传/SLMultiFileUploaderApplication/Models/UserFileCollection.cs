using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLMultiFileUploaderApplication.Models
{
    public class UserFileCollection : ObservableCollection<UserFile>
    {

        private double _UploadedLength;
        /// <summary>
        /// 已上传的累计字节数（多文件）
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
                this.OnPropertyChanged(new PropertyChangedEventArgs("UploadedLength"));
            }
        }


        private double _UploadedPercentage;
        /// <summary>
        /// 已上传字符数占全部字节数的百分比（多文件）
        /// </summary>
        public double UploadedPercentage
        {
            get
            {
                return this._UploadedPercentage;
            }
            set
            {
                this._UploadedPercentage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("UploadedPercentage"));
            }
        }

        /// <summary>
        /// 当前正在上传的文件序号
        /// </summary>
        private int _currentUpload = 0;

        /// <summary>
        /// 上传初始化参数，详情如下：
        /// MaxFileSizeKB: 	File size in KBs.
        /// MaxUploads: 	Maximum number of simultaneous uploads
        /// FileFilter:	File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg
        /// CustomParam: Your custom parameter, anything here will be available in the WCF webservice
        /// DefaultColor: The default color for the control, for example: LightBlue
        /// </summary>
        private string _customParams;

        /// <summary>
        /// 最大上传字节数
        /// </summary>
        private int _maxUpload;

        public UserFileCollection(string customParams, int maxUploads)
        {
            _customParams = customParams;
            _maxUpload = maxUploads;
            this.CollectionChanged += UserFileCollection_CollectionChanged;
        }
        /// <summary>
        /// 当添加或取消上传文件时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UserFileCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //当集合信息变化时（添加或删除项）时，则重新计算数据 
            recountTotal();
        }

        /// <summary>
        /// 重新计算数据
        /// </summary>
        private void recountTotal()
        {
            //Recount total
            double totalSize = 0;
            double totalSizeDone = 0;

            foreach (UserFile file in this)
            {
                totalSize += file.FileSize;
                totalSizeDone += file.UploadedLength;
            }

            double percentage = 0;

            if (totalSize > 0)
            {
                percentage = 100 * totalSizeDone / totalSize;
            }
            this.UploadedLength = totalSizeDone;

            this.UploadedPercentage = (int)percentage;
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="item"></param>
        public new void Add(UserFile item)
        {
            item.PropertyChanged += item_PropertyChanged;
            base.Add(item);
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //当属性变化为“从上传列表中移除”
            if (e.PropertyName == "IsDeleted")
            {
                UserFile file = (UserFile)sender;

                if (file.IsDeleted)
                {
                    if (file.State == UploadFileStates .Uploading)
                    {
                        _currentUpload--;
                        UploadFiles();
                    }

                    this.Remove(file);

                    file = null;
                }
            }
            //当属性变化为“开始上传”
            else if (e.PropertyName == "State")
            {
                UserFile file = (UserFile)sender;
                //此时file.State状态为ploading
                if (file.State ==  UploadFileStates.Finished || file.State ==  UploadFileStates.Error)
                {
                    _currentUpload--;
                    UploadFiles();
                }
                else if (file.State ==  UploadFileStates.Uploading)
                {
                }

            }
            //当属性变化为“上传进行中”
            else if (e.PropertyName == "UploadedLength")
            {
                //重新计算上传数据
                this.recountTotal();
            }
        }
      
        /// <summary>
        /// 上传文件
        /// </summary>
        public void UploadFiles()
        {
            lock (this)
            {
                foreach (UserFile file in this)
                {   //当上传文件未被移除（IsDeleted）或是暂停时

                    if (!file.IsDeleted && !file.IsStop && file.State ==  UploadFileStates.Pending && _currentUpload < _maxUpload)
                    {
                        file.Upload(_customParams);
                        _currentUpload++;
                    }
                }
            }
        }
    }
}
