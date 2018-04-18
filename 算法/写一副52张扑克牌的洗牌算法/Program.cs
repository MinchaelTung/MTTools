using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 写一副52张扑克牌的洗牌算法
{
    class Program
    {
        static void Main(string[] args)
        {
            Shuffle();

            Console.ReadLine();
        }
        /*
         * 7：试编写一副52张扑克牌的洗牌算法。
         * 提示：每次洗牌的时候，给牌一个随机因子。
         */
        public static void Shuffle()
        {
            var random = new Random();
            var result = new List<string>();
            string[] cardType = { "红桃", "黑桃", "方块", "梅花" };
            string[] cardValue = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            foreach (string type in cardType)
            {
                var list = cardValue.Select(value => string.Format("{0}{1}", type, value));
                result.AddRange(list);
            }
            result = (from c in result orderby random.Next(0, 51) descending select c).ToList();
            result.ForEach(str => Console.WriteLine("{0},", str));
        }

    }
}
