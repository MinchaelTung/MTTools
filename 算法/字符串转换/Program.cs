using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 字符串转换
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal d = 3366;
            int dd = StringToInt(d.ToString());

            Console.WriteLine(dd); ;
            Console.ReadLine();

        }
        //5：请自行实现一个函数，该函数的功能是将用户输入的numeric string 转换为integer。
       public static int StringToInt(string str)
        {
            int result = 0;
            if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^-?[0-9]\d*"))
            {
                bool isNegative = false;
                if (str.IndexOf('-') != -1)
                {
                    str = str.Substring(1);
                    isNegative = true;
                }
                for (int i = 0; i < str.Length; i++)
                {
                    result = result * 10 + (str[i] - '0');
                }
                result = isNegative ? result * -1 : result;
            }
            return result;
        }

    }
}
