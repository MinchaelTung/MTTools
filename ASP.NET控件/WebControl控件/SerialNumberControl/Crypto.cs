using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MTFramework.Web.Controls
{
    public class Crypto
    {
        #region 成员
        private byte[] _CryptIV;

        public byte[] CryptIV
        {
            get { return _CryptIV; }
            set { _CryptIV = value; }
        }
        private byte[] _CryptKey;

        public byte[] CryptKey
        {
            get { return _CryptKey; }
            set { _CryptKey = value; }
        }
        private string _CryptText;

        public string CryptText
        {
            get { return _CryptText; }
            set { _CryptText = value; }
        }
        #endregion

        public Crypto() { }

        #region  解密
        public string Decrypt()
        {
            string cryptText = this.CryptText;
            byte[] cryptKey = this.CryptKey;
            byte[] cryptIV = this.CryptIV;
            byte[] buffer = Convert.FromBase64String(cryptText);
            RijndaelManaged managed = new RijndaelManaged();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, managed.CreateDecryptor(cryptKey, cryptIV), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Encoding.Default.GetString(stream.ToArray());

        }
        #endregion

        #region 加密
        public string Encrypt()
        {
            string cryptText = this.CryptText;
            byte[] cryptKey = this.CryptKey;
            byte[] cryptIV = this.CryptIV;
            byte[] bytes = Encoding.Default.GetBytes(cryptText);
            RijndaelManaged managed = new RijndaelManaged();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, managed.CreateEncryptor(cryptKey, cryptIV), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }
        #endregion
    }

}
