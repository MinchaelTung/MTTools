

/*
C# 中的#if、#elif、#else、#endif等条件编译符号

这些是C#中的条件编译符号。这些指令我在项目中遇到过，查过网络，问过人（当然，既不认识大牛，也不认识小牛，所以没什么收获）。
今天翻看一本资料，有提到这个方面的东西，所以写下来和能看到这篇文章的人一起学习。
  
C#中的#define预处理指令不能定义替换常数。该指令现在只能定义用于条件编译的符号，为#if、#elif、#else及#endif所用。
源文件在进行编译之前要进行一次文件处理，这是由预处理器完成的。这种文件处理只对源文件进行文本处理而不进行任何编译动作。所有预处理指令都以“#”符号开头。
预处理识别以下指令识别以下指令：
   #define #undef #if #elif  #else  #endif #error  #warning #line #region #endregion
   #pragma warning disable       #pragma warning restore
   #if #elif  #else  #endif 指令可以完成，使用#define指令来定义一些指导预处理器修改原代码的符号的工作。

下面是使用条件编译的实例：
*/
#define MACRO1
//#define MACRO2
#if (MACRO1 && MACRO2)
#define BOTH
#else
#define BOTH2 //判断上这个必须在第一行
#endif
using System;
using System.Diagnostics;

namespace 条件编译符号使用
{


    class Program
    {
        static void demo1()
        {
#if (MACRO1)
            Console.WriteLine("MACRO1 is defined.");
#elif (MACRO2)
            Console.WriteLine("MACRO2 is defined and MACRO1 is not defined");
#else
            Console.WriteLine("MACRO2  and MACRO1 are both defined");
#endif
        }

        [Conditional("MACRO1")]
        static void demo21()
        {

            Console.WriteLine("MACRO1 is defined.");

        }

        [Conditional("MACRO2")]
        static void demo22()
        {
            Console.WriteLine("MACRO2 is defined and MACRO1 is not defined");
        }


        //其中一个存在都会执行？
        [Conditional("MACRO1"), Conditional("MACRO2")]
        static void demo23()
        {
            Console.WriteLine("MACRO2 or MACRO1 is defined");

        }




        //两个存在才会执行
        [Conditional("BOTH")]
        static void demo24()
        {
            Console.WriteLine("BOTH  is defined ");

        }

        //两个不存在才会执行
        [Conditional("BOTH2")]
        static void demo25()
        {
            Console.WriteLine("BOTH2  is defined ");

        }
        //在Debug 编译情况下 默认有 DEBUG  在 Release 编译情况下 默认有 RELEASE
        //建议使用 System.Diagnostics.Conditional



        static void Main(string[] args)
        {           
            //demo1();
            demo21();
            demo22();
            demo23();
            demo24();
            demo25();

            //在此例中，Obsolete 属性应用于类 A 和方法 B.OldMethod。由于应用于 B.OldMethod 的属性构造函数的第二个参数设置为 true，因此使用此方法将导致编译器错误，而使用类 A 只会产生警告。但是，调用 B.NewMethod 既不产生警告也不产生错误。

            //向属性构造函数提供的作为第一个参数的字符串将显示为警告或错误的一部分。例如，当与前面的定义一起使用时，下面的代码将生成两个警告和一个错误：
            // Generates 2 warnings:

            A aobj = new A(); //提示A类已过期请使用B类
            B bobj = new B();
            //bobj.OldMethod();//标记OldMethod方法已过时并不能使用编译器提示错误,请使用NewMethod方法;
            //为类 A 产生两个警告：一个用于声明类引用，一个用于类构造函数。
            //可在不使用参数的情况下使用 Obsolete 属性，但要包括此项已过时的原因及改用什么项的建议。
            bobj.NewMethod();

            demo3();
            Console.ReadLine();
        }


        static void demo3()
        {
            BaseClass b = new BaseClass();
            DerivedClass d = new DerivedClass();

            // Display custom attributes for each class.
            Console.WriteLine("Attributes on Base Class:");
            object[] attrs = b.GetType().GetCustomAttributes(true);
            foreach (Attribute attr in attrs)
            {
                Console.WriteLine(attr);
            }

            Console.WriteLine("Attributes on Derived Class:");
            attrs = d.GetType().GetCustomAttributes(true);
            foreach (Attribute attr in attrs)
            {
                Console.WriteLine(attr);
            }
        }


    }
    

    //Obsolete 属性将某个程序实体标记为一个建议不再使用的实体。每次使用被标记为已过时的实体时，随后将生成警告或错误，这取决于属性是如何配置的。例如：
    [System.Obsolete("use class B")]//提示A类已过期请使用B类但可以使用
    class A
    {
        public void Method() { }
    }
    class B
    {
        [System.Obsolete("use NewMethod", true)]//标记OldMethod方法已过时并不能使用,请使用NewMethod方法;
        public void OldMethod() { }
        public void NewMethod() { }
    }
    
    //查询对象有什么 Attribute
    //AttributeUsage使用
    //下面的示例将阐释 Inherited 参数和 AllowMultiple 参数对 AttributeUsage 属性的效果，以及如何才能枚举应用于类的自定义属性。
    // Create some custom attributes:
    [AttributeUsage(System.AttributeTargets.Class, Inherited = false)]//不会衍生到继承类;Inherited默认=true
    class A1 : System.Attribute {      
    }

    [AttributeUsage(System.AttributeTargets.Class)]
    class A2 : System.Attribute { }

    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]//同一个类或方法可以多次定义
    class A3 : System.Attribute { }

    // Apply custom attributes to classes:
    [A1, A2]
    class BaseClass { }

    [A3, A3]
    class DerivedClass : BaseClass { }

}
