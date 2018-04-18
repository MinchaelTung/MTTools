using System;
using System.Text;

namespace MTFramework.Utils.ConvertUitls
{
    /// <summary>
    /// string 和 byte[] 互转换工具
    /// </summary>
    public sealed class StringConvert
    {
        /// <summary>
        /// 字符串转换为byte数组
        /// </summary>
        /// <param name="inputString">传入字符串参数</param>
        /// <returns>byte[]</returns>
        public static byte[] StringConvertToBytes(string inputString)
        {
            return System.Convert.FromBase64String(inputString);
        }

        /// <summary>
        /// byte数组转换为字符串
        /// </summary>
        /// <param name="inputBytes">传入byte数组参数</param>
        /// <returns>string</returns>
        public static string BytesConvertToString(byte[] inputBytes)
        {
            return System.Convert.ToBase64String(inputBytes);
        }

        /// <summary>
        /// 16进制字符串转16进制字节
        /// </summary>
        /// <param name="hexString">16进制字符数据</param>
        /// <returns>字节数据</returns>
        public static byte[] HexStrToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        /// 16进制字节转16进制字符串
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <returns>16进制字符串</returns>
        public static string ByteToHexStr(byte[] bytes)
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
        /// 数字或16进制字符串转BCD编码
        /// </summary>
        /// <param name="strTemp">数据</param>
        /// <returns>BCD编码后的数据</returns>
        public static Byte[] ConvertFrom(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//数字的二进制码最后1位是1则为奇数
                {
                    strTemp = "0" + strTemp;//数位为奇数时前面补0
                }
                Byte[] aryTemp = new Byte[strTemp.Length / 2];
                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    char high = (char)Convert.ToByte(strTemp.Substring(i * 2, 1), 16);
                    char low = (char)Convert.ToByte(strTemp.Substring(i * 2 + 1, 1), 16);
                    aryTemp[i] = (Byte)((high << 4) | low);
                }
                return aryTemp;//高位在前
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// BCD码转换16进制(压缩BCD)
        /// </summary>
        /// <param name="strTemp">数据</param>
        /// <param name="len">数据长度</param>
        /// <returns>BCD压缩后的数据</returns>
        public static Byte[] ConvertFrom(string strTemp, int len)
        {
            try
            {
                Byte[] Temp = ConvertFrom(strTemp.Trim());
                Byte[] return_Byte = new Byte[len];
                if (len != 0)
                {
                    if (Temp.Length < len)
                    {
                        for (int i = 0; i < len - Temp.Length; i++)
                        {
                            return_Byte[i] = 0x00;
                        }
                    }
                    Array.Copy(Temp, 0, return_Byte, len - Temp.Length, Temp.Length);
                    return return_Byte;
                }
                else
                {
                    return Temp;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 16进制转换BCD（解压BCD）
        /// </summary>
        /// <param name="aData">数据</param>
        /// <returns>BCD解压后数据</returns>
        public static string ConvertTo(byte[] aData)
        {
            try
            {
                StringBuilder sb = new StringBuilder(aData.Length * 2);
                foreach (Byte b in aData)
                {
                    sb.Append(Convert.ToString(b >> 4, 16));
                    sb.Append(Convert.ToString(b & 0x0f, 16));
                }
                return sb.ToString();
            }
            catch
            {
                return null;
            }
        }

    }
}
