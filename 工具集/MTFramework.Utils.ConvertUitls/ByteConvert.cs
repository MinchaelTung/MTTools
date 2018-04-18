#define ABC

using System;
using System.Linq;
using System.Text;

namespace MTFramework.Utils.ConvertUitls
{

    public class ByteConvert
    {

        /// <summary>
        /// int转两位字节数组
        /// </summary>
        /// <param name="n">数据</param>
        /// <returns>字节数据</returns>
        public static byte[] ShortToByte(short n)
        {
#if(ABC)
            return BitConverter.GetBytes(n).Reverse().ToArray();
#else
            byte[] b = new byte[2];
            b[0] = (byte)(n >> 8);
            b[1] = (byte)n;
            return b;
#endif
        }
        /// <summary>
        /// 两位字节转int
        /// </summary>
        /// <param name="b">数据</param>
        /// <returns>对应值</returns>
        public static short ByteToShort(byte[] b)
        {
#if(ABC)
            return BitConverter.ToInt16(b.Reverse().ToArray(), 0);
#else
            return(short)(b[1] & 0xff | (b[0] & 0xff) << 8);
#endif
        }
        /// <summary>
        /// int转四字节数组
        /// </summary>
        /// <param name="n">数据</param>
        /// <returns>字节数据</returns>
        public static byte[] IntToByte(int n)
        {
#if(ABC)
            return BitConverter.GetBytes(n).Reverse().ToArray();
#else
            byte[] b = new byte[4];
            b[0] = (byte)(n >> 24);
            b[1] = (byte)(n >> 16);
            b[2] = (byte)(n >> 8);
            b[3] = (byte)n;
            return b;
#endif
        }

        /// <summary>
        /// 四位字节转int
        /// </summary>
        /// <param name="b">数据</param>
        /// <returns>对应值</returns>
        public static int ByteToInt(byte[] b)
        {
#if(ABC)
            return BitConverter.ToInt32(b.Reverse().ToArray(), 0);
#else
            return b[3] & 0xff | (b[2] & 0xff) << 8 | (b[1] & 0xff) << 16 | (b[0] & 0xff) << 24;
#endif
        }

        /// <summary>
        /// 16进制字符串转字节数组 "33d20046" 转换为 0x33 0xD2 0x00 0x46
        /// </summary>
        /// <param name="hexString">6进制字符数据</param>
        /// <returns>字节数据</returns>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (string.IsNullOrEmpty(hexString) == true)
            {
                return null;
            }
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
#if(ABC)
                returnBytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
#else
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
#endif
            }
            return returnBytes;
        }


        /// <summary>
        /// 字节数组转16进制字符串 0x33 0xD2 0x00 0x46 转换为 "33d20046" 
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <returns>16进制字符串</returns>
        public static string ByteToHexString(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// BCD码转为10进制串(阿拉伯数据)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BcdToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(Convert.ToString(((byte)(bytes[i] & 0xf0) >> 4), 16));
                sb.Append(Convert.ToString(((byte)(bytes[i] & 0x0f)), 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 10进制串转为BCD码
        /// </summary>
        /// <param name="asc"></param>
        /// <returns></returns>
        public static byte[] StringToBcd(string asc)
        {
            byte[] abt = Encoding.UTF8.GetBytes(asc);

            return ByteArrayToBcd(abt);
        }

        /// <summary>
        /// 10进制字节数组转为BCD码
        /// </summary>
        /// <param name="abt"></param>
        /// <returns></returns>
        public static byte[] ByteArrayToBcd(byte[] abt)
        {
            int len = abt.Length;
            int mod = len % 2;
            if (mod != 0)
            {
                byte[] temp = new byte[len + 1];
                temp[0] = Encoding.UTF8.GetBytes("0")[0];
                Array.Copy(abt, 0, temp, 1, abt.Length);
                abt = temp;
                len += 1;
            }
            if (len >= 2)
            {
                len = len / 2;
            }

            byte[] bbt = new byte[len];

            int j, k;

            for (int p = 0; p < abt.Length / 2; p++)
            {
                if ((abt[2 * p] >= '0') && (abt[2 * p] <= '9'))
                {
                    j = abt[2 * p] - '0';
                }
                else if ((abt[2 * p] >= 'a') && (abt[2 * p] <= 'z'))
                {
                    j = abt[2 * p] - 'a' + 0x0a;
                }
                else
                {
                    j = abt[2 * p] - 'A' + 0x0a;
                }

                if ((abt[2 * p + 1] >= '0') && (abt[2 * p + 1] <= '9'))
                {
                    k = abt[2 * p + 1] - '0';
                }
                else if ((abt[2 * p + 1] >= 'a') && (abt[2 * p + 1] <= 'z'))
                {
                    k = abt[2 * p + 1] - 'a' + 0x0a;
                }
                else
                {
                    k = abt[2 * p + 1] - 'A' + 0x0a;
                }
                int a = (j << 4) + k;
                byte b = (byte)a;
                bbt[p] = b;
            }
            return bbt;
        }
        /// <summary>
        /// 截取字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] Subarray(byte[] source, int offset, int length)
        {
            if (source.Length < offset + length)
            {
                length = source.Length - offset;
            }
            byte[] buf = new byte[length];
            Array.Copy(source, offset, buf, 0, length);
            return buf;
        }
    }
}
