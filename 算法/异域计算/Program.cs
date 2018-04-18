using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 5;
            int y = 10;

            Console.WriteLine("x={0},y={1}", x, y);

            swap(ref x, ref y);



            Console.WriteLine("x={0},y={1}", x, y);
        }

        static void swap(ref int a, ref int b)
        {
            a = a ^ b;
            b = b ^ a; //b^a相当于 b^a^b 也就是 b^a^b的值就是a了, 下边相同 
            a = a ^ b;
        }
    }
}
