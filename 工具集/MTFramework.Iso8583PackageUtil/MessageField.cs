using MTFramework.Utils.ByteUtil;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MTFramework.Iso8583PackageUtil
{
    [Serializable]
    public class MessageField
    {

        #region --- 字段属性 Begin ---


        private MessageFieldDefinition _Definition;
        /// <summary>
        /// 域定义
        /// </summary>
        public MessageFieldDefinition Definition
        {
            get
            {
                return this._Definition;
            }
            set
            {
                this._Definition = value;
            }
        }



        private object _Value;
        /// <summary>
        /// 域值
        /// </summary>
        public object Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }


        #endregion --- 字段属性 End ---

        #region --- ctor Begin ---

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="definition">域定义</param>
        /// <param name="value">域值</param>
        public MessageField(MessageFieldDefinition definition, object value)
        {
            this._Definition = definition;
            this._Value = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageField()
        {

        }
        #endregion --- ctor End ---

        /// <summary>
        /// 克隆方法
        /// </summary>
        /// <returns></returns>
        public MessageField Clone()
        {
            return MTFramework.Utils.SerializationHelper.ObjectSerializationHelper.Clone(this);
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public new bool Equals(object obj)
        {
            MessageField eqobj = obj as MessageField;

            if (eqobj == null)
            {
                return false;
            }
            return (this._Definition.Equals(eqobj) == true && this._Value.Equals(eqobj.Value) == true);
        }


        public override string ToString()
        {
            if (this.Value == null)
            {
                return "FieldValue<null>";
            }
            MessageFieldType type = this.Definition.Type;
            int length = this.Definition.Length;
            TransportMode m = this.Definition.TransportMode;
            byte[] buf = null;
            if (this.Value is string)
            {
                string temp = (string)this.Value;

                buf = Encoding.Default.GetBytes(temp);
                if (type == MessageFieldType.LLVAR || type == MessageFieldType.LLLVAR)
                {
                    return Encoding.Default.GetString(buf);
                }
                buf = this.Definition.Fill(buf, length);
                return Encoding.Default.GetString(buf);
            }

            if (this.Value is DateTime)
            {
                return this.Definition.Format(Convert.ToDateTime(this.Value));
            }
            if (this.Value is decimal)
            {
                return this.Definition.Format(Convert.ToDecimal(this.Value), length);
            }
            Regex rx = new Regex(@"^\d+$");
            if (this.Value is long || rx.IsMatch(Convert.ToString(this.Value)) == true)
            {
                return this.Definition.Format(Convert.ToInt64(this.Value), length);
            }
            if (this.Value is byte[])
            {
                return Encoding.Default.GetString((byte[])this.Value);
            }
            buf = Encoding.Default.GetBytes(this.Value.ToString());
            buf = this.Definition.Fill(buf, length);
            return Encoding.Default.GetString(buf);
        }

        /// <summary>
        /// 将域值写入输出流
        /// </summary>
        /// <param name="os"></param>
        /// <param name="binary"></param>
        public void Write(BinaryWriter os, bool binary)
        {
            MessageFieldType type = this.Definition.Type;
            int length = this.Definition.Length;
            TransportMode m = this.Definition.TransportMode;
            byte[] buf = null;
            if (type == MessageFieldType.LLVAR || type == MessageFieldType.LLLVAR)
            {
                if (this.Value is byte[])
                {
                    length = ((byte[])this.Value).Length;
                }
                else
                {
                    length = ToString().Length;
                }

                if (binary == true)
                {
                    if (type == MessageFieldType.LLLVAR)
                    {
                        // 00 to 09 automatically in BCD
                        //byte[] bb = Encoding.Default.GetBytes((length / 100).ToString());
                        //ByteUtil.HexStrToByte
                        //ByteUtil.ConvertFrom
                        os.Write((byte)(length / 100));
                    }
                    // BCD encode the rest of the length
                    //byte[] bb2 = Encoding.Default.GetBytes(((((length % 100) / 10) << 4) | (length % 10)).ToString());
                    os.Write((byte)((((length % 100) / 10) << 4) | (length % 10)));
                }
                else
                {
                    // write the length in ASCII
                    if (type == MessageFieldType.LLLVAR)
                    {
                        //byte[] bb = Encoding.Default.GetBytes(((length / 100) + 0).ToString());
                        //os.Write(bb, 0, bb.Length);
                        os.Write((byte)((length / 100) + 0));
                    }
                    if (length >= 10)
                    {
                        //byte[] bb = Encoding.Default.GetBytes((((length % 100) / 10) + 0).ToString());
                        //os.Write(bb, 0, bb.Length);
                        os.Write((byte)(((length % 100) / 10) + 0));
                    }
                    else
                    {
                        //byte[] bb = Encoding.Default.GetBytes("0");
                        //os.Write(bb, 0, bb.Length);
                        os.Write((byte)0);
                    }
                    //byte[] bb2 = Encoding.Default.GetBytes(((length % 10) + 0).ToString());
                    //os.Write(bb2, 0, bb2.Length);
                    os.Write((byte)((length % 10) + 0));

                }
            }

            if (m == TransportMode.BINARY)
            {
                if (this.Value is byte[])
                {
                    buf = (byte[])this.Value;
                }
                else
                {
                    buf = Encoding.Default.GetBytes(ToString());
                }
            }
            else if (m == TransportMode.LEFT_BCD || m == TransportMode.RIGHT_BCD)
            {
                if (this.Value is byte[])
                {
                    buf = (byte[])this.Value;
                    buf = ByteConvert.StringToBcd(ByteConvert.ByteToHexString(buf));
                }
                else
                {
                    string temp = ToString();
                    if (temp.Length % 2 == 1)
                    {
                        buf = this.Definition.Fill(Encoding.Default.GetBytes(temp), temp.Length + 1);
                        temp = Encoding.Default.GetString(buf);
                    }
                    buf = ByteConvert.HexStringToBytes(temp);
                }
            }
            else
            {
                if (this.Value is byte[])
                {
                    buf = (byte[])this.Value;
                }
                else
                {
                    buf = Encoding.Default.GetBytes(ToString());
                }
            }
            //os.Write(buf, 0, buf.Length);
            os.Write(buf);
        }
    }
}
