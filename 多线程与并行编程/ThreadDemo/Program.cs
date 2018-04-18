using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            //demo d = new demo();
            //d.TestAutoResetEvent();


            Paralleldemo p = new Paralleldemo();
            p.TaskWhenAllDemo();
            //p.TaskWhenAnyDemo();
            //p.Pdem1();
            Console.ReadLine();
        }


        /// <summary>
        /// 线程池
        /// </summary>
        void threadpooluse()
        {
            //启动一个线程池的线程任务
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                //处理在这个线程中的任务
            }));


            //启动另外的一个线程池的线程任务
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                //处理在这个线程中的任务
                int num = (int)state;
            }), 123);
        }


        //线程锁使用
        //锁旗标
        object lockThis = new object();
        public void process()
        {
            //使用lock 来锁住线程唯一的调用
            lock (lockThis)
            {
                //访问线程的公共资源
            }
        }


        /// <summary>
        /// 简单的线程使用 
        /// </summary>
        void threaduse()
        {
            //线程实例对象，参数=需要调用的函数名称
            Thread t = new Thread(tag); //就绪 runnable

            t.Start();//运行 running

            t.Abort();//死亡 dead

            Thread.Sleep(99);//堵塞 blocked

        }
        /// <summary>
        /// 线程调用的方法 
        /// </summary>
        void tag()
        {
            Thread.Sleep(1000);
            Console.WriteLine("线程调用的");
        }
    }

    public class demo
    {
        //同步事件和等待句柄

        //事例1使用 AutoResetEvent

        AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 效果：等待3秒后 其中一个线程会执行，另一个线程继续等待
        /// </summary>
        public void TestAutoResetEvent()
        {
            Thread t1 = new Thread(() =>
            {
                _AutoResetEvent.WaitOne();
                Console.WriteLine("线程1完成");
            });

            t1.Start();
            Thread t2 = new Thread(() =>
            {
                _AutoResetEvent.WaitOne();
                Console.WriteLine("线程2完成");
            });
            t2.Start();
            Thread.Sleep(3000);
            _AutoResetEvent.Set();
            //可以多次启动
            //Thread.Sleep(3000);
            //_AutoResetEvent.Set();

        }


        //事例2 使用 ManualResetEvent
        ManualResetEvent _ManualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// 效果： 几个线程同时执行,控制下的线程都会结束.
        /// </summary>
        public void TestManualResetEvent()
        {
            Thread t1 = new Thread(() =>
            {
                _ManualResetEvent.WaitOne();
                Console.WriteLine("线程1完成");
            });
            t1.Start();
            Thread t2 = new Thread(() =>
            {

                _ManualResetEvent.WaitOne();
                Console.WriteLine("线程2完成");
            });
            t2.Start();
            Thread.Sleep(3000);
            _ManualResetEvent.Set();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Paralleldemo
    {
        //数据并行
        public void Pdem1()
        {
            List<int> collection = new List<int>() { 1, 2, 3, 4, 56, 8, 9, 25, 12 };

            ////非并行编程的写法
            //foreach (var item in collection)
            //{
            //    Progress(item.ToString());
            //}

            //并行编程写法
            Parallel.ForEach(collection, item => Progress(item.ToString()));
        }
        void Progress(string nn)
        {
            Console.WriteLine(nn);
        }
        //任务并行
        /// <summary>
        /// 1.等待所有任务完成的任务:Task.When.All 多个任务完成后才算完成
        /// </summary>
        public void TaskWhenAllDemo()
        {
            //开始多个任务
            Task taskA = Task.Run(() => { Thread.Sleep(3000); Console.WriteLine("线程1"); });
            Task taskB = Task.Run(() => Console.WriteLine("线程2"));
            Task taskC = Task.Run(() => Console.WriteLine("线程3"));

            Task joinTask = Task.WhenAll(taskA, taskB, taskC);
            Console.WriteLine("输出数据");
            joinTask.Wait();

            Console.WriteLine("完成");
        }

        /// <summary>
        /// 只要一个任务完成就完成任务
        /// </summary>
        public async void TaskWhenAnyDemo()
        {
            Task<int> taskA = Task.Run(() => 78);
            Task<int> taskB = Task.Run(() => 82);
            Task<int> taskC = Task.Run(() => 99);

            Task<Task<int>> first = Task.WhenAny(taskA, taskB, taskC);

            Console.WriteLine("输出结果");

            Console.WriteLine((await first).Result);

        }

    }

}
