using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自定义委托和事件
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始");
            DemoClass demo = new DemoClass();
            demo.DemoCallBack += demo_DemoCallBack;

            demo.DemoTest();

            Console.WriteLine("结束");
            Console.ReadLine();
        }

        static void demo_DemoCallBack(object obj)
        {
            Console.WriteLine(obj.ToString());
        }
    }

    public delegate void OnDemoCallBack(object obj);
    public class DemoClass {
        public  event OnDemoCallBack DemoCallBack;


        public void DemoTest()
        {
            Console.WriteLine("进入 DemoTest");
            if (DemoCallBack != null)
            {
                DemoCallBack("调用事件");
            }
        }

    }

}
