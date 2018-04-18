using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using MTFramework.SimpleDataProxy;
using MTFramework.SimpleDataProxy.ConfigManagement;

namespace DBConnectionConfig
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBConfigEntity _DBConfigEntity = null;
        private int count = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            ComboBoxItem item = e.AddedItems[0] as ComboBoxItem;
            switch (item.Content.ToString())
            {
                case "MY-SQL":
                    this.textBox2.Text = "MTFramework.SimpleDataProxy.MySQL5x.MySql5xDbController";
                    this.textBox4.IsEnabled = true;
                    this.textBox5.IsEnabled = true;
                    this.textBox6.IsEnabled = true;
                    break;
                case "MS-SQL":
                    this.textBox2.Text = "MTFramework.SimpleDataProxy.MSSQL.MSSQLController";
                    this.textBox4.IsEnabled = true;
                    this.textBox5.IsEnabled = true;
                    this.textBox6.IsEnabled = true;
                    break;
                case "Oracle":
                    this.textBox2.Text = "MTFramework.SimpleDataProxy.Oracle.OracleController";
                    this.textBox4.IsEnabled = true;
                    this.textBox5.IsEnabled = true;
                    this.textBox6.IsEnabled = false;
                    break;
                default: this.textBox2.Text = "MTFramework.SimpleDataProxy.Odbc.OdbcController";
                    this.textBox4.IsEnabled = false;
                    this.textBox5.IsEnabled = false;
                    this.textBox6.IsEnabled = false;
                    break;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog1 = new Microsoft.Win32.OpenFileDialog();
            fileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog1.FilterIndex = 1;
            fileDialog1.Filter = "程序集文件 (*.dll)|*.dll";
            fileDialog1.FileName = "*.dll";
            if (fileDialog1.ShowDialog() == true)
            {
                this.textBox1.Text = fileDialog1.SafeFileName;
            }
            else
            {
                this.textBox1.Text = string.Empty;
            }
        }

        //测试
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.vdate() == false)
            {
                return;
            }
            try
            {
                count++;
                DataProxyPool.SetConnection(this._DBConfigEntity.AssemblyName, this._DBConfigEntity.ClassTypeName, "DBTEST" + count, this._DBConfigEntity.ConnectionString);

                bool bl = DataProxyPool.GetDataProxy["DBTEST" + count].DbOpen();
                if (bl == true && DataProxyPool.GetDataProxy["DBTEST" + count].DbState == System.Data.ConnectionState.Open)
                {
                    MessageBox.Show("连接数据库成功!", "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败!---错误信息:\n" + ex.Message, "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (DataProxyPool.GetDataProxy["DBTEST" + count].DbState == System.Data.ConnectionState.Open)
                {
                    DataProxyPool.GetDataProxy["DBTEST" + count].DbClose();
                }
            }
        }

        //保存
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.vdate() == false)
            {
                return;
            }
            System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
            sf.FileName = "data.dat";
            sf.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            sf.Filter = "数据文件 (*.dat)|*.dat";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sf.FileName;
                try
                {
                    DBConfigEntity.SaveToFile(this._DBConfigEntity, path);
                    if (File.Exists(path) == true)
                    {
                        this._DBConfigEntity = DBConfigEntity.LoadFile(path);
                    }
                    if (this._DBConfigEntity != null)
                    {
                        MessageBox.Show("保存成功\n保存位置:" + path, "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("保存失败!", "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool vdate()
        {
            this._DBConfigEntity = null;
            bool bl = true;
            if (string.IsNullOrWhiteSpace(this.textBox1.Text) == true)
            {
                bl = false;
            }
            if (File.Exists(this.textBox1.Text) == false)
            {
                bl = false;
            }
            if (string.IsNullOrWhiteSpace(this.textBox2.Text) == true)
            {
                bl = false;
            }
            try
            {
                object obj = Assembly.LoadFrom(this.textBox1.Text).CreateInstance(this.textBox2.Text);
                if (obj == null)
                {
                    bl = false;
                }
            }
            catch
            {
                bl = false;
            }


            if (string.IsNullOrWhiteSpace(this.textBox3.Text) == true)
            {
                bl = false;
            }
            if (this.textBox4.IsEnabled == true && string.IsNullOrWhiteSpace(this.textBox4.Text) == true)
            {
                bl = false;
            }
            if (this.textBox5.IsEnabled == true && string.IsNullOrWhiteSpace(this.textBox5.Text) == true)
            {
                bl = false;
            }
            if (this.textBox6.IsEnabled == true && string.IsNullOrWhiteSpace(this.textBox6.Text) == true)
            {
                bl = false;
            }

            if (bl == false)
            {
                MessageBox.Show("请检查数据后在重试", "信息提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            this._DBConfigEntity = new DBConfigEntity();
            this._DBConfigEntity.AssemblyName = this.textBox1.Text;
            this._DBConfigEntity.ClassTypeName = this.textBox2.Text;

            switch (this.cob_DBType.Text)
            {
                case "MY-SQL":
                    this._DBConfigEntity.ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", this.textBox3.Text, this.textBox6.Text, this.textBox4.Text, this.textBox5.Text);
                    break;
                case "MS-SQL":
                    this._DBConfigEntity.ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", this.textBox3.Text, this.textBox6.Text, this.textBox4.Text, this.textBox5.Text);
                    break;
                case "Oracle":
                    this._DBConfigEntity.ConnectionString = string.Format("Data Source={0}; User Id={1}; Password={2}", this.textBox3.Text, this.textBox4.Text, this.textBox5.Text);
                    break;
                default:
                    this._DBConfigEntity.ConnectionString = this.textBox3.Text;
                    break;
            }
            return true;
        }

    }
}
