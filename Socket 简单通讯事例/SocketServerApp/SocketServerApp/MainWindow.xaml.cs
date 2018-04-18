using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
namespace SocketServerApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket _ServerSocket;//服务端
        private IPEndPoint _ServerIPEndPoint;//服务端IP地址
        private int backlog = int.MaxValue;//监听数量
        private Thread _ServerAcceptThread;//多线程获取连接用户
        private bool _IsServerAcceptThread = false;//线程旗标
        private List<ClientInfo> _ClientInfoList = new List<ClientInfo>();//用户列表


        public MainWindow()
        {
            InitializeComponent();
            this.initData();
        }

        private void initData()
        {
            //this.cob_User.IsEnabled = false;
            this.cob_User.DisplayMemberPath = "UserName";
            this.cob_User.SelectedValuePath = "Client";
            this.cob_User.ItemsSource = _ClientInfoList;
        }

        private void startServer(int port)
        {
            //实例服务
            this._ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获取本机IP用于显示
            IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());           
            string ip = "0.0.0.0";
            foreach (var item in ipe.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = item.ToString();
                    break;
                }
            }
            //本机预使用的IP和端口
            this._ServerIPEndPoint = new IPEndPoint(IPAddress.Any, port);
            //端口绑定
            this._ServerSocket.Bind(this._ServerIPEndPoint);
            //设置监听数量
            this._ServerSocket.Listen(this.backlog);

            this.txtStatus.Text = string.Format("服务器IP:{0} 端口:{1}", ip, this._ServerIPEndPoint.Port);
            _IsServerAcceptThread = true;
            //使用线程获取多个客户端
            this._ServerAcceptThread = new Thread(acceptClient);
            this._ServerAcceptThread.Start();


        }

        private void acceptClient()
        {
            try
            {
                while (this._IsServerAcceptThread)
                {
                    //等待到来的客户端
                    Socket client = this._ServerSocket.Accept();
                    //获取连接的客户端IP
                    IPEndPoint clientip = (IPEndPoint)client.RemoteEndPoint;
                    //发送信息
                    client.Send(Encoding.Unicode.GetBytes("connected"));
                    //回接信息容器
                    byte[] rec = new byte[1024];
                    //等待回接信息并返回长度
                    int len = client.Receive(rec);                   
                    string name = Encoding.Unicode.GetString(rec, 0, len);
                    //保存客户端的容器
                    ClientInfo clientinfo = new ClientInfo();
                    clientinfo.Client = client;
                    clientinfo.ClientIP = clientip;
                    clientinfo.UserName = name;
                    clientinfo.ReceiveMsg += clientinfo_ReceiveMsg;
                    if (clientinfo.StartListenReceive() != 1)
                    {
                        clientinfo.Dispose();
                        continue;
                    }
                    var result = this._ClientInfoList.Where(s => s.UserName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
                    if (result.Count() > 0)
                    {
                        clientinfo.SendMsg("当前昵称已存在");
                        clientinfo.Dispose();
                        continue;
                    }
                    this._ClientInfoList.Add(clientinfo);
                    //使用代理返回到UI界面线程
                    MsgCallback callback = MsgCallbackDisplay;
                    //WPF的进入当前界面主线程方法
                    this.Dispatcher.Invoke(callback, string.Format("用户：{0}，登陆到服务器\n\r", clientinfo.UserName));
                }
            }
            catch (Exception)
            {

            }
        }

        void clientinfo_ReceiveMsg(object sender, ReceiveEvent e)
        {
            //连接信息
            ClientInfo clientInfo = sender as ClientInfo;
            if (clientInfo == null)
            {
                return;
            }
            MsgCallback callback = MsgCallbackDisplay;
            if (e.Connect == false)
            {
                this.Dispatcher.Invoke(callback, string.Format("当前用户：{0},{1}\n\r", e.UserName, e.ReaceiveStr));
                this._ClientInfoList.Remove(clientInfo);
                return;
            }
            this.Dispatcher.Invoke(callback, string.Format("{0}\n\r  {1}\n\r\n\r", e.UserName, e.ReaceiveStr));

        }

        delegate void MsgCallback(string str);
        //外界线程返回到主线程显示方法
        private void MsgCallbackDisplay(string str)
        {
            this.txtReceiveMsg.Text += str;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.txtStatus.Text = "";
            //启动
            int port = 0;
            if (int.TryParse(this.txtPort.Text, out port) == false || port < 80)
            {
                this.txtStatus.Text = "端口错误！";
                return;
            }

            this.startServer(port);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            closeServer();
        }

        private void txtReceiveMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txtReceiveMsg.ScrollToEnd();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeServer();

        }

        //关闭服务和线程
        private void closeServer()
        {
            _IsServerAcceptThread = false;
            foreach (var item in this._ClientInfoList)
            {
                item.Dispose();
            }

            if (this._ServerSocket != null)
            {
                //this._ServerSocket.Shutdown(SocketShutdown.Both);
                this._ServerSocket.Close();
                this._ServerSocket.Dispose();
                this._ServerSocket = null;
            }
            if (this._ServerAcceptThread != null && this._ServerAcceptThread.IsAlive == true)
            {
                this._ServerAcceptThread.Abort();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(this.txtSendMsg.Text) == true)
            {
                return;
            }
            if (!(this.cob_User.SelectedItem is ClientInfo))
            {
                MessageBox.Show("请选择对话用户");
                return;
            }

            ClientInfo clientInfo = this.cob_User.SelectedItem as ClientInfo;

            clientInfo.SendMsg(this.txtSendMsg.Text);


        }

    }
}
