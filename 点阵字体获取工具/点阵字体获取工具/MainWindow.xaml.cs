using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace 点阵字体获取工具
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int matrix = 16;
        private bool[,] zi_mo_arr = new bool[24, 24];
        private int[,] zi_mo_bit = new int[3, 24];//
        Color defaultColor;
        Color fountColor;
        public MainWindow()
        {
            this.fountColor = Colors.Lime;
            this.defaultColor = Color.FromArgb(255, 0, 64, 64);
            InitializeComponent();
            initgrid();
            //initData();
        }

        //private void initData()
        //{
        //    this.cob_Font.Items.Add("宋体");
        //    this.cob_Font.Items.Add("仿宋");
        //    this.cob_Font.Items.Add("繁宋");
        //    this.cob_Font.Items.Add("宋粗体");
        //    this.cob_Font.Items.Add("黑体");
        //    this.cob_Font.Items.Add("楷体");
        //    this.cob_Font.SelectedIndex = 0;


        //}

        private void initgrid()
        {
            this.ugrid.Children.Clear();
            this.ugrid.Rows = this.matrix;
            this.ugrid.Columns = this.matrix;

            for (int i = 0; i < this.matrix * this.matrix; i++)
            {
                Grid g = new Grid();
                g.Background = new SolidColorBrush(defaultColor);
                g.Margin = new Thickness(2);
                this.ugrid.Children.Add(g);
            }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rad = sender as RadioButton;
            if (rad == null)
            {
                return;
            }
            this.matrix = Convert.ToInt32(rad.Content.ToString());
            if (this.ugrid.Rows != this.matrix)
            {
                this.initgrid();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.g();
        }

        private void g()
        {
            this.txt_result1.Text = "";
            if (string.IsNullOrWhiteSpace(this.txt_Text.Text))
            {
                return;
            }
            char[] str_arr = this.txt_Text.Text.ToArray();
            string str = str_arr[0] + "";
            byte[] buf = Encoding.Default.GetBytes(str);
            int k = 0;
            for (int i = 0, j = 0; i < buf.Length; i += 2, j++)
            {
                if (buf[i] >= 161 && buf[i] <= 247)
                {
                    this.start_byte(buf[i], buf[i + 1]);
                    k++;
                }
            }
            if (k != 0)//有全角字符
            {
                gFount();
                this.txt_result1.Text = string.Format("当前字为: {0} 字体：宋体 大小:{1}\r\n", str, this.matrix);
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < this.matrix; j++)
                {
                    for (int i = 0; i < (this.matrix + 4) / 8; i++)
                    {
                        sb.AppendFormat("0x{0:X2}, ", this.zi_mo_bit[i, j]);
                    }
                    sb.Append("\r\n");
                }
                this.txt_result1.Text += sb.ToString();

            }
        }

        private void gFount()
        {
            int index = 0;
            for (int i = 0; i < this.matrix; i++)
            {
                for (int j = 0; j < this.matrix; j++)
                {
                    if (this.zi_mo_arr[j, i] == true)
                    {
                        ((Grid)this.ugrid.Children[index]).Background = new SolidColorBrush(fountColor);
                    }
                    else
                    {
                        ((Grid)this.ugrid.Children[index]).Background = new SolidColorBrush(defaultColor);
                    }
                    index++;
                }
            }
        }
        private void start_byte(byte a, byte b)
        {
            int temp;
            long k;
            if (matrix == 12)//点阵为12的例外
            {
                temp = 24;
            }
            else
            {
                temp = this.matrix * this.matrix / 8;
            }
            //计算区位码
            k = ((a - 160 - 1) * 94 + (b - 160 - 1)) * temp;
            //根据点阵大小选择不同的计算函数
            zi_mo_bit = new int[3, 24];//清空点阵保存容器
            this.hzk(k);

            //把字节数组赋值到点阵数组 

            for (int j = 0; j < 24; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    int temp2 = 128;
                    for (int k2 = 0; k2 < 8; k2++)
                    {
                        this.zi_mo_arr[i * 8 + k2, j] = (this.zi_mo_bit[i, j] / temp2 % 2 == 1);
                        temp2 /= 2;
                    }
                }
            }

        }

        private void hzk(long i)
        {
            string path = "宋体" + this.matrix;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    fs.Seek(i, SeekOrigin.Begin);
                    for (int j = 0; j < matrix; j++)
                    {
                        this.zi_mo_bit[0, j] = r.ReadByte();
                        this.zi_mo_bit[1, j] = r.ReadByte();
                        if (this.matrix == 24)
                        {
                            this.zi_mo_bit[2, j] = r.ReadByte();
                        }
                    }
                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.txt_result1.Text = "";
            this.initgrid();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.initgrid();
            g();
        }

    }
}
