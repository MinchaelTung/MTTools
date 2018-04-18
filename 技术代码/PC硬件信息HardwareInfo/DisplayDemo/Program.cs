using System;
using System.Collections.Generic;
using System.Text;
using ComputerHardwareInfo;
using System.Management;
using System.Runtime.InteropServices;

namespace DisplayDemo
{
    class Program
    {
        private const long INTERNET_CONNECTION_MODEM = 1;
        private const long INTERNET_CONNECTION_LAN = 2;
        private const long INTERNET_CONNECTION_PROXY = 4;
        private const long INTERNET_CONNECTION_MODEM_BUSY = 8;


        [DllImport("wininet.dll ")]
        public static extern bool InternetGetConnectedState(out   long lpdwFlags, long dwReserved); 

        static void Main(string[] args)
        {
            //List<string> cpus = ComputerHardwareInfo.ComputerHardwareInfo.GetHD_Ids();


            //foreach (var item in cpus)
            //{
            //    Console.WriteLine(item);
            //}

            //string d = IDE.Read(0);
            //Console.WriteLine(d);

            //Win32_OperatingSystem
            //ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            //ManagementObjectCollection queryCollection = searcher.Get();
            //foreach (ManagementObject m in queryCollection)
            //{
            //    foreach (var item in m.Properties)
            //    {
            //        Console.WriteLine("{0} : {1}", item.Name, item.Value);
            //    }

            //    Console.WriteLine("---------------------------------------------");
            //}

            long lfag;
            string strConnectionDev = " ";
            if (InternetGetConnectedState(out   lfag, 0))
                strConnectionDev = "在线呀！！用的是   ";
            else
                strConnectionDev = "不在线呀！！ ";

            if ((lfag & INTERNET_CONNECTION_MODEM) > 0)
                strConnectionDev += "Modem ";
            if ((lfag & INTERNET_CONNECTION_LAN) > 0)
                strConnectionDev += "LAN ";
            if ((lfag & INTERNET_CONNECTION_PROXY) > 0)
                strConnectionDev += "a   Proxy ";
            if ((lfag & INTERNET_CONNECTION_MODEM_BUSY) > 0)
                strConnectionDev += "Modem   but   modem   is   busy ";

            Console.WriteLine(strConnectionDev);

            Console.ReadLine();
        }
    }
}
