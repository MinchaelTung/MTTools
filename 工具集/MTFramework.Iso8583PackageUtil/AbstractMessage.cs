using MTFramework.Utils.ByteUtil;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTFramework.Iso8583PackageUtil
{
    public abstract class AbstractMessage : IBaseMessage
    {

        /// <summary>
        /// 报文类型ID
        /// </summary>
        private string typeId;

        /// <summary>
        /// 报文头
        /// </summary>        
        private string header;

        /// <summary>
        /// 报文包含的域
        /// </summary>
        private SortedList<int, MessageField> fields = new SortedList<int, MessageField>();


        public string TypeID
        {
            get
            {
                return typeId;
            }
            set
            {
                typeId = value;
            }
        }

        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }



        /// <summary>
        /// 获取位图
        /// </summary>
        /// <returns></returns>
        public BitArray Bitmap
        {
            get
            {
                BitArray bitmap = new BitArray(Protocol.MaxFieldCount);

                foreach (int index in fields.Keys)
                {
                    bitmap.Set(index - 1, true);
                }

                if (bitmap.Length > 64)
                {
                    bitmap.Set(0, true);
                }
                return bitmap;
            }
        }

        public byte[] GetContent()
        {
            try
            {
                return WriteInternal();
            }
            catch (IOException e)
            {
                throw e;
            }
            //return null;
        }

        public MessageField GetField(int index)
        {
            if (fields.ContainsKey(index) == true)
            {
                return fields[index];
            }
            else
            {
                return null;
            }
        }

        public MessageField this[int index]
        {
            get
            {
                if (fields.ContainsKey(index) == true)
                {
                    return fields[index];
                }
                else
                {
                    return null;
                }
            }
        }


        public bool IsDataNullMessageField(int index)
        {
            return !this.Bitmap[index];
        }


        public void SetFieldValue(int index, object obj)
        {
            MessageFieldDefinition definition = Protocol.GetFieldDefinition(index);
            if (definition == null)
            {
                return;
            }
            if (obj == null)
            {
                fields.Remove(index);
            }
            else
            {
                MessageField field = new MessageField(Protocol.GetFieldDefinition(index), obj);
                if (fields.ContainsKey(index) == false)
                {
                    fields.Add(index, field);
                }
                else
                {
                    fields[index] = field;
                }
            }
        }
        /// <summary>
        /// 设置MAC校验码
        /// </summary>
        /// <param name="index"></param>
        public void SetMacField(int index)
        {
            this.SetFieldValue(index, new byte[8]);
            this.SetFieldValue(index, this.GetMac());

        }
        /// <summary>
        /// 验证MAC校验码是否正确
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ValidationMac(int index)
        {
            string resMac = Encoding.UTF8.GetString((byte[])this[index].Value);
            this.SetFieldValue(index, new byte[8]);
            return this.GetMac().Equals(resMac, System.StringComparison.InvariantCultureIgnoreCase);

        }
        

        public void SetField(int index, MessageField field)
        {
            MessageFieldDefinition definition = Protocol.GetFieldDefinition(index);
            if (definition == null)
            {
                return;
            }
            if (field == null)
            {
                fields.Remove(index);
            }
            else
            {
                fields.Add(index, field);
            }
        }

        /// <summary>
        /// 输出报文头
        /// </summary>
        /// <param name="os"></param>
        protected abstract void WriteHeader(BinaryWriter baos);

        /// <summary>
        /// 将报文内容写入缓存, 不包括报文长度
        /// </summary>
        /// <returns></returns>
        protected byte[] WriteInternal()
        {

            MemoryStream ms = new MemoryStream();
            BinaryWriter baos = new BinaryWriter(ms);
            // 报文头
            WriteHeader(baos);
            // 报文类型
            if (Protocol.IsBinary)
            {
                byte[] tmp = ByteConvert.StringToBcd(typeId);
                baos.Write(tmp);

            }
            else
            {
                byte[] tmp = Encoding.Default.GetBytes(typeId);
                baos.Write(tmp);

            }

            // 位图
            List<int> keys = new List<int>();
            keys.AddRange(fields.Keys);
            keys.Sort();
            BitArray bs = new BitArray(64);
            foreach (int i in keys)
            {
                bs.Set(i - 1, true);
            }

            if (bs.Length > 64)
            {
                bs.Set(0, true);
            }
            int pos = 128;
            int b = 0;
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs.Get(i))
                {
                    b |= pos;
                }
                pos >>= 1;
                if (pos == 0)
                {
                    // byte[] bb = Encoding.Default.GetBytes(b.ToString());
                    baos.Write((byte)b);
                    pos = 128;
                    b = 0;
                }
            }
            // 写入报文域            
            foreach (int i in keys)
            {
                MessageField field = fields[i];
                field.Write(baos, Protocol.IsBinary);
            }
            return ms.ToArray();
        }

        public abstract BaseMessageProtocol Protocol { get; }

        public abstract void SetHeader(byte[] bytes);

        public abstract string GetMac();



        public override string ToString()
        {
            string tmp = string.Empty;

            foreach (var item in fields)
            {
                tmp += "[" + item.Key + "]\t" + item.Value.ToString() + "\n";
            }
            return tmp;
        }

    }
}
