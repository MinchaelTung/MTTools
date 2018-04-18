using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 牧场羊数目计算
{
    class Program
    {
        static void Main(string[] args)
        {
            int sheepCount;
            for (int i = 1; i <= 20; i++)
            {
                sheepCount = GetSheepCount(i) - GetDeathSheepCount(i);
                Console.WriteLine(sheepCount);
            }
            Console.ReadLine();
        }
        /*
         * 8：一个牧场目前一共有20头刚出生的羊，母羊、公羊各一半。
         * 假如母羊5岁时后每年生一胎（母羊,公羊各一半）。
         * 羊活到10岁后死亡。请问20年后这个牧场有多少只羊？ 
         * 
         */ 
        public static int GetSheepCount(int year)
        {
            if (year <= 4)
                return 20;
            return GetSheepCount(year - 1) + GetSheepCount(year - 5) - GetDeathSheepCount(year);
        }

        public static int GetDeathSheepCount(int year)
        {
            if (year < 10)
                return 0;
            return GetSheepCount(year - 10);
        }
    }
}
