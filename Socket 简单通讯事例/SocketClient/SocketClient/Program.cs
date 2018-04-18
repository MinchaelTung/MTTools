using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            //创建服务对象
            Socket newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.Write("please input the server ip:");
            string ipadd = Console.ReadLine();
            Console.WriteLine();
            Console.Write("please input the server port:");
            int port = Convert.ToInt32(Console.ReadLine());

            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipadd), port);//服务器的IP和端口
            try
            {
                //因为客户端只是用来向特定的服务器发送信息，所以不需要绑定本机的IP和端口。不需要监听。
                newclient.Connect(ie);
            }
            catch (SocketException e)
            {
                Console.WriteLine("unable to connect to server");
                Console.WriteLine(e.ToString());
                return;
            }
            //获取服务端发送的数据
            int recv = newclient.Receive(data);
            string stringdata = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(stringdata);
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
                //发送信息到服务端
                newclient.Send(Encoding.ASCII.GetBytes(input));
                data = new byte[1024];
                //获取服务端返回信息
                recv = newclient.Receive(data);
                stringdata = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(stringdata);
            }
            Console.WriteLine("disconnect from sercer");
            newclient.Shutdown(SocketShutdown.Both);
            newclient.Close();
        }
    }
}
