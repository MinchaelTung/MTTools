using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DemoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //int recv; //用于表示客户端发送的信息长度
            byte[] data = new byte[1024];//用于缓存客户端所发送的信息,通过socket传递的信息必须为字节数组
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);//本机预使用的IP和端口
            //实例服务
            Socket _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _Socket.Bind(ipep);//绑定
            _Socket.Listen(int.MaxValue);//监听列队长度
            Console.WriteLine("waiting for a client");
            Socket client = _Socket.Accept();//当有可用的客户端连接尝试时执行，并返回一个新的socket,用于与客户端之间的通信
            IPEndPoint clientip = (IPEndPoint)client.RemoteEndPoint;//获取用户IP地址信息和通讯端口
            Console.WriteLine("connect with client:" + clientip.Address + " at port:" + clientip.Port);
            string welcome = "welcome here!";
            data = Encoding.ASCII.GetBytes(welcome);
            //发送信息到用户     ;
            client.Send(data);//发送信息
            try
            {
                while (true)
                {
                    //用死循环来不断的从客户端获取信息
                    data = new byte[1024];
                    //等待用户信息
                    int recv = client.Receive(data);
                    if (recv == 0)//当信息长度为0，说明客户端连接断开
                    {
                        break;
                    }
                    Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                    //发送用户信息
                    client.Send(data, recv, SocketFlags.None);
                }
            }
            catch (Exception)
            {

            }
            Console.WriteLine("Disconnected from" + clientip.Address);
            //关闭客户端连接
            client.Close();
            //关闭服务端连接
            _Socket.Close();
        }
    }
}
