using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SocketClientApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //连接服务的IP地址
        private IPAddress _IPAddress;
        //连接服务的端口
        private int port;
        //连接服务的通讯对象
        private IPEndPoint _IPEndPoint;
        //套接器
        private Socket _ClientSocket;
        //接收信息的线程
        private Thread _Thread;
        //线程标记
        private bool _F = false;
        //接收数据包
        private byte[] data = new byte[1024];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IPAddress.TryParse(this.txtIP.Text, out _IPAddress) == false)
            {
                MessageBox.Show("IP错误");
                return;
            }
            if (int.TryParse(this.txtPort.Text, out port) == false)
            {
                MessageBox.Show("端口错误");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtName.Text) == true)
            {
                MessageBox.Show("昵称不能为空");
                return;
            }
            try
            {
                //创建套接器
                _ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建服务通讯对象
                this._IPEndPoint = new IPEndPoint(this._IPAddress, port);
                //连接
                this._ClientSocket.Connect(this._IPEndPoint);
                this._F = true;
                byte[] data = new byte[1024];
                int len = this._ClientSocket.Receive(data);
                string recstr = Encoding.Unicode.GetString(data, 0, len);
                if (recstr.Equals("connected", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    if (this._Thread != null)
                    {
                        this._F = false;
                        this._Thread = null;
                    }
                    if (this._IPEndPoint != null)
                    {
                        this._IPEndPoint = null;
                    }
                    if (this._ClientSocket != null)
                    {
                        this._ClientSocket.Close();
                        this._ClientSocket.Dispose();
                        this._ClientSocket = null;
                    }
                    MessageBox.Show("连接服务器错误，请确认服务器端口是否正确！");
                }
                this.txtMsgResult.Text += "连接服务器成功\n\r";
                byte[] senddata = Encoding.Unicode.GetBytes(this.txtName.Text);

                this._ClientSocket.Send(senddata);

                this._Thread = new Thread(ReceiveMeg);
                this._Thread.Start();
            }
            catch (Exception)
            {
                if (this._Thread != null)
                {
                    this._F = false;
                    this._Thread = null;
                }
                if (this._IPEndPoint != null)
                {
                    this._IPEndPoint = null;
                }
                if (this._ClientSocket != null)
                {
                    this._ClientSocket.Close();
                    this._ClientSocket.Dispose();
                    this._ClientSocket = null;
                }
                MessageBox.Show("连接失败");

            }
        }

        private void ReceiveMeg()
        {
            try
            {
                while (this._F)
                {
                    int reslen = this._ClientSocket.Receive(data);

                    if (reslen == 0)
                    {
                        continue;
                    }

                    string str = Encoding.Unicode.GetString(data, 0, reslen);
                    MsgCallback callback = MsgCallbackDisplay;
                    this.Dispatcher.Invoke(callback, str);
                }
            }
            catch (Exception)
            {

            }

        }


        void receive_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0)
            {
                return;
            }
            string str = string.Format("{0}\n\r", Encoding.Unicode.GetString(e.Buffer, 0, e.BytesTransferred));
            MsgCallback d = MsgCallbackDisplay;
            this.Dispatcher.Invoke(d, str);
        }

        delegate void MsgCallback(string str);

        private void MsgCallbackDisplay(string str)
        {
            this.txtMsgResult.Text += string.Format("{0}\n\r", str);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtSend.Text) == true)
            {
                return;
            }
            byte[] send = Encoding.Unicode.GetBytes(this.txtSend.Text);
            this._ClientSocket.Send(send);
            this.txtSend.Text = "";
        }



        private void txtMsgResult_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txtMsgResult.ScrollToEnd();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this._F = false;
            if (this._ClientSocket != null)
            {
                this._ClientSocket.Shutdown(SocketShutdown.Both);
                this._ClientSocket.Close();
                this._ClientSocket.Dispose();
            }
            if (this._Thread != null && this._Thread.IsAlive == true)
            {
                this._Thread.Abort();
            }
            this._Thread = null;
        }

    }
}
