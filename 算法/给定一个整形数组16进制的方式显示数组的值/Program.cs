using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 给定一个整形数组16进制的方式显示数组的值
{
    class Program
    {
        static void Main(string[] args)
        {
            Array arr1 = new short[] { 255, 255, 255 };
            DisplayArrayValues(arr1);
            Array arr2 = new byte[] { 255, 255, 255 };
            DisplayArrayValues(arr2);
            Console.ReadLine();
        }

        /*
         * 4：给定一个整形数组，请用16进制的方式显示数组的值。
         * 比方：一个short类型数组:[255,255,255],输出的结果为 00FF 00FF 00FF,如果是byte类型，则输出为 FF FF FF
         */

        public static void DisplayArrayValues(Array arr)
        {
            int elementLength = Buffer.ByteLength(arr) / arr.Length;
            string formatString = String.Format("{{0:X{0}}} ", 2 * elementLength);
            for (int ctr = 0; ctr < arr.Length; ctr++)
                Console.Write(formatString, arr.GetValue(ctr));
            Console.WriteLine();
        }
    }
}
