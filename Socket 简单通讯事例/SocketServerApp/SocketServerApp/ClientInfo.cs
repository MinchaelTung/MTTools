using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServerApp
{
    public class ClientInfo : IDisposable
    {
        public delegate void ReceiveHandler(object sender, ReceiveEvent e);
        public event ReceiveHandler ReceiveMsg;
        private bool _IsConnect = false;
        private Thread _ReceiveThread;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户连接服务
        /// </summary>
        public Socket Client { get; set; }
        //用户IP地址信息
        public IPEndPoint ClientIP { get; set; }
        //发送信息到用户
        public bool SendMsg(string str)
        {
            if (Client == null)
            {
                return false;
            }

            try
            {
                byte[] send = Encoding.Unicode.GetBytes(str);
                this.Client.Send(send);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //启动线程监听用户发送的消息
        public int StartListenReceive()
        {
            if (this.ReceiveMsg == null)
            {
                return -1;
            }
            if (this._ReceiveThread != null && this._ReceiveThread.IsAlive == true)
            {
                return 1;
            }
            try
            {
                this._IsConnect = true;
                this._ReceiveThread = new Thread(receiveData);
                this._ReceiveThread.Start();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void receiveData()
        {
            try
            {

                while (this._IsConnect)
                {

                    if (this.ReceiveMsg == null)
                    {
                        this.Disconnect();
                        return;
                    }
                    byte[] rec = new byte[1024];
                    //等待用户信息
                    int len = this.Client.Receive(rec);
                    if (len == 0)
                    {
                        this.ReceiveMsg(this, new ReceiveEvent() { ReaceiveStr = "退出服务", Connect = false, UserName = this.UserName });
                        this.Disconnect();
                    }

                    string str = Encoding.Unicode.GetString(rec, 0, len);
                    this.ReceiveMsg(this, new ReceiveEvent() { Connect = true, ReaceiveStr = str, UserName = this.UserName });

                }

            }
            catch (Exception)
            {
                
            }
        }

        //关闭用户连接
        public void Disconnect()
        {
            this._IsConnect = false;
            if (this._ReceiveThread == null)
            {
                return;
            }
            if (this._ReceiveThread != null && this._ReceiveThread.IsAlive==true)
            {
                this._ReceiveThread.Abort();
            }

            this._ReceiveThread = null;

            if (this.Client != null)
            {
                this.Client.Shutdown(SocketShutdown.Both);
                this.Client.Close();
                this.Client.Dispose();
                this.Client = null;
            }

        }


        //析构函数
        public void Dispose()
        {
            this.Disconnect();
        }
    }


    public class ReceiveEvent : EventArgs
    {
        public string UserName { get; internal set; }

        public string ReaceiveStr { get; internal set; }

        public bool Connect { get; internal set; }
    }

}
