using System;
using System.IO;
using System.Xml.Serialization;

namespace MTFramework.Utils.SerializationHelper
{
    /// <summary>
    /// XmlSerializationHelper Xml序列化工具
    /// </summary>
    public class XmlSerializationHelper
    {
        #region --- Functions Begin ---

        /// <summary>
        /// 从Xml文档读取获得对象
        /// </summary>
        /// <typeparam name="T">对象类</typeparam>
        /// <param name="fileFullPath">xml文件完整路径</param>
        /// <returns>对象实体，失败抛出异常</returns>
        public static T LoadXmlFile<T>(string fileFullPath) where T : new()
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer xml = new XmlSerializer(typeof(T));
                return (T)xml.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 保存对象到Xml文档
        /// </summary>
        /// <typeparam name="T">对象类</typeparam>
        /// <param name="t">对象</param>
        /// <param name="fileFullPath">xml文件完整路径</param>
        /// <returns>成功返回 True 抛出异常</returns>
        public static bool Save<T>(T t, string fileFullPath) where T : new()
        {
            bool bl = false;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(fs, t);
                bl = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return bl;
        }
        #endregion --- Functions End ---
    }

}
