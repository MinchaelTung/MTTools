using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormDelegateDemo
{
    //C#中的三种委托方式:Func委托，Action委托，Predicate委托
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void delegateFunc()
        {
            //主要是可以返回泛型
            //（1） *delegate TResult Func<TResult>(); 

            //（2）*delegate TResult Func<T1,TResult>(T1 arg1);

            //（3） *delegate TResult Func<T1,T2,TResult>(T1 arg1, T2 arg2);

            //（4）*delegate TResult Func<T1,T2,T3,TResult>(T1 arg1, T2 arg2, T3 arg3);

            //（5）*delegate TResult Func<T1,T2,T3,T4,TResult>T1 arg1, T2 arg2, T3 arg3, T4 arg4);

            Func<string, string> func = delegate(string str)
            {
                return "传入文字是： " + str;
            };

            label3.Text = func("Func方法调用");
        }

        private void delegateAction()
        {
            //没有返回值
            //（1） * delegate void Action(); 无参,无返回值

            //（2）* delegate void Action<T>(T1 arg1);

            //（3）* delegate void Action<T1,T2>(T1 arg1, T2 arg2);

            //（4）* delegate void Action<T1,T2,T3>T1 arg1, T2 arg2, T3 arg3);

            //（5）* delegate void Action<T1,T2,T3,T4>T1 arg1, T2 arg2, T3 arg3, T4 arg4);

            Action<string> action = delegate(string str)
            {
                label3.Text = "传入文字是： " + str;
            };

            action("Action方法调用");
        }
        private void delegatePredicate()
        {
            //只能返回 bool值
            Predicate<string> predicate =
                delegate(string str)
                {
                    label3.Text = "传入文字是： " + str;

                    return true;
                };
            predicate("Predicate方法调用");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            delegateFunc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            delegateAction();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            delegatePredicate();
        }

    }
}
