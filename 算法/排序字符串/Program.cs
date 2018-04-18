using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 排序字符串
{
    class Program
    {
        static void Main(string[] args)
        {
            Order();
            Console.ReadLine();
        }


        /*
         * 2：有5个Aspx页面，分别为"Page_1.aspx","Page_10.aspx","Page_100.aspx","Page_11.aspx","Page_111.aspx",
         * 请编写代码，让5个Aspx页面按下面的顺序输出:
         * Page_1.aspx
         * Page_10.aspx
         * Page_11.aspx
         * Page_100.aspx
         * Page_111.aspx
         */
        public static void Order()
        {
            var pageList = new[] { "Page_10.aspx", "Page_100.aspx", "Page_1.aspx", "Page_11.aspx", "Page_111.aspx" };
            pageList = pageList.OrderBy(s => int.Parse(System.Text.RegularExpressions.Regex.Match(s, @"\d+").Value)).ToArray();
            Array.ForEach(pageList, Console.WriteLine);
        }
    }
}
