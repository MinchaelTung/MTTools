using System;
using System.IO;
using System.Security.Cryptography;

namespace MTFramework.Utils.Cryptography
{
    /// <summary>
    /// DES、3DES 加解密；MAC算法
    /// </summary>
    public sealed class DES_Cryptographycs
    {
        #region --- 字段 Begin ---

        private PaddingMode mPaddingMode;
        private CipherMode mCipherMode;
        private byte[] mbyKey;
        private byte[] mbyIV;

        #endregion --- 字段 End ---

        #region --- 构造方法 Begin ---

        /// <summary>
        /// 构造函数
        /// </summary>
        public DES_Cryptographycs()
        {
            this.mbyIV = new byte[8];
            this.mbyKey = new byte[8];
            this.mCipherMode = CipherMode.CBC;
            this.mPaddingMode = PaddingMode.None;
        }

        #endregion --- 构造方法 End ---

        #region --- 属性 Begin ---

        /// <summary>
        /// DES指定用于加密的块密码模式
        /// </summary>
        public CipherMode CipherMode
        {
            get
            {
                return this.mCipherMode;
            }
            set
            {
                this.mCipherMode = value;
            }
        }

        /// <summary>
        /// 指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型
        /// </summary>
        public PaddingMode PaddingMode
        {
            get
            {
                return this.mPaddingMode;
            }
            set
            {
                this.mPaddingMode = value;
            }
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public byte[] Key
        {
            get
            {
                return this.mbyKey;
            }
            set
            {
                this.mbyKey = value;
            }
        }

        /// <summary>
        /// 初始化向量
        /// </summary>
        public byte[] IV
        {
            get
            {
                return this.mbyIV;
            }
            set
            {
                this.mbyIV = value;
            }
        }

        #endregion --- 属性 End ---

        #region --- DES 加解密 Begin ---

        /// <summary>
        /// DES
        /// </summary>
        /// <param name="data">源数据</param>
        /// <returns>返回加密结果</returns>
        public byte[] DES_Encrypt(byte[] data)
        {
            try
            {
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                //计算des加密所采用的算法
                MyServiceProvider.Mode = this.mCipherMode;
                //计算填充类型
                MyServiceProvider.Padding = this.mPaddingMode;
                //创建加密对象
                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(this.mbyKey, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到加密流中
                    MyCryptoStream.Write(data, 0, data.Length);
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] byEncRet = ms.ToArray();
                    ms.Close();
                    return byEncRet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">加密后数据</param>
        /// <returns>返回解密结果</returns>
        public byte[] DES_Decrypt(byte[] data)
        {
            try
            {
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                //计算des加密所采用的算法
                MyServiceProvider.Mode = this.mCipherMode;
                //计算填充类型
                MyServiceProvider.Padding = this.mPaddingMode;
                //创建解密对象
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(this.mbyKey, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到加密流中
                    MyCryptoStream.Write(data, 0, data.Length);
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] byEncRet = ms.ToArray();
                    ms.Close();
                    return byEncRet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion --- DES 加解密 End ---

        #region --- MAC 算法 Begin ---

        /// <summary>
        /// MAC计算所要采用的CBC DES算法实现加密
        /// </summary>
        /// <param name="key">Key数据</param>
        /// <param name="data">原数据</param>
        /// <returns>返回加密后结果</returns>
        public byte[] HCDES_Encrypt(byte[] key, byte[] data)
        {
            try
            {
                //创建一个DES算法的加密类
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                MyServiceProvider.Mode = CipherMode.CBC;
                MyServiceProvider.Padding = PaddingMode.None;
                //从DES算法的加密类对象的CreateEncryptor方法,创建一个加密转换接口对象
                //第一个参数的含义是：对称算法的机密密钥(长度为64位,也就是8个字节)
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateKey();
                //第二个参数的含义是：对称算法的初始化向量(长度为64位,也就是8个字节)
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateIV()
                //创建加密对象
                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(key, new byte[8]);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到加密流中
                    MyCryptoStream.Write(data, 0, data.Length);
                    //MyCryptoStream关闭之前ms.Length 为8， 关闭之后为16
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] bTmp = ms.ToArray();
                    ms.Close();
                    return bTmp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// MAC计算所要采用的CBC DES算法实现解密
        /// </summary>
        /// <param name="key">Key数据</param>
        /// <param name="data">加密后数据</param>
        /// <returns>返回解密结果</returns>
        public byte[] HCDES_Decrypt(byte[] key, byte[] data)
        {
            try
            {
                //创建一个DES算法的加密类
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                MyServiceProvider.Mode = CipherMode.CBC;
                MyServiceProvider.Padding = PaddingMode.None;
                //从DES算法的加密类对象的CreateEncryptor方法,创建一个加密转换接口对象
                //第一个参数的含义是：对称算法的机密密钥(长度为64位,也就是8个字节)
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateKey();
                //第二个参数的含义是：对称算法的初始化向量(长度为64位,也就是8个字节)
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateIV()
                //创建解密对象
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(key, new byte[8]);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到解密流中
                    MyCryptoStream.Write(data, 0, data.Length);
                    // MyCryptoStream关闭之前ms.Length 为8， 关闭之后为16
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] bTmp = ms.ToArray();
                    ms.Close();
                    return bTmp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// MAC计算 (ANSI-X9.9-MAC)
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>返回该数据MAC值</returns>
        public byte[] GetMAC(byte[] data)
        {
            try
            {
                int iGroup = 0;
                byte[] bKey = this.mbyKey;
                byte[] bIV = this.mbyIV;
                byte[] bTmpBuf1 = new byte[8];
                byte[] bTmpBuf2 = new byte[8];
                // init
                Array.Copy(bIV, bTmpBuf1, 8);
                if ((data.Length % 8 == 0))
                {
                    iGroup = data.Length / 8;
                }
                else
                {
                    iGroup = data.Length / 8 + 1;
                }
                int i = 0;
                int j = 0;
                for (i = 0; i < iGroup; i++)
                {
                    Array.Copy(data, 8 * i, bTmpBuf2, 0, 8);
                    for (j = 0; j < 8; j++)
                    {
                        bTmpBuf1[j] = (byte)(bTmpBuf1[j] ^ bTmpBuf2[j]);
                    }
                    bTmpBuf2 = HCDES_Encrypt(bKey, bTmpBuf1);
                    Array.Copy(bTmpBuf2, bTmpBuf1, 8);
                }
                return bTmpBuf2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion --- MAC 算法 End ---

        #region --- 3DES 加解密 Begin ---

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <returns>返回加密结果</returns>
        public byte[] TripleDES_Encrypt(byte[] data)
        {
            try
            {
                TripleDESCryptoServiceProvider MyServiceProvider = new TripleDESCryptoServiceProvider();
                //计算des加密所采用的算法
                MyServiceProvider.Mode = this.mCipherMode;
                //计算填充类型
                MyServiceProvider.Padding = this.mPaddingMode;
                //TripleDESCryptoServiceProvider
                //支持从 128 位到 192 位（以 64 位递增）的密钥长度
                //IV需要8个字节
                //设置KEY时要注意的是可能引发CryptographicException异常，主要是因为所设置的KEY为WeakKey
                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(this.mbyKey, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到加密流中
                    MyCryptoStream.Write(data, 0, data.Length);
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] byEncRet = ms.ToArray();
                    ms.Close();
                    return byEncRet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="data">加密后数据</param>
        /// <returns>返回解密结果</returns>
        public byte[] TripleDES_Decrypt(byte[] data)
        {
            try
            {
                TripleDESCryptoServiceProvider MyServiceProvider = new TripleDESCryptoServiceProvider();
                //计算des加密所采用的算法
                MyServiceProvider.Mode = this.mCipherMode;
                //计算填充类型
                MyServiceProvider.Padding = this.mPaddingMode;
                //TripleDESCryptoServiceProvider
                //支持从 128 位到 192 位（以 64 位递增）的密钥长度
                //IV需要8个字节
                //设置KEY时要注意的是可能引发CryptographicException异常，主要是因为所设置的KEY为WeakKey
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(this.mbyKey, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到加密流中
                    MyCryptoStream.Write(data, 0, data.Length);
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] byEncRet = ms.ToArray();
                    ms.Close();
                    return byEncRet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion --- 3DES 加解密 End ---
    }
}
