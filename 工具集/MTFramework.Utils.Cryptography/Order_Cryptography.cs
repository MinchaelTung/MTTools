
namespace MTFramework.Utils.Cryptography
{
    /// <summary>
    /// 循序加解密
    /// </summary>
    public sealed class Order_Cryptography
    {
        /// <summary>
        /// 顺序加密
        /// <para>非极度保密性信息，需要时可以进行加解密</para>
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>返回加密后字符串</returns>
        public static string OrderEncrypt(string str)
        {
            byte[] by = new byte[str.Length];
            for (int i = 0; i <= str.Length - 1; i++)
            {
                by[i] = (byte)((byte)str[i] + 1);
            }
            str = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                str += ((char)by[i]).ToString();
            }
            return str;
        }

        /// <summary>
        /// 顺序解密
        /// <para>非极度保密性信息，需要时可以进行加解密</para>
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public static string OrderDecrypt(string str)
        {
            byte[] by = new byte[str.Length];
            for (int i = 0; i <= str.Length - 1; i++)
            {
                by[i] = (byte)((byte)str[i] - 1);
            }
            str = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                str += ((char)by[i]).ToString();
            }
            return str;
        }
    }
}
