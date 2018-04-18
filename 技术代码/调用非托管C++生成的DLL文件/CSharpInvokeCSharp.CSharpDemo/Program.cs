using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices; 
using System.Text;
using System.Threading.Tasks;

namespace CSharpInvokeCSharp.CSharpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int result = CPPDLL.Add(10, 20);
            Console.WriteLine("10 + 20 = {0}", result);

            result = CPPDLL.Sub(30, 12);
            Console.WriteLine("30 - 12 = {0}", result);

            result = CPPDLL.Multiply(5, 4);
            Console.WriteLine("5 * 4 = {0}", result);

            result = CPPDLL.Divide(30, 5);
            Console.WriteLine("30 / 5 = {0}", result);

            Console.ReadLine();
        }
    }
    //DllImport作为C#中对C++的DLL类的导入入口特征，并通过static extern对extern “C”进行对应。
    //8. 另外，记得把CPPDemo中生成的DLL文件拷贝到CSharpDemo的bin目录下，
    //你也可以通过设置【项目属性】->【配置属性】->【常规】中的输出目录：
    public class CPPDLL
    {
        [DllImport("CSharpInvokeCPP.CPPDemo.dll")]
        public static extern int Add(int x, int y);

        [DllImport("CSharpInvokeCPP.CPPDemo.dll")]
        public static extern int Sub(int x, int y);

        [DllImport("CSharpInvokeCPP.CPPDemo.dll")]
        public static extern int Multiply(int x, int y);

        [DllImport("CSharpInvokeCPP.CPPDemo.dll")]
        public static extern int Divide(int x, int y);
    }
}
