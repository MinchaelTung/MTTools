using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace MTFramework.Web.Controls
{
    public class SerialNumber : Control
    {
        //请求标识
        private const string _ImageTag = "ImageTag";
        //噪点线数量
        private int _Bother = 10;

        //字符串加解密规则
        private static byte[] Key;
        private static byte[] IV;

        //字体定义
        //定义一个字符串数组储存汉字编码的组成元素 
        private static string[] rBase = new string[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
        //定义字体 
        private static string[] fontStyles = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
        //定义字体颜色        
        private static Color[] fontColors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        //字体旋转角度
        private const int randAngle = 30;

        //随机数对象
        private static Random random = new Random();

        // 保存当前产生的验证码        
        private string mSN;

        static SerialNumber()
        {
            Key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0x10, 0x11, 0x12, 0x13, 20, 0x15, 0x16 };
            IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0x10, 0x11, 0x12, 0x13, 20, 0x15, 0x16 };
        }

        public SerialNumber()
        {
            this.mSN = "";
        }

        /// <summary>
        /// 噪点线数量
        /// </summary>
        public int Bother
        {
            get { return _Bother; }
            set { _Bother = value; }
        }
        /// <summary>
        /// 验证码个数,默认4个
        /// </summary>
        public int SNCount
        {
            get
            {
                int num = 4;
                Int32.TryParse(ConfigurationManager.AppSettings["SNCount"], out num);
                return num < 1 ? 4 : num;
            }
        }
        /// <summary>
        /// 验证码字体类型
        /// </summary>
        public SerialNumberType SNType
        {
            get
            {
                switch (ConfigurationManager.AppSettings["SerialNumberType"].ToUpper())
                {
                    case "NUMBER":
                        return SerialNumberType.NUMBER;
                    case "LETTER":
                        return SerialNumberType.LETTER;
                    case "BASESTR":
                        return SerialNumberType.BASESTR;
                    default:
                        return SerialNumberType.NUMBERANDLETTER;
                }
            }
        }


        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="sn">用户输入的验证</param>
        /// <returns></returns>
        public bool CheckSN(string sn)
        {
            return sn.ToUpper() == this.mSN.ToUpper();
        }
        /// <summary>
        /// 创建新验证码方法
        /// </summary>
        public void Create()
        {
            this.OnCreate();
        }

        #region --- 产生随机字符串 Begin ---

        /// <summary>
        /// 随机生成验证码
        /// </summary>
        private void OnCreate()
        {
            this.mSN = "";
            for (int i = 0; i < this.SNCount; i++)
            {
                //更换随机数发生器的种子避免产生重复值 
                random = new Random(unchecked((int)DateTime.Now.Ticks) + i);
                if (SNType == SerialNumberType.NUMBER)
                    this.mSN += GetNumber;
                else if (SNType == SerialNumberType.LETTER)
                    this.mSN += GetLetter;
                else if (SNType == SerialNumberType.BASESTR)
                    this.mSN += GetBaseStr;
                else
                    this.mSN += ((random.Next() % 2) == 0) ? GetNumber : GetLetter;
            }
        }

        /// <summary>
        /// 随机获取数字
        /// </summary>
        private string GetNumber
        {
            get
            {
                return random.Next(0, 10).ToString();
            }
        }
        /// <summary>
        /// 随机获取字符
        /// </summary>
        private string GetLetter
        {
            get
            {
                return ((random.Next() % 2) == 0)
                    ? ((char)random.Next('a', 'z' + 1)).ToString()
                    : ((char)random.Next('A', 'Z' + 1)).ToString();
            }
        }

        private string GetBaseStr
        {
            get
            {


                //区位码第1位
                int r1 = random.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位

                //更换随机数发生器的种子避免产生重复值 
                random = new Random(r1 * unchecked((int)DateTime.Now.Ticks));
                int r2;
                if (r1 == 13)
                    r2 = random.Next(0, 7);
                else
                    r2 = random.Next(0, 16);
                string str_r2 = rBase[r2].Trim();

                //区位码第3位

                //更换随机数发生器的种子避免产生重复值 
                random = new Random(r2 * unchecked((int)DateTime.Now.Ticks));
                int r3 = random.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位

                //更换随机数发生器的种子避免产生重复值 
                random = new Random(r2 * unchecked((int)DateTime.Now.Ticks));
                int r4;
                if (r3 == 10)
                    r4 = random.Next(1, 16);
                else if (r3 == 15)
                    r4 = random.Next(0, 15);
                else
                    r4 = random.Next(0, 16);
                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码,
                //将两个字节变量存储在字节数组中
                byte[] str_r = {
                    Convert.ToByte(str_r1 + str_r2, 16), 
                    Convert.ToByte(str_r3 + str_r4, 16) 
                };

                //获取GB2312编码页（表） 
                Encoding gb = Encoding.GetEncoding("gb2312");
                return gb.GetString(str_r);
            }
        }

        #endregion --- 产生随机字符串 End ---



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string str = Page.Request.QueryString[_ImageTag];
            if (str != null)
            {
                byte[] buffer = null;
                try
                {
                    //解密字符串
                    Crypto cryto = new Crypto();
                    cryto.CryptText = str;
                    cryto.CryptIV = IV;
                    cryto.CryptKey = Key;
                    mSN = cryto.Decrypt();



                    //绘制图片
                    int mapWidth = (int)(mSN.Length * 28);//背景长度
                    using (Image img = new Bitmap(mapWidth, 28))
                    {
                        Graphics grfx = Graphics.FromImage(img);
                        grfx.Clear(Color.AliceBlue);//清除画面，填充背景
                        for (int i = 0; i < Bother; i++)
                        {
                            int x1 = random.Next(img.Width);
                            random = new Random(unchecked((int)DateTime.Now.Ticks) * x1 + i);
                            int x2 = random.Next(img.Width);
                            random = new Random(unchecked((int)DateTime.Now.Ticks) * x2 + i);
                            int y1 = random.Next(img.Height);
                            random = new Random(unchecked((int)DateTime.Now.Ticks) * y1 + i);
                            int y2 = random.Next(img.Height);
                            grfx.DrawLine(new Pen(Color.FromArgb(random.Next(50, 255), random.Next(50, 255), random.Next(50, 255))), x1, y1, x2, y2);
                        }

                        //文字距中
                        StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        for (int i = 0; i < mSN.Length; i++)
                        {
                            random = new Random(unchecked((int)DateTime.Now.Ticks) * 5 + i);
                            Font font = new Font(fontStyles[random.Next(fontStyles.Length)], 13, FontStyle.Bold);
                            Brush fontBrush = new SolidBrush(fontColors[random.Next(fontColors.Length)]);
                            Point dot = new Point(22, 16);

                            //转动的度数
                            float angle = random.Next(-randAngle, randAngle);

                            //移动光标到指定位置
                            grfx.TranslateTransform(dot.X, dot.Y);
                            grfx.RotateTransform(angle);
                            grfx.DrawString(mSN.Substring(i, 1), font, fontBrush, random.Next(-3, 3), random.Next(-3, 3), format);
                            grfx.RotateTransform(-angle);//转回去
                            grfx.TranslateTransform(2, -dot.Y);
                        }
                        grfx.Flush();
                        grfx.Dispose();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, ImageFormat.Jpeg);
                        img.Dispose();
                        //存放图片的流

                        buffer = new byte[ms.Length];
                        ms.Position = 0L;
                        ms.Read(buffer, 0, buffer.Length);

                    }
                }
                catch (Exception ex)
                {
                    //错误记录
                    string filePath = HttpContext.Current.Server.MapPath("~/App_Data/SerialNumberExection.txt");
                    string error = String.Format("{0}\r\n{1}\r\n\r\n************************************\r\n", ex.Message, ex.StackTrace);
                    File.AppendAllText(filePath, error, Encoding.Default);
                }
                this.Page.Response.Clear();
                this.Page.Response.BinaryWrite(buffer);
                this.Page.Response.Flush();
                this.Page.Response.End();
            }

        }

        protected override void Render(HtmlTextWriter output)
        {
            if (base.Site != null && base.Site.DesignMode)
            {
                output.Write("<br>NO SerialNumber<br>");
            }
            else if (this.mSN != null)
            {
                Crypto crypto = new Crypto();
                crypto.CryptText = mSN;
                crypto.CryptKey = Key;
                crypto.CryptIV = IV;
                string s = crypto.Encrypt();
                output.Write("<img border=\"1\" src=\"{0}?{1}={2}\">", this.Page.Request.Path, _ImageTag, HttpContext.Current.Server.UrlEncode(s));
            }
            else
            {
                output.Write("<br>NO SerialNumber<br>");
            }
        }

        protected override void LoadViewState(object savedState)
        {
            object[] objArray = (object[])savedState;
            base.LoadViewState(objArray[0]);
            Crypto crypto = new Crypto();
            crypto.CryptText = (string)objArray[1];
            crypto.CryptIV = IV;
            crypto.CryptKey = Key;
            this.mSN = crypto.Decrypt();
        }

        protected override object SaveViewState()
        {
            Crypto crypto = new Crypto();
            crypto.CryptText = this.mSN;
            crypto.CryptIV = IV;
            crypto.CryptKey = Key;
            return new object[] { base.SaveViewState(), crypto.Encrypt() };
        }

    }

    /// <summary>
    /// NUMBER 数字
    /// LETTER 字母
    /// NUMBERANDLETTER 数字+字符
    /// BASESTR 中文字
    /// </summary>
    public enum SerialNumberType { NUMBER, LETTER, NUMBERANDLETTER, BASESTR }

}
