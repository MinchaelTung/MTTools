using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace LinkingEAsy.ZipValidationApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        private void clearViewData()
        {
            this.txtFilePath.Text = string.Empty;
            this.txtFileMD5.Text = string.Empty;
            this.txtClientMD5.Text = string.Empty;
            this.txtResultMsg.Text = string.Empty;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "zip文件|*.zip|rar文件|*.rar|所有文件|*.*";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "zip";
            if (openFileDialog.ShowDialog() == true)
            {
                this.txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtFilePath.Text) == true)
            {
                return;
            }
            if (File.Exists(this.txtFilePath.Text) == false)
            {
                return;
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            using (FileStream fst = new FileStream(this.txtFilePath.Text, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                md5.ComputeHash(fst);
                byte[] hash = md5.Hash;
                StringBuilder sb = new StringBuilder();
                foreach (byte byt in hash)
                {
                    sb.Append(String.Format("{0:X1}", byt));
                }
                this.txtFileMD5.Text = sb.ToString();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtFileMD5.Text) == true)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtClientMD5.Text) == true)
            {
                MessageBox.Show("请输入比较值!", "比较提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (this.txtFileMD5.Text.Equals(this.txtClientMD5.Text) == true)
            {
                MessageBox.Show("比较结果:和源文件一样!", "比较提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("比较结果:和源文件不一致!", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtFilePath.Text) == true)
            {
                return;
            }
            int f = this.txtFilePath.Text.ToLower().LastIndexOf(".zip");

            if (f != this.txtFilePath.Text.Length - 4)
            {
                MessageBox.Show("该文件不是Zip文件", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            System.Windows.Forms.FolderBrowserDialog folderBD = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBD.SelectedPath;
                bool isExport = this.unZiped(path, this.txtFilePath.Text);
                this.txtResultMsg.Text = isExport ? string.Format("解壓文件成功！請到 {0} 目錄下檢查。", path) : "解壓失敗，請檢查文件的完整性。";
            }
        }
        private bool unZiped(string ZipedFile, string exportPath)
        {
            try
            {
                FastZip fastZip = new FastZip();
                fastZip.ExtractZip(exportPath, ZipedFile, "");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
