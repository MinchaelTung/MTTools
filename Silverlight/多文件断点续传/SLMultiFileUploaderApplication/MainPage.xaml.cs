using SLMultiFileUploaderApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;


namespace SLMultiFileUploaderApplication
{
    public partial class MainPage : UserControl
    {
        private int _maxFileSize = int.MaxValue;
        private UserFileCollection _files;
        //限制正在上传的文件数量
        private int _maxUpload = 4;
        private string _customParams;
        private string _fileFilter;

        public MainPage(IDictionary<string, string> initParams)
        {
            InitializeComponent();
            LoadConfiguration(initParams);

            _files = new UserFileCollection(_customParams, _maxUpload);

            FileList.ItemsSource = _files;
            FilesCount.DataContext = _files;
            TotalProgress.DataContext = _files;
            TotalKB.DataContext = _files;

        }

        private void LoadConfiguration(IDictionary<string, string> initParams)
        {
            string tryTest = string.Empty;

            //加载定制配置信息串
            if (initParams.ContainsKey("CustomParam") && !string.IsNullOrEmpty(initParams["CustomParam"]))
                _customParams = initParams["CustomParam"];

            if (initParams.ContainsKey("MaxUploads") && !string.IsNullOrEmpty(initParams["MaxUploads"]))
            {
                int.TryParse(initParams["MaxUploads"], out _maxUpload);
            }

            if (initParams.ContainsKey("MaxFileSizeKB") && !string.IsNullOrEmpty(initParams["MaxFileSizeKB"]))
            {
                if (int.TryParse(initParams["MaxFileSizeKB"], out _maxFileSize))
                    _maxFileSize = _maxFileSize * 1024;
            }

            if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
                _fileFilter = initParams["FileFilter"];



            ////从配置文件中获取相关信息
            //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxFileSizeKB"]))
            //{
            //    if (int.TryParse(ConfigurationManager.AppSettings["MaxFileSizeKB"], out _maxFileSize))
            //        _maxFileSize = _maxFileSize * 1024;
            //}


            //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxUploads"]))
            //    int.TryParse(ConfigurationManager.AppSettings["MaxUploads"], out _maxUpload);

            //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FileFilter"]))
            //    _fileFilter = ConfigurationManager.AppSettings["FileFilter"];
        }



        /// <summary>
        /// 选择文件对话框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            try
            {
                if (!string.IsNullOrEmpty(_fileFilter))
                    ofd.Filter = _fileFilter;
            }
            catch (ArgumentException ex)
            {
                //User supplied a wrong configuration file
                throw new Exception("Wrong file filter configuration.", ex);
            }

            if (ofd.ShowDialog() == true)
            {

                foreach (FileInfo file in ofd.Files)
                {
                    string fileName = file.Name;

                    UserFile userFile = new UserFile();
                    userFile.FileName = file.Name;
                    userFile.FileStream = file.OpenRead();


                    if (userFile.FileStream.Length <= _maxFileSize)
                    {
                        //向文件列表中添加文件信息
                        _files.Add(userFile);
                    }
                    else
                    {
                        MessageBox.Show("Maximum file size is: " + (_maxFileSize / 1024).ToString() + "KB.");
                    }
                }
            }
        }

        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (_files.Count == 0)
            {
                MessageBox.Show("No files to upload. Please select one or more files first.");

            }
            else
            {
                _files.UploadFiles();
            }
        }

        /// <summary>
        /// 清空上传文件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _files.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //注意不能跨域下载
            string download = @"http://localhost:27759/111.msi";
            Uri uri = new Uri(download);
            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            //client.DownloadFileAsync(uri);
            client.OpenReadAsync(uri);

        }



        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.tbMsgString.Text = string.Format("完成百分比：{0} 当前收到的字节数：{1} 资料大小：{2} ",
             e.ProgressPercentage.ToString() + "%",
             e.BytesReceived.ToString(),
             e.TotalBytesToReceive.ToString());

        }



        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //OpenReadCompletedEventArgs.Error - 该异步操作期间是否发生了错误
            //OpenReadCompletedEventArgs.Cancelled - 该异步操作是否已被取消
            //OpenReadCompletedEventArgs.Result - 下载后的 Stream 类型的数据
            //OpenReadCompletedEventArgs.UserState - 用户标识
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            if (e.Cancelled != true)
            {
                //获取下载的流数据(在此处是图片数据)并显示在图片控件中
                Stream stream = e.Result;


                FileStream fs = new FileStream(@"e:\111.msi", FileMode.CreateNew);
                byte[] buf = new byte[stream.Length];

                stream.Read(buf, 0, buf.Length);

                fs.Write(buf, 0, buf.Length);
                fs.Close();


            }



        }

    }
}
