using System;

namespace MTFramework.Web.Controls
{
    /// <summary>
    /// 菜单子项
    /// </summary>
    [Serializable]
    public class MemuItem
    {
        private string _Title = string.Empty;
        /// <summary>
        /// 显示标题
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
        private string _NavigateUrl = string.Empty;
        /// <summary>
        /// 导航路径
        /// </summary>
        public string NavigateUrl
        {
            get
            {
                return this._NavigateUrl;
            }
            set
            {
                this._NavigateUrl = value;
            }
        }
        private string _ImageUrl = string.Empty;
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return this._ImageUrl;
            }
            set
            {
                this._ImageUrl = value;
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
    }
}
