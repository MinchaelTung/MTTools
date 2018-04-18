
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace 反射事例
{
    class Program
    {
        static void Main(string[] args)
        {
            ReflectionTest test = new ReflectionTest();
            test.ClassMemberInfo();
            Console.ReadLine();
        }
    }

    public class ReflectionTest
    {
        /// <summary>
        /// Assembly类的使用
        /// </summary>
        public void AssemblyTest()
        {
            Console.WriteLine("\t\t------------Assembly类的使用-------------");
            Console.WriteLine();

            //获取当前所执行代码的程序集信息
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("获取程序集位置：" + myAssembly.CodeBase);
            Console.WriteLine("获取程序集的入口点：" + myAssembly.EntryPoint);
            Console.WriteLine("获取程序集的显示名称：" + myAssembly.FullName);
            Console.WriteLine("获取包含当前程序集清单的模块：" + myAssembly.ManifestModule);
            Console.WriteLine("获取URL，表示基本代码的转义符：" + myAssembly.EscapedCodeBase);
            Console.WriteLine("获取此程序集的证据：" + myAssembly.Evidence);
            Console.WriteLine("获取当前程序集的授予权：" + myAssembly.PermissionSet);
            Console.WriteLine("当前程序集是否从缓存中加载的：" + myAssembly.GlobalAssemblyCache.ToString());
            Console.WriteLine("加载程序集主机上下文：" + myAssembly.HostContext);
            Console.WriteLine("CRL版本：" + myAssembly.ImageRuntimeVersion);
            Console.WriteLine("当前程序集是否通过反射得到的：" + myAssembly.IsDynamic);
            Console.WriteLine("当前程序集是否是完全信任的方式加载的：" + myAssembly.IsFullyTrusted);
            Console.WriteLine("清单的已加载路径：" + myAssembly.Location);
            Console.WriteLine("获取当前程序集的授予集：" + myAssembly.PermissionSet);
            Console.WriteLine("当前程序集是否加载到只反射上下文中：" + myAssembly.ReflectionOnly);
            Console.WriteLine("CLR对当前程序集强制执行的安全规则集：" + myAssembly.SecurityRuleSet);
            Console.WriteLine();

            //获取当前程序集的名称及信息
            AssemblyName asmName = myAssembly.GetName();
            Console.WriteLine("获取或设置程序集的简单名称：" + asmName.Name);
            Console.WriteLine("获取或设置程序集的全名：" + asmName.FullName);
            Console.WriteLine("获取或设置程序集的URL位置：" + asmName.CodeBase);
            Console.WriteLine("获取或设置程序集的主版本号、次版本号、内部版本号和修订版本号：{0}", asmName.Version);


            Console.WriteLine("获取或设置程序集支持的区域性：" + asmName.CultureInfo);
            Console.WriteLine("获取URL，包括表示基本代码的转义符：" + asmName.EscapedCodeBase);
            Console.WriteLine("获取或设置程序集的特性：" + asmName.Flags);
            Console.WriteLine("获取或设置程序集的清单使用的哈希算法：" + asmName.HashAlgorithm);
            Console.WriteLine("获取或设置为程序集的创建强名称签名的加密公钥/私钥对：" + asmName.KeyPair);
            Console.WriteLine("获取或设置程序集的可执行文件的目标平台的处理器和每字位数：" + asmName.ProcessorArchitecture);
            Console.WriteLine("获取或设置程序集同其他程序的兼容性相关的信息：" + asmName.VersionCompatibility);
            Console.WriteLine();

            //获取当前程序集的版本相关信息
            System.Version asmVersion = asmName.Version;
            Console.WriteLine("获取当前程序集的主版本号：" + asmVersion.Major);
            Console.WriteLine("获取当前程序集的次版本号：" + asmVersion.Minor);
            Console.WriteLine("获取当前程序集的内部版本号：" + asmVersion.Build);
            Console.WriteLine("获取当前程序集的修订版本号：" + asmVersion.MajorRevision);
        }

        /// <summary>
        /// Type 类：检索类信息
        /// </summary>
        public void TypeTest()
        {
            Console.WriteLine("\t\t------------Type类的使用-------------");
            Console.WriteLine();

            Type myType = Type.GetType("反射事例.WeiMei");

            //检索信息
            Console.WriteLine("获取当前成员名称：" + myType.Name);
            Console.WriteLine("获取当前完全限定名（不包括程序集）：" + myType.FullName);
            Console.WriteLine("获取当前TYPE所在的命名空间：" + myType.Namespace);
            Console.WriteLine("获取当前TYPE关联的GUID：" + myType.GUID);
            Console.WriteLine("获取在其中定义在当前模块：" + myType.Module);
            Console.WriteLine("获取该成员的类对象：" + myType.ReflectedType);

            //检索类成员
            Console.WriteLine("获取方法相关信息：{0}", myType.GetMethod("MetName").ToString());
            Console.WriteLine("获取属性相关信息：{0}", myType.GetProperty("ProName").ToString());
            Console.WriteLine("获取字段相关信息：{0}", myType.GetField("mName", BindingFlags.NonPublic | BindingFlags.Instance).ToString());
            Console.WriteLine();
            Type myThis = this.GetType();
        }

        /// <summary>
        /// 访问类成员
        /// </summary>
        public void ClassMemberInfo()
        {
            //指定被访问的类
            //Type WMtype=Type.GetType("反射.WeiMei");//使用这个时无法获取属性值
            WeiMei myWeiMei = new WeiMei();
            Type WMtype = myWeiMei.GetType();
            Console.WriteLine("\t\t------------MemberInfo访问类的所有成员-------------");
            Console.WriteLine();
            //MemberInfo类：遍历被访问的类的所有成员
            MemberInfo[] members = WMtype.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MemberInfo member in members)
            {
                Console.WriteLine("成员名称：{0}-----成员类型：{1}", member.Name, member.MemberType);
            }
            Console.WriteLine();
            Console.WriteLine("\t\t------------MethodInfo访问类的方法-------------");
            Console.WriteLine();

            MethodInfo[] methods = WMtype.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                Console.WriteLine("方法名称：{0}-----成员类型：{1}", method.Name, method.ReturnType);
            }
            Console.WriteLine();

            Console.WriteLine("\t\t------------PropertyInfo访问类的属性-------------");
            Console.WriteLine();
            PropertyInfo[] propertys = WMtype.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in propertys)
            {
                Console.WriteLine("属性名称：{0}-----属性类型：{1}-----属性值：{2}", property.Name, property.PropertyType, property.GetValue(myWeiMei, null));
            }
            Console.WriteLine();
            Console.WriteLine("\t\t------------FieldInfo访问类的字段-------------");
            Console.WriteLine();
            FieldInfo[] fields = WMtype.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
                Console.WriteLine("字段名称：{0}-----字段类型：{1}", field.Name, field.FieldType);
            }

            Console.WriteLine("\t\t------------ConstructorInfo访问类的构造函数-------------");
            Console.WriteLine();

            ConstructorInfo[] constructors = WMtype.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (ConstructorInfo constructor in constructors)
            {
                Console.WriteLine("构造函数：{0}-----构造函数类型：{1}", constructor.Name, constructor.MemberType);
            }
            Console.WriteLine();
            Console.WriteLine("\t\t------------MemberInfo访问类的构造函数-------------");
            Console.WriteLine();
            EventInfo[] eventinfos = WMtype.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (EventInfo eventinfo in eventinfos)
            {
                Console.WriteLine("事件名称：{0}------成员类型：{1}", eventinfo.Name, eventinfo.MemberType);
            }
            Console.WriteLine();

            Console.WriteLine("\t\t------------MemberInfo访问类的构造函数-------------");
            Console.WriteLine();
            MethodInfo myMethod = WMtype.GetMethod("TestName");
            ParameterInfo[] parameters = myMethod.GetParameters();
            int count = 0;
            foreach (ParameterInfo param in parameters)
            {
                count++;
                Console.WriteLine("参数" + count + " 名称：{0}-----类型：{1}", param.Name, param.ParameterType);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 调用类成员
        /// </summary>
        public void OperMembers()
        {
            //指定被访问的类
            WeiMei weiMeiObj = new WeiMei();
            Type WMtype = weiMeiObj.GetType();
            //Type WMtype=Type.GetType("反射.WeiMei");
            //操作方法
            string[] parName = new string[] { "WeiMeiHua" };//方法参数的值
            MethodInfo method = WMtype.GetMethod("MetName");
            method.Invoke(weiMeiObj, parName);
            Console.WriteLine("操作方法改变值后为：{0}", weiMeiObj.ProName);

            //操作属性
            PropertyInfo proper = WMtype.GetProperty("ProName");
            proper.SetValue(weiMeiObj, "HuaWeiMei", null);
            //proper.GetValue(weiMeiObj,null);
            Console.WriteLine("操作属性改变值后为：{0}", weiMeiObj.ProName);

            //操作字段
            FieldInfo field = WMtype.GetField("mName", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(weiMeiObj, "WeiMei");
            //field.GetValue(weiMeiObj);
            Console.WriteLine("操作字段改变值后为：{0}", weiMeiObj.ProName);

            //操作事件
            EventInfo eventinfo = WMtype.GetEvent("WMShout");
            Console.WriteLine("声明该成员的类：" + eventinfo.DeclaringType);
            Console.WriteLine("事件名称：" + eventinfo.Name);
            Console.WriteLine("事件类型：" + eventinfo.MemberType);

            //操作构造函数
            ConstructorInfo constructor = WMtype.GetConstructor(System.Type.EmptyTypes);
        }
    }

    /// <summary>
    /// 被访问的类
    /// </summary>
    public class WeiMei
    {
        //字段
        private string mName;
        private string mSex;
        private int mAge;

        public WeiMei()
        {
            mName = "LiuWeiMei";
            mSex = "gril";
            mAge = 20;
        }

        public WeiMei(string name, string sex, int age)
        {
            mName = name;
            mSex = sex;
            mAge = age;
        }

        public string ProName
        {
            get { return mName; }
            set { mName = value; }
        }

        public string ProSex
        {
            get { return mSex; }
            set { mSex = value; }
        }

        public int ProAge
        {
            get { return mAge; }
            set { mAge = value; }
        }

        private string ProLoveYou
        {
            get { return "这是一个秘密，只有我知道"; }
        }

        public string MetName(string herName)
        {
            this.mName = herName;
            return mName;
        }

        public string MetSex(string herSex)
        {
            this.mSex = herSex;
            return mSex;
        }

        public int MetAge(int herAge)
        {
            this.mAge = herAge;
            return mAge;
        }

        public string TestName(string herName, int like, float love)
        {
            this.mName = herName;
            return mName;
        }

        public delegate string WMName(string wmName);

        public event WMName WMShout;
        private event WMName WMLove;
    }

}
