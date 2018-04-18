using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MTFramework.Web.Controls
{
    /// <summary>
    /// 菜单对象
    /// </summary>
    [Serializable]
    public class Memu
    {
        private string _Title = string.Empty;
        /// <summary>
        /// 主菜单标题
        /// </summary>
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }
        private string _Authority = "*";
        /// <summary>
        /// 权限*号没限制，多个权限使用"|"分隔
        /// </summary>
        public string Authority
        {
            get
            {
                return this._Authority;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "*";
                this._Authority = value;
            }
        }
        private List<MemuItem> _Items = new List<MemuItem>();
        /// <summary>
        /// 菜单集合
        /// </summary>
        public List<MemuItem> Items
        {
            get
            {
                return this._Items;
            }
            set
            {
                this._Items = value;
            }
        }

        public static bool Save(List<Memu> memuList, string path)
        {
            try
            {
                if (File.Exists(path) == true)
                {
                    File.Delete(path);
                }

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
                {

                    XmlSerializer xml = new XmlSerializer(typeof(List<Memu>));
                    xml.Serialize(fs, memuList);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<Memu> Load(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<Memu>));
                    return (List<Memu>)xml.Deserialize(fs);
                }
            }
            catch
            {
                return new List<Memu>();
            }
        }

    }
}
