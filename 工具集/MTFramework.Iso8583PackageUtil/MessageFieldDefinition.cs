using System;
using System.Text;

namespace MTFramework.Iso8583PackageUtil
{
    public class MessageFieldDefinition
    {
        #region --- 字段属性 Begin ---

        private int _Index;

        /// <summary>
        /// 域序号
        /// </summary>
        public int Index
        {
            get
            {
                return this._Index;
            }
            set
            {
                this._Index = value;
            }
        }

        private MessageFieldType _Type;

        /// <summary>
        /// 域类型
        /// </summary>
        public MessageFieldType Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
            }
        }

        private int _Length;

        /// <summary>
        /// 域长度
        /// </summary>
        public int Length
        {
            get
            {
                return this._Length;
            }
            set
            {
                this._Length = value;
            }
        }


        private TransportMode _TransportMode;

        /// <summary>
        /// 传输方式
        /// </summary>
        public TransportMode TransportMode
        {
            get
            {
                return this._TransportMode;
            }
            set
            {
                this._TransportMode = value;
            }
        }

        #endregion --- 字段属性 End ---

        #region --- ctor Begin ---

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="transportMode"></param>
        public MessageFieldDefinition(int index, MessageFieldType type, int length, TransportMode transportMode)
        {
            this._Index = index;
            this._Type = type;
            this._Length = length;
            this._TransportMode = transportMode;
        }

        #endregion --- ctor End ---

        #region --- Functions Begin ---

        public byte[] Fill(byte[] src, int length)
        {
            if (src == null || length <= 0 || src.Length == length)
            {
                return src;
            }

            byte[] buf = new byte[length];
            switch (this.TransportMode)
            {
                case TransportMode.BINARY:
                    buf = src;
                    break;
                case TransportMode.LEFT_ASCII:
                    if (src.Length > length)
                    {
                        Array.Copy(src, 0, buf, 0, length);
                        break;
                    }
                    Array.Copy(src, 0, buf, 0, src.Length);
                    for (int i = src.Length; i < length; i++)
                    {
                        buf[i] = Encoding.Default.GetBytes(" ")[0];
                    }
                    break;
                case TransportMode.RIGHT_ASCII:
                    if (src.Length > length)
                    {
                        Array.Copy(src, src.Length - length, buf, 0, length);
                        break;
                    }
                    for (int i = 0; i < length - src.Length; i++)
                    {
                        buf[i] = Encoding.Default.GetBytes(" ")[0];
                    }
                    Array.Copy(src, 0, buf, length - src.Length, src.Length);
                    break;
                case TransportMode.LEFT_BCD:
                    if (src.Length > length)
                    {
                        Array.Copy(src, 0, buf, 0, length);
                        break;
                    }
                    Array.Copy(src, 0, buf, 0, src.Length);
                    for (int i = src.Length; i < length; i++)
                    {
                        buf[i] = Encoding.Default.GetBytes("0")[0];
                    }
                    break;
                case TransportMode.RIGHT_BCD:
                    if (src.Length > length)
                    {
                        Array.Copy(src, src.Length - length, buf, 0, length);
                        break;
                    }
                    for (int i = 0; i < length - src.Length; i++)
                    {
                        buf[i] = Encoding.Default.GetBytes("0")[0];
                    }
                    Array.Copy(src, 0, buf, length - src.Length, length);
                    break;
                default:
                    break;
            }
            return buf;
        }


        public string Format(decimal value, int length)
        {
            if (this.Type == MessageFieldType.AMOUNT)
            {
                string v = value.ToString(value < 0 ? "000000000.00" : "0000000000.00");

                return v.Substring(0, 10) + v.Substring(11);
            }
            else if (this.Type == MessageFieldType.NUMERIC)
            {
                long va = Convert.ToInt64(value);
                return this.Format(va, length);
            }
            else if (this.Type == MessageFieldType.ALPHA || this.Type == MessageFieldType.LLVAR || this.Type == MessageFieldType.LLLVAR)
            {
                return this.Format(value.ToString(), length);
            }

            throw new ArgumentException("Cannot format BigDecimal as " + this.Type);
        }

        public String Format(DateTime value)
        {
            switch (this.Type)
            {
                case MessageFieldType.DATE:
                    return value.ToString("yyyyMMdd");
                case MessageFieldType.DATE_EXP:
                    return value.ToString("yyMM");
                case MessageFieldType.DATE10:
                    return value.ToString("MMddHHmmss");
                case MessageFieldType.DATE4:
                    return value.ToString("MMdd");
                case MessageFieldType.TIME:
                    return value.ToString("HHmmss");
                default:
                    break;
            }
            throw new ArgumentException("Cannot format DateTime as " + this.Type);
        }

        public string Format(long value, int length)
        {
            if (this.Type == MessageFieldType.NUMERIC)
            {
                char[] c = new char[length];
                char[] x = value.ToString().ToCharArray();
                if (x.Length > length)
                {
                    throw new ArgumentOutOfRangeException("Numeric value is larger than intended length: " + value + " LEN " + length);
                }
                int lim = c.Length - x.Length;
                for (int i = 0; i < lim; i++)
                {
                    c[i] = '0';
                }
                Array.Copy(x, 0, c, lim, x.Length);
                return new String(c);
            }
            else if (this.Type == MessageFieldType.ALPHA || this.Type == MessageFieldType.LLVAR || this.Type == MessageFieldType.LLLVAR)
            {
                return Format(value.ToString(), length);
            }
            else if (this.Type == MessageFieldType.AMOUNT)
            {
                string v = value.ToString();
                char[] digits = new char[12];
                for (int i = 0; i < 12; i++)
                {
                    digits[i] = '0';
                }
                Array.Copy(v.ToCharArray(), 0, digits, 10 - v.Length, v.Length);
                return new String(digits);
            }
            throw new ArgumentException("Cannot format long as " + this.Type);
        }

        public string Format(string value, int length)
        {
            if (this.Type == MessageFieldType.ALPHA)
            {
                if (string.IsNullOrEmpty(value) == true)
                {
                    value = "";
                }
                byte[] buf = new byte[length];
                byte[] tmp = Encoding.Default.GetBytes(value);
                if (tmp.Length > length)
                {
                    Array.Copy(tmp, 0, buf, 0, length);
                    return Encoding.Default.GetString(buf);
                }
                Array.Copy(tmp, 0, buf, 0, tmp.Length);
                for (int i = tmp.Length; i < length; i++)
                {
                    buf[i] = Encoding.Default.GetBytes(" ")[0];
                }
                return Encoding.Default.GetString(buf);
            }
            else if (this.Type == MessageFieldType.LLVAR || this.Type == MessageFieldType.LLLVAR)
            {
                return value;
            }
            else if (this.Type == MessageFieldType.NUMERIC)
            {
                char[] c = new char[length];
                char[] x = value.ToCharArray();
                if (x.Length > length)
                {
                    throw new ArgumentOutOfRangeException("Numeric value is larger than intended length: " + value + " LEN " + length);
                }
                int lim = c.Length - x.Length;
                for (int i = 0; i < lim; i++)
                {
                    c[i] = '0';
                }
                Array.Copy(x, 0, c, lim, x.Length);
                return new String(c);
            }
            throw new ArgumentException("Cannot format string as " + this.Type);
        }

        public object Parse(string value)
        {
            if (value == null)
            {
                return null;
            }

            if (this.Type == MessageFieldType.NUMERIC)
            {
                return Convert.ToInt64(value);
            }
            else if (this.Type == MessageFieldType.ALPHA)
            {
                return value;
            }
            else if (this.Type == MessageFieldType.DATE10)
            {
                if (value.Length != 10)
                {
                    return null;
                }
                else
                {
                    int mm = Convert.ToInt16(value.Substring(0, 2));
                    int dd = Convert.ToInt16(value.Substring(2, 2));
                    int h = Convert.ToInt16(value.Substring(4, 2));
                    int m = Convert.ToInt16(value.Substring(6, 2));
                    int s = Convert.ToInt16(value.Substring(8, 2));
                    return new DateTime(DateTime.Now.Year, mm, dd, h, m, s);
                }
            }
            else if (this.Type == MessageFieldType.DATE4)
            {
                if (value.Length != 4)
                {
                    return null;
                }
                else
                {
                    int mm = Convert.ToInt16(value.Substring(0, 2));
                    int dd = Convert.ToInt16(value.Substring(2, 2));
                    return new DateTime(DateTime.Now.Year, mm, dd, 0, 0, 0);
                }
            }
            else if (this.Type == MessageFieldType.DATE_EXP)
            {
                if (value.Length != 4)
                {
                    return null;
                }
                else
                {
                    int yy = Convert.ToInt16(value.Substring(0, 2));
                    int mm = Convert.ToInt16(value.Substring(2, 2));
                    return new DateTime(yy, mm, 1, 0, 0, 0);
                }
            }
            else if (this.Type == MessageFieldType.TIME)
            {
                if (value.Length != 6)
                {
                    return null;
                }
                else
                {
                    int h = Convert.ToInt16(value.Substring(0, 2));
                    int m = Convert.ToInt16(value.Substring(2, 2));
                    int s = Convert.ToInt16(value.Substring(4, 2));
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, h, m, s);
                }
            }
            else if (this.Type == MessageFieldType.AMOUNT)
            {
                decimal v = Convert.ToDecimal(value);
                return System.Decimal.Round(v / 100, 2);
            }
            return value;
        }

        #endregion --- Functions End ---

    }
}
