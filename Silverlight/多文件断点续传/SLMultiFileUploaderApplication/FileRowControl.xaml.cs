using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLMultiFileUploaderApplication.Models;

namespace SLMultiFileUploaderApplication
{
    public partial class FileRowControl : UserControl
    {
        private UserFile UserFile
        {
            get
            {
                if (this.DataContext != null)
                {
                    return (UserFile)this.DataContext;
                }
                else
                {
                    return null;
                }
            }
        }
        public FileRowControl()
        {
            InitializeComponent();
            this.Loaded += FileRowControl_Loaded;
        }

        void FileRowControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserFile.PropertyChanged += UserFile_PropertyChanged;
        }

        void UserFile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                //当前文件上传完毕后显示灰字
                if (this.UserFile.State == UploadFileStates.Finished)
                {
                    GreyOutText();
                    ShowValidIcon();
                }

                //如上传失败显示错误信息
                if (this.UserFile.State == UploadFileStates.Error)
                {
                    ErrorMsgTextBlock.Visibility = Visibility.Visible;
                }

                if (this.UserFile.State == UploadFileStates.Pending)
                {
                    ShowStopIcon();
                }
            }
        }

        /// <summary>
        /// 显示暂停状态
        /// </summary>
        private void ShowStopIcon()
        {
            PercentageProgress.Visibility = Visibility.Visible;
            ValidUploadIcon.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 隐藏进度条
        /// </summary>
        private void ShowValidIcon()
        {
            PercentageProgress.Visibility = Visibility.Collapsed;
            ValidUploadIcon.Visibility = Visibility.Visible;
            btnContinueUpload.IsEnabled = false;
            btnCancelUpload.IsEnabled = false;
        }

        /// <summary>
        /// 进度条状态设置
        /// </summary>
        private void GreyOutText()
        {
            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);

            FileNameTextBlock.Foreground = grayBrush;
            StateTextBlock.Foreground = grayBrush;
            FileSizeTextBlock.Foreground = grayBrush;
        }

        /// <summary>
        /// 删除按钮操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UserFile file = (UserFile)((TextBlock)e.OriginalSource).DataContext;
            file.IsDeleted = true;

            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 删除按钮操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserFile file = (UserFile)((Button)e.OriginalSource).DataContext;
            file.IsDeleted = true;

            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 暂停上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelUpload_Click(object sender, RoutedEventArgs e)
        {
            UserFile file = (UserFile)((Button)e.OriginalSource).DataContext;
            file.State = UploadFileStates.Pending;
            file.IsStop = true;
            btnContinueUpload.IsEnabled = true;
            btnCancelUpload.IsEnabled = false;
        }

        /// <summary>
        /// 继续上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinueUpload_Click(object sender, RoutedEventArgs e)
        {
            UserFile file = (UserFile)((Button)e.OriginalSource).DataContext;
            file.IsStop = false;
            file.State = UploadFileStates.Uploading;
            //当上传文件未被移除（IsDeleted）或是暂停时          
            btnCancelUpload.IsEnabled = true;
            btnContinueUpload.IsEnabled = false;
        }


    }
}
