using MTFramework.Utils.ByteUtil;
using MTFramework.Utils.Cryptography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTFramework.Iso8583PackageUtil
{
    public class MessageHelper : AbstractMessage
    {
        private DES_Cryptographycs _GneteCryptography;

        public void SetKey(byte[] key)
        {
            _GneteCryptography.Key = key;
        }

        public void SetIV(byte[] iv)
        {
            _GneteCryptography.IV = iv;
        }

        public void SetKeyAndIV(byte[] key, byte[] iv)
        {
            _GneteCryptography.Key = key;
            _GneteCryptography.IV = iv;
        }
        public MessageHelper()
        {
            _GneteCryptography = new DES_Cryptographycs();
            //this._GneteCryptography.Key = ByteConvert.HexStringToBytes("0123456789ABCDEF");
            //this._GneteCryptography.IV = ByteConvert.HexStringToBytes("0123456789ABCDEF");
            //Header = "1234567890123456789012";

        }

        public MessageHelper(byte[] key, byte[] iv)
        {
            _GneteCryptography = new DES_Cryptographycs();
            _GneteCryptography.Key = key;
            _GneteCryptography.IV = iv;
            //MessageUtils.GneteCryptography = _GneteCryptography;
        }
        protected override void WriteHeader(BinaryWriter os)
        {
            //Header = "6000430000602200000000";
            byte[] buf = ByteConvert.StringToBcd(Header);
            os.Write(buf);
        }

        public override BaseMessageProtocol Protocol
        {
            get
            {
                return MessagePackageProtocol.GetInstance();
            }
        }
        /// <summary>
        /// 设置包头
        /// </summary>
        /// <param name="bytes"></param>
        public override void SetHeader(byte[] bytes)
        {
            string value = ByteConvert.Bcd2String(bytes);
            Header = value;
        }

        /// <summary>
        /// 获取MAC
        /// </summary>
        /// <returns></returns>
        public override string GetMac()
        {
            byte[] content = GetContent();

            int len = content.Length - Protocol.HeaderLength;
            if (this[64] != null)
            {
                len -= 8;
            }
            byte[] buf = new byte[len];
            Array.Copy(content, Protocol.HeaderLength, buf, 0, len);
            //System.arraycopy(content, getProtocol().getHeaderLength(), buf, 0, len);
            try
            {
                int N8 = (len - 1) / 8 + 1;
                byte[] round_data = new byte[N8 * 8];
                Array.Copy(buf, 0, round_data, 0, len);
                //System.arraycopy(buf, 0, round_data, 0, len);
                byte[] macBytes = new byte[8];
                for (int i = 0; i < N8; i++)
                {
                    macBytes = makeXOR(macBytes, round_data, i * 8, 8);
                }
                string temp = ByteConvert.ByteToHexString(macBytes);
                byte[] b1 = Encoding.Default.GetBytes(temp.Substring(0, 8));
                byte[] b2 = Encoding.Default.GetBytes(temp.Substring(8, 8));

                byte[] macKey = _GneteCryptography.HCDES_Decrypt(_GneteCryptography.Key, _GneteCryptography.IV);

                macBytes = _GneteCryptography.HCDES_Encrypt(macKey, b1);
                b1 = makeXOR(macBytes, b2, 0, 8);
                macBytes = _GneteCryptography.HCDES_Encrypt(macKey, b1);
                temp = ByteConvert.ByteToHexString(macBytes);
                return temp.Substring(0, 8);
            }
            catch (Exception)
            {
                //logger.error("计算mac失败", e);
                //return null;
                //throw e;               
            }
            return null;
        }

        private byte[] makeXOR(byte[] b1, byte[] b2, int start, int n)
        {
            for (int i = 0; i < n; i++)
            {
                b1[i] = (byte)(b1[i] ^ b2[start + i]);
            }
            return b1;
        }

        /// <summary>
        /// 转换为包
        /// </summary>
        /// <param name="data"></param>
        public void Parse(byte[] data)
        {
            int offset = 0;
            int length = this.Protocol.HeaderLength;

            // 报文头
            byte[] buf = ByteConvert.Subarray(data, offset, length);
            this.SetHeader(buf);

            offset += length;

            // 2位报文类型
            length = this.Protocol.MessageTypeIdLength;
            if (this.Protocol.IsBinary)
            {
                length >>= 1;
            }

            String temp = null;
            buf = ByteConvert.Subarray(data, offset, length);
            if (this.Protocol.IsBinary)
            {
                temp = ByteConvert.Bcd2String(buf);
            }
            else
            {
                temp = Encoding.Default.GetString(buf);
            }

            this.TypeID = temp;
            offset += length;

            // 8位位图
            length = this.Protocol.BitmapLength;
            buf = ByteConvert.Subarray(data, offset, length);
            BitArray bitmap = new BitArray(64);
            int pos = 0;
            for (int i = 0; i < length; i++)
            {
                int bit = 128;
                for (int b = 0; b < 8; b++)
                {
                    bitmap.Set(pos++, (buf[i] & bit) != 0);
                    bit >>= 1;
                }
            }

            offset += length;

            // 读取数据域
            for (int i = 1; i < bitmap.Length; i++)
            {

                if (!bitmap.Get(i))
                {
                    continue;
                }

                int index = i + 1;
                MessageFieldDefinition definition = this.Protocol.GetFieldDefinition(index);
                if (definition == null)
                {
                    continue;
                }

                length = definition.Length;

                MessageFieldType type = definition.Type;

                if (type == MessageFieldType.LLVAR)
                {
                    if (this.Protocol.IsBinary)
                    {
                        buf = ByteConvert.Subarray(data, offset, 1);
                        length = Convert.ToInt32(ByteConvert.Bcd2String(buf));
                        offset += 1;
                    }
                    else
                    {
                        buf = ByteConvert.Subarray(data, offset, 2);
                        length = Convert.ToInt32(Encoding.Default.GetString(buf));
                        offset += 2;
                    }
                }
                else if (type == MessageFieldType.LLLVAR)
                {
                    if (this.Protocol.IsBinary)
                    {
                        buf = ByteConvert.Subarray(data, offset, 2);
                        length = Convert.ToInt32(ByteConvert.Bcd2String(buf));
                        offset += 2;
                    }
                    else
                    {
                        buf = ByteConvert.Subarray(data, offset, 3);
                        length = Convert.ToInt32(Encoding.Default.GetString(buf));
                        offset += 3;
                    }
                }

                int l = length;

                Object value = null;

                switch (definition.TransportMode)
                {

                    case TransportMode.BINARY:
                        value = ByteConvert.Subarray(data, offset, length);
                        break;

                    case TransportMode.LEFT_BCD:
                        length = (length + 1) / 2;
                        buf = ByteConvert.Subarray(data, offset, length);
                        temp = ByteConvert.Bcd2String(buf);
                        if (temp.Length != l)
                        {
                            temp = temp.Substring(0, temp.Length - 1);
                        }
                        value = temp;
                        break;

                    case TransportMode.RIGHT_BCD:
                        length = (length + 1) / 2;
                        buf = ByteConvert.Subarray(data, offset, length);
                        temp = ByteConvert.Bcd2String(buf);

                        if (temp.Length != l)
                        {
                            temp = temp.Substring(1);
                        }
                        value = temp;
                        break;

                    case TransportMode.LEFT_ASCII:
                        buf = ByteConvert.Subarray(data, offset, length);
                        temp = Encoding.Default.GetString(buf);
                        if (type == MessageFieldType.LLVAR || type == MessageFieldType.LLLVAR)
                        {
                            if (temp.Length > length)
                            {
                                temp = temp.Substring(0, temp.Length - 1);
                            }
                        }
                        value = temp;
                        break;

                    case TransportMode.RIGHT_ASCII:
                        buf = ByteConvert.Subarray(data, offset, length);
                        temp = Encoding.Default.GetString(buf);
                        if (type == MessageFieldType.LLVAR || type == MessageFieldType.LLLVAR)
                        {
                            if (temp.Length > length)
                            {
                                temp = temp.Substring(1);
                            }
                        }
                        value = temp;
                        break;

                    default:
                        buf = ByteConvert.Subarray(data, offset, length);
                        value = Encoding.Default.GetString(buf);
                        break;
                }

                MessageField field = new MessageField(definition, value);
                this.SetField(index, field);

                offset += length;
            }
        }

    }
}
