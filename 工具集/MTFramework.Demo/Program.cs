using MTFramework.Iso8583PackageUtil;
using MTFramework.Reflections.DynamicImplementInterface;
using MTFramework.Utils.ConvertUitls;
using MTFramework.Utils.Cryptography;
using MTFramework.Utils.SerializationHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTFramework.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始执行");
            //Iso8583协议 组解包事例
            //Iso8583PackageUtil_Demo();
            //Byte转换事例
            //ByteConvert_Demo();
            //加解密事例
            //Cryptography_Dome();
            //对象序列化事例
            //SerializationHelper_Demo();
            //动态加载接口并实现接口成员
            //DynamicRuntimeClass_Demo();
            Console.WriteLine("Demo执行完毕");


            Console.ReadLine();
        }

        #region 加解密事例

        /// <summary>
        /// 加解密事例
        /// </summary>
        static void Cryptography_Dome()
        {
            Console.WriteLine("加解密事例 Cryptography_Dome");
            //循序加解密
            string orderEncrypt = Order_Cryptography.OrderEncrypt("ABCDEF");
            Console.WriteLine("循序加解密: Demo 字符串: ABCDEF");
            Console.WriteLine(string.Format("加密结果:{0}", orderEncrypt));
            Console.WriteLine(string.Format("解密结果:{0}", Order_Cryptography.OrderDecrypt(orderEncrypt)));
            Console.WriteLine("下一个");
            Console.ReadLine();

            //MD5 加密
            Console.WriteLine("MD5加密： Demo 字符串: ABCDEF");
            string md5str = MD5_Cryptography.MD5Encrypt("ABCDEF");
            Console.WriteLine(string.Format("标准MD5加密结果：{0} 长度：{1}", md5str, md5str.Length));
            md5str = MD5_Cryptography.MD5Encrypt("ABCDEF", MD5_Type.MD5_32BIT);
            Console.WriteLine(string.Format("标准MD5 32BIT 加密结果：{0} 长度：{1}", md5str, md5str.Length));
            md5str = MD5_Cryptography.MD5Encrypt("ABCDEF", MD5_Type.MD5_64BIT);
            Console.WriteLine(string.Format("标准MD5 64BIT 加密结果：{0} 长度：{1}", md5str, md5str.Length));

            Console.WriteLine("下一个");
            Console.ReadLine();

            //DES 加解密
            DES_Cryptographycs des = new DES_Cryptographycs();
            //设置Key
            des.Key = ByteConvert.HexStringToBytes("0123456789ABCDEF");
            //设置IV
            des.IV = ByteConvert.HexStringToBytes("0123456789ABCDEF");
            //设置填充模式
            des.PaddingMode = System.Security.Cryptography.PaddingMode.None;
            //设置加密类型
            des.CipherMode = System.Security.Cryptography.CipherMode.CBC;
            Console.WriteLine("DES加解密：Demo 字符串: 1111111111111111");
            //加密
            byte[] des_E = des.DES_Encrypt(ByteConvert.HexStringToBytes("1111111111111111"));
            Console.WriteLine(string.Format("加密结果：{0}", ByteConvert.ByteToHexString(des_E)));
            //解密结果
            byte[] des_D = des.DES_Decrypt(des_E);
            Console.WriteLine(string.Format("解密结果: {0}", ByteConvert.ByteToHexString(des_D)));

            Console.WriteLine("下一个");
            Console.ReadLine();
            //MAC加密
            Console.WriteLine(string.Format("MAC加密 数据：1111111111111111 加密结果: {0}", ByteConvert.ByteToHexString(des.GetMAC(ByteConvert.HexStringToBytes("1111111111111111")))));
            Console.WriteLine("下一个");
            Console.ReadLine();

            //3DES 加解密

            Console.WriteLine("3DES 加解密 Demo 数据 ：1111111111111111");
            byte[] tdes_E = des.TripleDES_Encrypt(ByteConvert.HexStringToBytes("1111111111111111"));
            Console.WriteLine("3DES加密结果: {0}", ByteConvert.ByteToHexString(tdes_E));
            byte[] tdes_D = des.TripleDES_Decrypt(tdes_E);
            Console.WriteLine("3DES解密结果: {0}", ByteConvert.ByteToHexString(tdes_D));

        }

        #endregion

        #region --- 对象序列化事例 Begin ---

        /// <summary>
        /// 对象序列化事例
        /// </summary>
        static void SerializationHelper_Demo()
        {
            Console.WriteLine("对象序列化事例 SerializationHelper_Demo");
            UserInfo user = new UserInfo() { Name = "测试", Age = 18 };
            //把简单对象序列化为XML格式文件
            XmlSerializationHelper.Save(user, "user.xml");
            //把序列化后的文件还原为对象
            user = XmlSerializationHelper.LoadXmlFile<UserInfo>("user.xml");

            //把简单的对象序列化为字符串
            string stxmlstring = ObjectSerializationHelper.SerializeToXmlString(user);
            //把序列化后的字符串还原为对象
            user = ObjectSerializationHelper.DeSerializeForXmlString<UserInfo>(stxmlstring);

            //把对象序列化为字节数组
            byte[] buf = ObjectSerializationHelper.SerializeToBytes(user);
            //把数组序反列化为对象
            user = ObjectSerializationHelper.DeSerializeForBytes<UserInfo>(buf);

            //浅客隆 只能快捷可能简单结构的对象
            UserInfo simpleClone = ObjectSerializationHelper.SimpleClone(user);
            //深克隆 可以克隆比较复杂结构的对象
            UserInfo advancedClone = ObjectSerializationHelper.AdvancedClone(user);
        }
        #endregion --- 对象序列化事例 End ---

        #region --- Byte转换事例 Begin ---

        /// <summary>
        /// Byte转换事例
        /// </summary>
        static void ByteConvert_Demo()
        {
            Console.WriteLine("Byte转换事例 ByteConvert_Demo");
            StringBuilder sb = new StringBuilder();
            //数字和两字节互转
            byte[] shortbytes = ByteConvert.ShortToByte(18);
            sb.Append("ByteConvert.ShortToByte(18)结果: ");
            foreach (var item in shortbytes)
            {
                sb.AppendFormat("0x{0:X2}({0}) ", item);
            }
            Console.WriteLine(sb.ToString());
            short resultShort = ByteConvert.ByteToShort(shortbytes);
            Console.WriteLine(string.Format("ByteConvert.ByteToShort(shortbytes)结果：short {0}", resultShort));
            Console.WriteLine("下一个");
            Console.ReadLine();
            //数字和4字节互转
            byte[] intbytes = ByteConvert.IntToByte(56);
            sb = new StringBuilder();
            sb.Append("ByteConvert.IntToByte(56)结果: ");
            foreach (var item in intbytes)
            {
                sb.AppendFormat("0x{0:X2}({0})", item);
            }
            Console.WriteLine(sb.ToString());
            int resultInt = ByteConvert.ByteToInt(intbytes);
            Console.WriteLine(string.Format("ByteConvert.ByteToInt(intbytes)：int {0}", resultInt));
            Console.WriteLine("下一个");
            Console.ReadLine();
            //十六进制哈希字符串和字节数组互转
            byte[] hexStringBytes = ByteConvert.HexStringToBytes("33d20046");
            sb = new StringBuilder();
            sb.Append("ByteConvert.HexStringToBytes(\"33d20046\")结果: ");
            foreach (var item in hexStringBytes)
            {
                sb.AppendFormat("0x{0:X2}({0})", item);
            }
            Console.WriteLine(sb.ToString());
            string byteToHexString = ByteConvert.ByteToHexString(hexStringBytes);
            Console.WriteLine(string.Format("ByteConvert.ByteToHexString(hexStringBytes)结果: {0} ", byteToHexString));
            Console.WriteLine("下一个");
            Console.ReadLine();
            //10进制串转为BCD码互转方法
            byte[] bcdbytes = ByteConvert.StringToBcd("12345678");
            sb = new StringBuilder();
            sb.Append("ByteConvert.StringToBcd(\"12345678\")结果：");
            foreach (var item in bcdbytes)
            {
                sb.AppendFormat("0x{0:X2}({0}) ", item);
            }
            Console.WriteLine(sb.ToString());
            string ace = ByteConvert.BcdToString(bcdbytes);
            Console.WriteLine(string.Format("ByteConvert.BcdToString(bcdbytes)结果:{0}", ace));
            //十进制字节数组转为BCD码
            byte[] abt = Encoding.UTF8.GetBytes(ace);
            sb = new StringBuilder();
            sb.Append("数组内容:");
            foreach (var item in abt)
            {
                sb.AppendFormat("{0} )", item);
            }
            Console.WriteLine(sb.ToString());
            byte[] bcdbytes2 = ByteConvert.ByteArrayToBcd(abt);
            sb = new StringBuilder();
            sb.Append("ByteConvert.ByteArrayToBcd(abt)结果: ");
            foreach (var item in bcdbytes2)
            {
                sb.AppendFormat("0x{0:X2}({0}) ", item);
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine("下一个，看代码");
            Console.ReadLine();

            byte[] buf = new byte[50];
            //截取数组
            byte[] subBytes = ByteConvert.Subarray(buf, 5, 10);
        }

        #endregion --- Byte转换事例 End ---

        #region --- Iso8583协议 组解包事例 Begin ---

        /// <summary>
        /// Iso8583协议 组解包事例
        /// </summary>
        static void Iso8583PackageUtil_Demo()
        {
            Console.WriteLine("Iso8583协议 组解包事例 Iso8583PackageUtil_Demo");

            //定义数据池类型
            SortedList<int, MessageFieldDefinition> messageFieldDefinitions = new SortedList<int, MessageFieldDefinition>();

            messageFieldDefinitions.Add(2, new MessageFieldDefinition(2, MessageFieldType.LLVAR, 16, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(3, new MessageFieldDefinition(3, MessageFieldType.LLVAR, 16, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(4, new MessageFieldDefinition(4, MessageFieldType.NUMERIC, 1, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(5, new MessageFieldDefinition(5, MessageFieldType.NUMERIC, 1, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(6, new MessageFieldDefinition(6, MessageFieldType.NUMERIC, 1, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(7, new MessageFieldDefinition(7, MessageFieldType.LLLVAR, 100, TransportMode.BINARY));
            messageFieldDefinitions.Add(8, new MessageFieldDefinition(8, MessageFieldType.ALPHA, 36, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(9, new MessageFieldDefinition(9, MessageFieldType.NUMERIC, 1, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(10, new MessageFieldDefinition(10, MessageFieldType.NUMERIC, 4, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(11, new MessageFieldDefinition(11, MessageFieldType.LLLVAR, 200, TransportMode.BINARY));
            messageFieldDefinitions.Add(12, new MessageFieldDefinition(12, MessageFieldType.NUMERIC, 6, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(13, new MessageFieldDefinition(13, MessageFieldType.NUMERIC, 6, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(14, new MessageFieldDefinition(14, MessageFieldType.NUMERIC, 8, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(15, new MessageFieldDefinition(15, MessageFieldType.ALPHA, 36, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(16, new MessageFieldDefinition(16, MessageFieldType.NUMERIC, 5, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(17, new MessageFieldDefinition(17, MessageFieldType.NUMERIC, 4, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(18, new MessageFieldDefinition(18, MessageFieldType.LLVAR, 16, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(19, new MessageFieldDefinition(19, MessageFieldType.LLVAR, 16, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(20, new MessageFieldDefinition(20, MessageFieldType.AMOUNT, 6, TransportMode.RIGHT_ASCII));
            messageFieldDefinitions.Add(21, new MessageFieldDefinition(21, MessageFieldType.AMOUNT, 6, TransportMode.RIGHT_ASCII));
            messageFieldDefinitions.Add(22, new MessageFieldDefinition(22, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(23, new MessageFieldDefinition(23, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(24, new MessageFieldDefinition(24, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(25, new MessageFieldDefinition(25, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(26, new MessageFieldDefinition(26, MessageFieldType.ALPHA, 8, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(27, new MessageFieldDefinition(27, MessageFieldType.NUMERIC, 1, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(28, new MessageFieldDefinition(28, MessageFieldType.NUMERIC, 1, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(29, new MessageFieldDefinition(29, MessageFieldType.BINARY, 16, TransportMode.BINARY));
            messageFieldDefinitions.Add(30, new MessageFieldDefinition(30, MessageFieldType.NUMERIC, 12, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(31, new MessageFieldDefinition(31, MessageFieldType.BINARY, 16, TransportMode.BINARY));
            messageFieldDefinitions.Add(32, new MessageFieldDefinition(32, MessageFieldType.BINARY, 12, TransportMode.BINARY));
            messageFieldDefinitions.Add(33, new MessageFieldDefinition(33, MessageFieldType.BINARY, 32, TransportMode.BINARY));
            messageFieldDefinitions.Add(34, new MessageFieldDefinition(34, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(35, new MessageFieldDefinition(35, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(36, new MessageFieldDefinition(36, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(37, new MessageFieldDefinition(37, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(38, new MessageFieldDefinition(38, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(39, new MessageFieldDefinition(39, MessageFieldType.ALPHA, 2, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(40, new MessageFieldDefinition(40, MessageFieldType.LLVAR, 10, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(41, new MessageFieldDefinition(41, MessageFieldType.LLVAR, 10, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(42, new MessageFieldDefinition(42, MessageFieldType.LLLVAR, 255, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(43, new MessageFieldDefinition(43, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(44, new MessageFieldDefinition(44, MessageFieldType.LLLVAR, 25, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(45, new MessageFieldDefinition(45, MessageFieldType.LLLVAR, 800, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(46, new MessageFieldDefinition(46, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(47, new MessageFieldDefinition(47, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(48, new MessageFieldDefinition(48, MessageFieldType.LLLVAR, 800, TransportMode.LEFT_ASCII));
            messageFieldDefinitions.Add(49, new MessageFieldDefinition(49, MessageFieldType.LLLVAR, 800, TransportMode.BINARY));
            messageFieldDefinitions.Add(50, new MessageFieldDefinition(50, MessageFieldType.LLLVAR, 800, TransportMode.BINARY));
            messageFieldDefinitions.Add(51, new MessageFieldDefinition(51, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(52, new MessageFieldDefinition(52, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(53, new MessageFieldDefinition(53, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(54, new MessageFieldDefinition(54, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(55, new MessageFieldDefinition(55, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(56, new MessageFieldDefinition(56, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(57, new MessageFieldDefinition(57, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(58, new MessageFieldDefinition(58, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(59, new MessageFieldDefinition(59, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(60, new MessageFieldDefinition(60, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(61, new MessageFieldDefinition(61, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(62, new MessageFieldDefinition(62, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(63, new MessageFieldDefinition(63, MessageFieldType.BINARY, 0, TransportMode.BINARY));
            messageFieldDefinitions.Add(64, new MessageFieldDefinition(64, MessageFieldType.BINARY, 64, TransportMode.BINARY));

            //设置数据池类型
            MessagePackageProtocol.GetInstance().MessageFieldDefinitions = messageFieldDefinitions;

            //组包
            MessageHelper package = new MessageHelper();
            //设置加密的Key 和 VI
            package.SetKeyAndIV(ByteConvert.HexStringToBytes("0123456789ABCDEF"), ByteConvert.HexStringToBytes("0123456789ABCDEF"));
            //设置头
            package.Header = "4401000000602200000000";
            //设置信息类型
            package.TypeID = "0110";
            //设置域数据
            package.SetFieldValue(39, "00");
            //设置MAC校验码
            package.SetMacField(64);
            //获取发送数据
            byte[] buf = package.GetContent();

            //解包
            MessageHelper result = new MessageHelper();
            //设置加密的Key 和 VI
            result.SetKeyAndIV(ByteConvert.HexStringToBytes("0123456789ABCDEF"), ByteConvert.HexStringToBytes("0123456789ABCDEF"));
            //设置头
            result.Header = "4401000000602200000000";
            //解析包
            result.Parse(buf);
            //获取解析数据
            string str39 = result[39].Value.ToString();
            //获取解析数据
            string str64 = ByteConvert.ByteToHexString((byte[])result[64].Value);

        }

        #endregion --- Iso8583协议 组解包事例 End ---

        #region --- 动态加载接口并实现接口成员 Begin ---

        /// <summary>
        /// 动态加载接口并实现接口成员
        /// </summary>
        static void DynamicRuntimeClass_Demo()
        {
            Console.WriteLine("动态加载接口并实现接口成员 DynamicRuntimeClass_Demo");
            DynamicRuntimeClass<ITry> cls = new DynamicRuntimeClass<ITry>();
            cls.ImplementProperty<string>("Prop1",
                (@this) =>
                {
                    //属性 get
                    return cls.Fields["prop1"].ToString();
                },
                (@this, s) =>
                {
                    //属性 set
                    cls.Fields["prop1"] = s;
                });
            cls.ImplementMethod("Fun2", new Func<ITry, string>((@this) =>
            {
                //Fun2 的方法体
                @this.Prop1 = "3212";
                return @this.Prop1;
            }));
            cls.ImplementMethod("Fun1", new Action<ITry, int>((@this, num) =>
            {
                //Fun1的方法体
                Console.WriteLine(num);

            }));
            // cls.OnEventHandler(@this, "Click", null, EventArgs.Empty);
            ITry obj = (ITry)cls.CreateInstance();

            //obj.OnClick += (sender, e) =>
            //{
            //    MessageBox.Show("点击");
            //};
            obj.Prop1 = "123";
            string ss = obj.Prop1;
            ss = obj.Fun2();
            Console.WriteLine(ss);
            obj.Fun1(50);
        }



        #endregion --- 动态加载接口并实现接口成员 End ---
    }

    #region --- 动态加载接口并实现接口成员-实体 Begin ---

    [Serializable]
    class UserInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public interface ITry
    {
        string Prop1
        {
            get;
            set;
        }

        event EventHandler OnClick;

        void Fun1(int num);
        string Fun2();
    }
    #endregion --- 动态加载接口并实现接口成员-实体 End ---

}
