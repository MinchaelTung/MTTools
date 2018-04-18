using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 在控制台中打印出1_200这200个数
{
    class Program
    {
        static void Main(string[] args)
        {
            //1：不允许使用循环语句、条件语句，在控制台中打印出1-200这200个数。
            //Print1(1);
            //Print2(1);
            // 2.增加条件不适用递归
            Print3();

            Console.ReadLine();
        }

        //1：不允许使用循环语句、条件语句，在控制台中打印出1-200这200个数。
        //方案1
        public static void Print1(int number)
        {
            try
            {
                Console.WriteLine(number);
                int i = 1 / (200 - number);
                number = number + 1;
                Print1(number);
            }
            catch (DivideByZeroException)
            {
            }
        }
        //比简短的方案
        public static bool Print2(int number)
        {
            Console.WriteLine(number);
            return number >= 200 || Print2(number + 1);
        }

        // 2.增加条件不适用递归
        public static void Print3()
        {
            Enumerable.Range(1, 200).ToList().ForEach(n => Console.WriteLine(n));
        }
    }
}
