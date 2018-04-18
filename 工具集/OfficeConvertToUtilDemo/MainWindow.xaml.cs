using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using MTFramework.OfficeConvertToUtil;

namespace Demo
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Word(*.doc,docx)|*.doc;*.docx|Excel(*.xls,*.xlsx)|*.xls;*.xlsx|PowerPoint(*.ppt,*.pptx)|*.ppt;*.pptx|Visio(.vsd)|*.vsd";
            if ((bool)dlg.ShowDialog(this))
            {
                string filePath = dlg.FileName;
                string xpsFilePath = string.Empty;
                if (this.rad_PDF.IsChecked == true)
                {
                    xpsFilePath = dlg.FileName + ".pdf";
                }

                if (this.rad_XPS.IsChecked == true)
                {
                    xpsFilePath = dlg.FileName + ".xps";
                }
                if (this.rad_HTML.IsChecked == true)
                {
                    xpsFilePath = dlg.FileName + ".html";
                }
                if (this.rad_MHT.IsChecked == true)
                {
                    xpsFilePath = dlg.FileName + ".mht";
                }
              
                OfficeConvertResultInfo convertResults = OfficeConvert.OfficeConvertTo(filePath, xpsFilePath);

                switch (convertResults.Result)
                {
                    case OfficeConvertResult.Success:
                        MessageBox.Show("转换成功");
                        break;
                    case OfficeConvertResult.InvalidPath:
                        MessageBox.Show("文件路径错误");
                        break;
                    case OfficeConvertResult.InvalidFileType:
                        MessageBox.Show("文件类型错误");
                        break;
                    case OfficeConvertResult.InitializeOfficeAppError:
                        MessageBox.Show("没有安装Office程序");
                        break;
                    case OfficeConvertResult.OpenOfficeFileError:
                        MessageBox.Show("文件不是Office文件或者文件已损坏");
                        break;
                    case OfficeConvertResult.OfficeInteropError:
                        MessageBox.Show("当前用户安装的Office是非法副本");
                        break;
                    case OfficeConvertResult.ExportToXpsError:
                        MessageBox.Show("转换为XPS文件错误");
                        break;
                    case OfficeConvertResult.ExportToPdfError:
                        MessageBox.Show("转换为PDF文件错误");
                        break;
                    case OfficeConvertResult.ExportToError:
                        MessageBox.Show("转换为目标文件错误");
                        break;
                    case OfficeConvertResult.UnknownError:
                        MessageBox.Show("未知错误");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
