using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MTFramework.Utils
{
    /// <summary>
    /// 配置文件池
    /// </summary>
    public sealed class XmlConfigUtilPool
    {
        private static Dictionary<string, XmlConfigUtil> dicXmlConfigUtil = new Dictionary<string, XmlConfigUtil>();
        private static ReadOnlyDirectionary<string, XmlConfigUtil> readOnlyDicXmlConfigUtil = null;

        /// <summary>
        /// 添加一个XmlConfigUtilPool
        /// </summary>
        /// <param name="key">索引标记</param>
        /// <param name="filePath">文件相对路径</param>
        public static void Add(string key, string filePath)
        {
            if (dicXmlConfigUtil.ContainsKey(key) == true)
            {
                throw new ArgumentException("key ：[" + key + "]已存在");
            }
            else
            {
                dicXmlConfigUtil.Add(key, XmlConfigUtil.SetXmlFile(filePath));

                readOnlyDicXmlConfigUtil = new ReadOnlyDirectionary<string, XmlConfigUtil>(dicXmlConfigUtil);
            }
        }
        /// <summary>
        /// 读取XmlConfigUtil对象集合
        /// </summary>
        public static ReadOnlyDirectionary<string, XmlConfigUtil> GetXmlConfigUtil
        {
            get
            {
                return readOnlyDicXmlConfigUtil;
            }
        }
    }

    /// <summary>
    /// 读取XML配置文件
    /// </summary>
    public class XmlConfigUtil
    {
        /** xml文档对象 */
        private XmlDocument clsXmlDoc = null;

        /** 文件名 */
        private string CONFIG_FILE = "";

        /** 系统配置文件最后修改时间 */
        private DateTime configFileLastModifyTime;

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="xmlConfigPath">文件相对路径</param>
        /// <returns>返回实例</returns>
        public static XmlConfigUtil SetXmlFile(string xmlConfigPath)
        {

            if (string.IsNullOrEmpty(xmlConfigPath) == true)
            {
                throw new ArgumentNullException("参数不能为空");
            }
            XmlConfigUtil instance = new XmlConfigUtil();
            instance.CONFIG_FILE = xmlConfigPath;
            instance.load();
            return instance;
        }



        private XmlConfigUtil()
        {

        }

        #region --- 基本操作函数 Begin ---

        /// <summary>
        /// 得到程序工作目录
        /// </summary>
        /// <returns></returns>
        private string getWorkDirectory()
        {
            try
            {
                return Path.GetDirectoryName(typeof(XmlConfigUtil).Assembly.Location);
            }
            catch
            {
                return "/";
            }
        }
        /// <summary>
        /// 判断字符串是否为空串
        /// </summary>
        /// <param name="szString">目标字符串</param>
        /// <returns>true:为空串;false:非空串</returns>
        private bool isEmptyString(string szString)
        {
            if (szString == null)
                return true;
            if (szString.Trim() == string.Empty)
                return true;
            return false;
        }
        /// <summary>
        /// 创建一个制定根节点名的XML文件
        /// </summary>
        /// <param name="szFileName">XML文件</param>
        /// <param name="szRootName">根节点名</param>
        /// <returns>bool</returns>
        private bool CreateXmlFile(string szFileName, string szRootName)
        {
            if (szFileName == null || szFileName.Trim() == "")
                return false;
            if (szRootName == null || szRootName.Trim() == "")
                return false;

            XmlDocument clsXmlDoc = new XmlDocument();
            clsXmlDoc.AppendChild(clsXmlDoc.CreateXmlDeclaration("1.0", "GBK", null));
            clsXmlDoc.AppendChild(clsXmlDoc.CreateNode(XmlNodeType.Element, szRootName, ""));
            try
            {
                clsXmlDoc.Save(szFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从XML文件获取对应的XML文档对象
        /// </summary>
        /// <param name="szXmlFile">XML文件</param>
        /// <returns>XML文档对象</returns>
        private XmlDocument getXmlDocument(string szXmlFile)
        {
            if (isEmptyString(szXmlFile))
                return null;
            if (!File.Exists(szXmlFile))
                return null;
            XmlDocument clsXmlDoc = new XmlDocument();
            try
            {
                clsXmlDoc.Load(szXmlFile);
            }
            catch
            {
                return null;
            }
            return clsXmlDoc;
        }

        /// <summary>
        /// 将XML文档对象保存为XML文件
        /// </summary>
        /// <param name="clsXmlDoc">XML文档对象</param>
        /// <param name="szXmlFile">XML文件</param>
        /// <returns>bool:保存结果</returns>
        private bool saveXmlDocument(XmlDocument clsXmlDoc, string szXmlFile)
        {
            if (clsXmlDoc == null)
                return false;
            if (isEmptyString(szXmlFile))
                return false;
            try
            {
                if (File.Exists(szXmlFile))
                    File.Delete(szXmlFile);
            }
            catch
            {
                return false;
            }
            try
            {
                clsXmlDoc.Save(szXmlFile);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取XPath指向的单一XML节点
        /// </summary>
        /// <param name="clsRootNode">XPath所在的根节点</param>
        /// <param name="szXPath">XPath表达式</param>
        /// <returns>XmlNode</returns>
        private XmlNode selectXmlNode(XmlNode clsRootNode, string szXPath)
        {
            if (clsRootNode == null || isEmptyString(szXPath))
                return null;
            try
            {
                return clsRootNode.SelectSingleNode(szXPath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取XPath指向的XML节点集
        /// </summary>
        /// <param name="clsRootNode">XPath所在的根节点</param>
        /// <param name="szXPath">XPath表达式</param>
        /// <returns>XmlNodeList</returns>
        private XmlNodeList selectXmlNodes(XmlNode clsRootNode, string szXPath)
        {
            if (clsRootNode == null || isEmptyString(szXPath))
                return null;
            try
            {
                return clsRootNode.SelectNodes(szXPath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建一个XmlNode并添加到文档
        /// </summary>
        /// <param name="clsParentNode">父节点</param>
        /// <param name="szNodeName">结点名称</param>
        /// <returns>XmlNode</returns>
        private XmlNode createXmlNode(XmlNode clsParentNode, string szNodeName)
        {
            try
            {
                XmlDocument clsXmlDoc = null;
                if (clsParentNode.GetType() != typeof(XmlDocument))
                    clsXmlDoc = clsParentNode.OwnerDocument;
                else
                    clsXmlDoc = clsParentNode as XmlDocument;
                XmlNode clsXmlNode = clsXmlDoc.CreateNode(XmlNodeType.Element, szNodeName, string.Empty);
                if (clsParentNode.GetType() == typeof(XmlDocument))
                {
                    clsXmlDoc.LastChild.AppendChild(clsXmlNode);
                }
                else
                {
                    clsParentNode.AppendChild(clsXmlNode);
                }
                return clsXmlNode;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置指定节点中指定属性的值
        /// </summary>
        /// <param name="clsXmlNode">XML节点</param>
        /// <param name="szAttrName">属性名</param>
        /// <param name="szAttrValue">属性值</param>
        /// <returns>bool</returns>
        private bool setXmlAttr(XmlNode clsXmlNode, string szAttrName, string szAttrValue)
        {
            if (clsXmlNode == null)
                return false;

            XmlAttribute clsAttrNode = clsXmlNode.Attributes.GetNamedItem(szAttrName) as XmlAttribute;
            if (clsAttrNode == null)
            {
                XmlDocument clsXmlDoc = clsXmlNode.OwnerDocument;
                if (clsXmlDoc == null)
                    return false;
                clsAttrNode = clsXmlDoc.CreateAttribute(szAttrName);
                clsXmlNode.Attributes.Append(clsAttrNode);
            }
            clsAttrNode.Value = szAttrValue;
            return true;
        }
        #endregion

        #region"配置文件的读取和写入"

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="xpath">xml Xpath 路径</param>
        /// <returns>Key值</returns>
        private string getConfigData(string xpath)
        {
            if (!assertLoaded() || xpath == null || xpath.Trim() == "")
            {
                return null;
            }
            string[] keys = (xpath.Trim()).Split(new string[] { "/" }, StringSplitOptions.None);
            string szXPath = string.Format(".//Config[@name='{0}']", keys[0]);
            XmlNode clsXmlNode = selectXmlNode(clsXmlDoc, szXPath);
            szXPath = "";
            for (int i = 1; i < keys.Length; i++)
            {
                if (i > 1)
                    szXPath += "/";
                szXPath += keys[i];

            }
            if (clsXmlNode != null)
            {
                return clsXmlNode.SelectSingleNode(szXPath).InnerText;
            }
            return null;
        }

        /// <summary>
        ///  读取指定的配置文件中指定Key的值
        /// </summary>
        /// <param name="xpath">xml Xpath 路径</param>
        /// <param name="szDefaultValue">指定的Key不存在时,返回的值</param>
        /// <returns>Key值</returns>
        public string GetConfig(string xpath, string szDefaultValue)
        {
            string value = null;
            try
            {
                value = getConfigData(xpath);
            }
            catch
            {
                value = null;
            }
            return (value == null ? szDefaultValue : value);
        }

        private void load()
        {
            string szConfigFile = string.Format("{0}\\{1}", getWorkDirectory(), CONFIG_FILE);
            if (!File.Exists(szConfigFile))
            {
                CreateXmlFile(szConfigFile, "SystemConfig");

            }
            configFileLastModifyTime = File.GetLastWriteTime(szConfigFile);
            clsXmlDoc = getXmlDocument(szConfigFile);
        }

        /// <summary>
        ///  保存指定Key的值到指定的配置文件中
        /// </summary>
        /// <param name="xpath">xml Xpath 路径</param>
        /// <param name="szValue">新修改的值</param>
        public bool SetConfig(string xpath, string szValue)
        {
            return writeConfigDataPrivate(xpath, szValue);
        }

        /// <summary>
        ///  保存指定Key的值到指定的配置文件中
        /// </summary>
        /// <param name="xpath">xml Xpath 路径</param>
        /// <param name="szValue">新修改的值</param>
        private bool writeConfigDataPrivate(string xpath, string szValue)
        {
            if (!assertLoaded() || xpath == null || xpath.Trim() == "")
            {
                return false;
            }
            string szConfigFile = string.Format("{0}\\{1}", getWorkDirectory(), CONFIG_FILE);
            string[] keys = (xpath.Trim()).Split(new string[] { "/" }, StringSplitOptions.None);
            string szXPath = string.Format(".//Config[@name='{0}']", keys[0]);
            XmlNode clsXmlNode = selectXmlNode(clsXmlDoc, szXPath);
            if (clsXmlNode == null)
            {
                clsXmlNode = createXmlNode(clsXmlDoc, "Config");
                setXmlAttr(clsXmlNode, "name", keys[0]);

            }
            clsXmlNode = createNode(clsXmlNode, keys, 1);
            clsXmlNode.InnerText = szValue;

            return saveXmlDocument(clsXmlDoc, szConfigFile);
        }

        #endregion --- 基本操作函数 End ---

        /// <summary>
        /// 递归创建节点
        /// </summary>
        /// <param name="parantNode"></param>
        /// <param name="keys"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private XmlNode createNode(XmlNode parantNode, string[] keys, int i)
        {
            XmlNode clsXmlNode = null;
            if (i < keys.Length)
            {
                clsXmlNode = selectXmlNode(parantNode, keys[i]);
                if (clsXmlNode == null)
                {
                    clsXmlNode = createXmlNode(parantNode, keys[i]);
                    //clsXmlNode.InnerText = szValue;
                }
                if ((i + 1) < keys.Length)
                    clsXmlNode = createNode(clsXmlNode, keys, i + 1);

            }

            return clsXmlNode;

        }

        private bool assertLoaded()
        {
            string szConfigFile = string.Format("{0}\\{1}", getWorkDirectory(), CONFIG_FILE);
            if (configFileLastModifyTime != File.GetLastWriteTime(szConfigFile))
            {
                load();
            }
            if (clsXmlDoc == null)
            {
                return false;
            }
            return true;
        }
    }
}
