using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI.Adapters;
namespace MTFramework.Web.Controls
{
    /// <summary>
    /// 手风琴菜单
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MemuAccordion runat=\"server\"> </{0}:MemuAccordion>")]
    public class MemuAccordion : Control
    {
        #region --- 字段 Begin ---
        /// <summary>
        /// 控件关联的样式类
        /// </summary>
        private string _CssClass = string.Empty;
        /// <summary>
        /// 菜单配置Xml文件
        /// </summary>
        private string _MemuConfigFileUrl = string.Empty;
        /// <summary>
        /// 高度
        /// </summary>
        private int _ImgHeight = 18;
        /// <summary>
        /// 宽度
        /// </summary>
        private int _ImgWidth = 18;
        /// <summary>
        /// 行间距
        /// </summary>
        private int _LineSpacing = 30;
        /// <summary>
        /// 文字大小
        /// </summary>
        private int _FontSize = 14;
        /// <summary>
        /// 获取或设置控当前路径菜单样式
        /// </summary>
        private string _CurrentCssClass = string.Empty;
        /// <summary>
        /// 菜单信息
        /// </summary>
        private List<Memu> _Memus = new List<Memu>();
        /// <summary>
        /// 权限SessionName
        /// </summary>
        private string _AuthoritySessionName;

        /// <summary>
        /// 当前页面的样式
        /// </summary>
        private string _CurrentBackgroundColor = "#CAECFF";
        /// <summary>
        /// 焦点背景颜色
        /// </summary>
        private string _FocusBackgroundColor = "#CAECFF";
        /// <summary>
        /// 主菜单分类背景颜色
        /// </summary>
        private string _MemuRootBackgroundColor = "#77CDFF";
        /// <summary>
        /// 主菜单背景颜色
        /// </summary>
        private string _MemuBackgroundColor = "transparent";
        /// <summary>
        /// 菜单字体颜色
        /// </summary>
        private string _FontColor = "#000000";
        /// <summary>
        /// 超连接菜单颜色
        /// </summary>
        private string _FontLinkColor = "#000000";
        /// <summary>
        /// 超连接菜单焦点时颜色
        /// </summary>
        private string _FontLinkFocusColor = "#000000";
        #endregion --- 字段 End ---

        /// <summary>
        /// 获取或设置控件关联的样式类
        /// </summary>
        [Category("Behavior")]
        [Description("Css的样式类名称")]
        [DefaultValue("")]
        [CssClassProperty]
        public string CssClass
        {
            get
            {
                return _CssClass;
            }
            set
            {
                _CssClass = value;
            }
        }

        /// <summary>
        /// 获取或设置控当前路径菜单样式
        /// </summary>
        [Category("Behavior")]
        [Description("当前焦点菜单样式类名称")]
        [DefaultValue("")]
        [CssClassProperty]
        public string CurrentCssClass
        {
            get
            {
                return this._CurrentCssClass;
            }
            set
            {
                this._CurrentCssClass = value;
            }
        }

        /// <summary>
        /// 主菜单分类背景颜色
        /// </summary>
        [Category("Behavior")]
        [Description("主菜单分类背景颜色")]
        [DefaultValue("#77CDFF")]
        public string MemuRootBackgroundColor
        {
            get
            {
                return this._MemuRootBackgroundColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "#77CDFF";
                this._MemuRootBackgroundColor = value;
            }
        }

        /// <summary>
        /// 主菜单背景颜色
        /// </summary>
        [Category("Behavior")]
        [Description("主菜单分类背景颜色")]
        [DefaultValue("transparent")]
        public string MemuBackgroundColor
        {
            get
            {
                return this._MemuBackgroundColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "transparent";
                this._MemuBackgroundColor = value;
            }
        }

        /// <summary>
        /// 当前页面菜单项的背景颜色
        /// </summary>
        [Category("Behavior")]
        [Description("当前连接菜单项背景颜色")]
        [DefaultValue("#CAECFF")]
        public string CurrentBackgroundColor
        {
            get
            {
                return this._CurrentBackgroundColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "#CAECFF";
                this._CurrentBackgroundColor = value;
            }
        }

        /// <summary>
        /// 焦点菜单项背景颜色
        /// </summary>
        [Category("Behavior")]
        [Description("当前焦点菜单项背景颜色")]
        [DefaultValue("#CAECFF")]
        public string FocusBackgroundColor
        {
            get
            {
                return this._FocusBackgroundColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "#CAECFF";
                this._FocusBackgroundColor = value;
            }
        }

        /// <summary>
        /// 菜单字体颜色
        /// </summary>
        [Category("Behavior")]
        [Description("菜单字体颜色")]
        [DefaultValue("#000000")]
        public string FontColor
        {
            get
            {
                return this._FontColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "#000000";
                this._FontColor = value;
            }
        }

        /// <summary>
        /// 超连接菜单颜色
        /// </summary>
        [Category("Behavior")]
        [Description("超连接菜单颜色")]
        [DefaultValue("#000000")]
        public string FontLinkColor
        {
            get
            {
                return this._FontLinkColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "#000000";
                this._FontLinkColor = value;
            }
        }

        /// <summary>
        /// 超连接菜单焦点时颜色
        /// </summary>
        [Category("Behavior")]
        [Description("超连接菜单焦点时颜色")]
        [DefaultValue("#000000")]
        public string FontLinkFocusColor
        {
            get
            {
                return this._FontLinkFocusColor;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true) value = "#000000";
                this._FontLinkFocusColor = value;
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [Category("Behavior")]
        [Description("图标宽度")]
        [DefaultValue(20)]
        public int ImgWidth
        {
            get
            {
                return this._ImgWidth;
            }
            set
            {
                this._ImgWidth = value;
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        [Category("Behavior")]
        [Description("图标高度")]
        [DefaultValue(20)]
        public int ImgHeight
        {
            get
            {
                return this._ImgHeight;
            }
            set
            {
                this._ImgHeight = value;
            }
        }

        /// <summary>
        /// 行间距
        /// </summary>
        [Category("Behavior")]
        [Description("行间距")]
        [DefaultValue(30)]
        public int LineSpacing
        {
            get
            {
                return this._LineSpacing;
            }
            set
            {
                this._LineSpacing = value;
            }
        }

        /// <summary>
        /// 文字大小
        /// </summary>
        [Category("Behavior")]
        [Description("文字大小")]
        [DefaultValue(14)]
        public int FontSize
        {
            get
            {
                return this._FontSize;
            }
            set
            {
                this._FontSize = value;
            }
        }

        /// <summary>
        /// 菜单配置Xml文件
        /// </summary>
        [Category("Behavior")]
        [Description("菜单配置Xml文件相对路径")]
        [DefaultValue("")]
        [UrlProperty]
        public string MemuConfigFileUrl
        {
            get
            {
                return this._MemuConfigFileUrl;
            }
            set
            {
                this._MemuConfigFileUrl = value;
            }
        }

        /// <summary>
        /// 权限SessionName Value必须是String[]对象 该Session不存在则权限为*
        /// </summary>
        [Category("Behavior")]
        [Description("权限角色的会话名称")]
        [DefaultValue("MemuAccordionAuthoritys")]
        public string AuthoritySessionName
        {
            get
            {
                return this._AuthoritySessionName;
            }
            set
            {
                this._AuthoritySessionName = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示服务器控件是否向发出请求的客户端保持自己的视图状态以及它所包含的任何子控件的视图状态。当前控件设定为True
        /// </summary>
        public override bool EnableViewState
        {
            get
            {
                return true;
            }
            set
            {
                base.EnableViewState = true;
            }
        }

        /// <summary>
        /// 将服务器控件内容发送到提供的 System.Web.UI.HtmlTextWriter 对象，此对象编写将在客户端呈现的内容。
        /// </summary>
        /// <param name="writer">接收服务器控件内容的 System.Web.UI.HtmlTextWriter 对象。</param>
        protected override void Render(HtmlTextWriter writer)
        {

            string[] authoritys = null;

            try
            {
                if (string.IsNullOrEmpty(this._MemuConfigFileUrl) == true)
                {
                    return;
                }

                if (Page.Session[this.AuthoritySessionName] != null)
                {
                    authoritys = Page.Session[this.AuthoritySessionName] as string[];
                }
                else
                {
                    authoritys = new string[] { "*" };
                }

                string path = Page.Server.MapPath(this.MemuConfigFileUrl.Replace("~", ""));
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<Memu>));
                    this._Memus = (List<Memu>)xml.Deserialize(fs);
                }
            }
            catch
            {
                return;
            }


            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            if (!String.IsNullOrEmpty(_CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
            }
            else
            {
                writer.WriteLine("<style type='text/css'>");
                writer.WriteLine("#" + this.ClientID + " {width: auto; font-size: " + this.FontSize + "px; color: " + this.FontColor + "; }");
                writer.WriteLine("#" + this.ClientID + " dt{width: auto; text-align: center; height: " + this.LineSpacing + "px; line-height: " + this.LineSpacing + "px; font-weight: bold; font-size: 130%; background-color: " + this.MemuRootBackgroundColor + "; float:inherit; }");
                writer.WriteLine("#" + this.ClientID + " dd{width: auto; display:none; text-align: left; margin-left: -40px; background-color:" + this.MemuBackgroundColor + "; }");
                writer.WriteLine("#" + this.ClientID + " li{width: auto; height: " + this.LineSpacing + "px; line-height: " + this.LineSpacing + "px; text-align: left; list-style-type: none; }");
                writer.WriteLine("#" + this.ClientID + " img{width: " + this._ImgWidth + "px; height: " + this.ImgHeight + "px; border: 0px solid #FFFFFF; }");
                writer.WriteLine("#" + this.ClientID + " a, #" + this.ClientID + " a:visited{display: block; padding: 0px 5px; border: 0px; text-decoration: none; color: " + this.FontLinkColor + "; }");
                writer.WriteLine("#" + this.ClientID + " a:hover, #" + this.ClientID + " a:active{padding: 0px 5px; border: 0px; color: " + this.FontLinkFocusColor + "; text-decoration: none; }");
                if (string.IsNullOrEmpty(this.CurrentCssClass) == true)
                {
                    writer.WriteLine("#" + this.ClientID + " .CurrentMemu{background-color: " + this.CurrentBackgroundColor + "; }");
                }
                writer.WriteLine("</style>");
            }
            writer.WriteLine("<script type='text/javascript'>");
            writer.WriteLine("    var memuAccordionSelectobj = null");
            writer.WriteLine("    function MemuShowHide(objID) {");
            writer.WriteLine("        if(memuAccordionSelectobj != null){");
            writer.WriteLine("             memuAccordionSelectobj.style.display = 'none' ");
            writer.WriteLine("        }");
            writer.WriteLine("        var obj = document.getElementById(objID);");
            writer.WriteLine("        if (obj.style.display == 'block') {");
            writer.WriteLine("            obj.style.display = 'none';");
            writer.WriteLine("        } else {");
            writer.WriteLine("            obj.style.display = 'block';");
            writer.WriteLine("            memuAccordionSelectobj = obj;");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("    function MemuOnMouseOver(obj) {");
            writer.WriteLine("        obj.style.backgroundColor = '{0}';", this.FocusBackgroundColor);
            writer.WriteLine("    }");
            writer.WriteLine("    function MemuOnMouseOut(obj) {");
            writer.WriteLine("       obj.style.backgroundColor = '';");
            writer.WriteLine("    }");
            writer.WriteLine("</script>");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.WriteLine();
            /*
             *  /Default.aspx
             *  this.Page.Request.Url.AbsolutePath;
             *  http://localhost:1732/Default.aspx
             *  this.Page.Request.Url.AbsoluteUri;
             *  请求参数
             *  this.Page.Request.Url.Query;
             */
            int currentMemuNo = 0;
            bool bl = false;
            for (int i = 0; i < this._Memus.Count; i++)
            {
                if (this.validationAuthority(this._Memus[i].Authority, authoritys) == false)
                {
                    continue;
                }
                writer.WriteLine(string.Format("<dt  onclick=\"MemuShowHide('item_{0}')\">{1}</dt>", i, this._Memus[i].Title));
                writer.WriteLine(string.Format("<dd id=\"item_{0}\">", i));

                foreach (MemuItem item in this._Memus[i].Items)
                {
                    if (this.validationAuthority(item.Authority, authoritys) == false)
                    {
                        continue;
                    }

                    if (item.NavigateUrl.Equals(this.Page.Request.Url.AbsolutePath, StringComparison.InvariantCultureIgnoreCase) == true
                        || item.NavigateUrl.Equals("~" + this.Page.Request.Url.AbsolutePath, StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        if (string.IsNullOrEmpty(this.CurrentCssClass) == true)
                        {
                            writer.WriteLine("<li class=\"CurrentMemu\">");
                        }
                        else
                        {
                            writer.WriteLine("<li class=\"" + this.CurrentCssClass + "\">");
                        }
                        currentMemuNo = i;
                        bl = true;
                    }
                    else
                    {
                        writer.WriteLine("<li onmouseover=\"MemuOnMouseOver(this)\" onmouseout=\"MemuOnMouseOut(this)\">");
                    }
                    writer.WriteLine(this.fetchMemuItem(item));
                    writer.WriteLine("</li>");
                }
                writer.WriteLine("</dd>");
            }
            writer.WriteLine("<script type='text/javascript'>");
            writer.WriteLine("    MemuShowHide('item_{0}');", currentMemuNo);
            if (bl == false)
            {
                writer.WriteLine("    window.history.go(-1);");
            }
            writer.WriteLine("</script>");

            writer.RenderEndTag();

        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="memuAuthorityStr">菜单权限</param>
        /// <param name="authoritys">用户权限</param>
        /// <returns>True=可以通过,False=不可以通过</returns>
        private bool validationAuthority(string memuAuthorityStr, string[] authoritys)
        {
            bool isAuthority = false;
            string[] memuAuthoritys = memuAuthorityStr.Split('|');
            foreach (string memuAuthority in memuAuthoritys)
            {
                isAuthority = memuAuthority.Equals("*");
                for (int i = 0; i < authoritys.Length && isAuthority == false; i++)
                {
                    isAuthority = memuAuthority.Equals(authoritys[i], StringComparison.InvariantCultureIgnoreCase);
                }
                if (isAuthority == true)
                {
                    break;
                }
            }

            return isAuthority;
        }

        /// <summary>
        /// 填充子菜单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string fetchMemuItem(MemuItem item)
        {
            string data = string.Empty;
            if (string.IsNullOrEmpty(item.Title) == true)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(item.ImageUrl) == false)
            {
                data = string.Format("<img src=\"{0}\" alt=\"\" />", item.ImageUrl);
            }
            data += item.Title;
            if (string.IsNullOrEmpty(item.NavigateUrl) == false)
            {
                data = this.fetchMemuItemNavigateUrl(data, item.NavigateUrl);
            }

            return data;
        }
        /// <summary>
        /// 填充连接
        /// </summary>
        /// <param name="str"></param>
        /// <param name="navigateUrl"></param>
        /// <returns></returns>
        private string fetchMemuItemNavigateUrl(string str, string navigateUrl)
        {
            string link = "";
            if (navigateUrl.IndexOf("/") == 0)
            {
                link = string.Format("<a href=\"{0}\">{1}</a>", this.Page.Request.Url.Scheme + "://" + this.Page.Request.Url.Authority + navigateUrl, str);
            }
            else if (navigateUrl.IndexOf("~") == 0)
            {
                link = string.Format("<a href=\"{0}\">{1}</a>", this.Page.Request.Url.Scheme + "://" + this.Page.Request.Url.Authority + navigateUrl.Replace("~", ""), str);
            }
            else
            {
                link = string.Format("<a href=\"{0}\">{1}</a>", navigateUrl, str);
            }

            return link;
        }

    }
}
