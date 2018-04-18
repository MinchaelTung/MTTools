using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 判断这个整数是否是2的N次方
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 8;
            bool bl = GetFlag(num);
            Console.WriteLine(bl);
            Console.ReadLine();

        }
        //6：给定一个整数num，判断这个整数是否是2的N次方。
        public static bool GetFlag(int num)
        {
            if (num < 1) return false;
            //使用 位与运算方法判断
            return (num & num - 1) == 0;
        }
    }
}
