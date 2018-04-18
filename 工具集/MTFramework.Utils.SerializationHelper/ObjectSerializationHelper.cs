using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MTFramework.Utils.SerializationHelper
{
    /// <summary>
    /// 对象序列化工具
    /// </summary>
    public class ObjectSerializationHelper
    {
        /// <summary>
        /// 序列化集合
        /// </summary>
        private static Dictionary<int, XmlSerializer> xmlSerializer_dict = new Dictionary<int, XmlSerializer>();

        #region --- Functions Begin ---

        /// <summary>
        /// 获取对象类型的序列化Xml格式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>Xml格式</returns>
        private static XmlSerializer GetXmlSerializer<T>() where T : new()
        {
            int tHash = typeof(T).GetHashCode();

            if (!xmlSerializer_dict.ContainsKey(tHash))
            {
                xmlSerializer_dict.Add(tHash, new XmlSerializer(typeof(T)));
            }
            return xmlSerializer_dict[tHash];
        }

        /// <summary>
        /// 把对象序列化为字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>序列化后的字符串，失败则会抛出异常信息</returns>
        public static string SerializeToXmlString<T>(T t) where T : new()
        {
            string str = string.Empty;

            XmlSerializer xml = GetXmlSerializer<T>();
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                xml.Serialize(xtw, t);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                str = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
                if (sr != null)
                    sr.Close();
                ms.Close();
            }

            return str;
        }

        /// <summary>
        /// 从字符窜反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类</typeparam>
        /// <param name="s">需要反序列化的字符串</param>
        /// <returns>该对象实例,失败则会抛出异常信息</returns>
        public static T DeSerializeForXmlString<T>(string s) where T : new()
        {
            byte[] bArray = System.Text.Encoding.UTF8.GetBytes(s);

            try
            {
                XmlSerializer xml = GetXmlSerializer<T>();
                return (T)xml.Deserialize(new MemoryStream(bArray));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对象序列化为Byte数组
        /// </summary>
        /// <typeparam name="T">对象类</typeparam>
        /// <param name="t">来源对象</param>
        /// <returns>返回结果</returns>
        public static byte[] SerializeToBytes<T>(T t) where T : new()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    byte[] buf = null;
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, t);
                    buf = stream.ToArray();
                    stream.Close();
                    return buf;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 序列化为Byte数组对象还原为对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="buf">对象序列化后数据</param>
        /// <returns></returns>
        public static T DeSerializeForBytes<T>(byte[] buf) where T : new()
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(buf))
                {
                    IFormatter formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 简单克隆利用Xml模式进行序列化克隆
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实体</param>
        /// <returns>对象副本</returns>
        public static T SimpleClone<T>(T obj) where T : new()
        {
            try
            {
                string xmlString = SerializeToXmlString(obj);

                return DeSerializeForXmlString<T>(xmlString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 高级克隆利用内存流模式进行序列化克隆
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实体</param>
        /// <returns>对象副本</returns>
        public static T AdvancedClone<T>(T obj) where T : new()
        {
            try
            {
                byte[] buf = SerializeToBytes(obj);
                return DeSerializeForBytes<T>(buf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="obj">需要克隆的对象</param>
        /// <returns>返回当前对象的克隆对象</returns>
        public static T Clone<T>(T obj) where T : new()
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                    stream.Position = 0L;
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion --- Functions End ---
    }
}
