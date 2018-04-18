using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 重复N倍输出字符串的功能
{
    class Program
    {
        static void Main(string[] args)
        {
          string tmp=  RepeatString("ABC", 5);
          Console.WriteLine(tmp);
            Console.ReadLine();
        }

        /*
         * 3：给定一个字符串，试编写代码，实现重复N倍输出字符串的功能。
         * 这个题目要注意的是char在C#中占用的是两个字节。
         */
        public static string RepeatString(string str, int repeatCount)
        {
            var source = str.ToCharArray();
            var dest = new char[source.Length * repeatCount];
            for (int i = 0; i < repeatCount; i++)
            {
                Buffer.BlockCopy(source, 0, dest, source.Length * i * 2, source.Length * 2);
            }
            return new String(dest);
        }
    }
}
