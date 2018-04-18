using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.MemuInfo
{
    /// <summary>
    /// 手风琴式菜单
    /// </summary>
    public class SimpleMemuAccordion
    {
        /// <summary>
        /// 客户端控件ID
        /// </summary>
        public string ClientStaticID { get; set; }

        /// <summary>
        /// 菜单样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 菜单内容
        /// </summary>
        public List<Memu> MemuInfoList { get; set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        public string[] UserAuthorityArray { get; set; }

        /// <summary>
        /// 宽度 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 手风琴式菜单构造函数
        /// </summary>
        /// <param name="_ClientStaticID">控件ID</param>        
        /// <param name="_MemuInfoList">菜单</param>
        /// <param name="_UserAuthorityArray">用户访问权限列表</param>
        /// <param name="_Width">宽度</param>
        /// <param name="_CssClass">样式</param>
        public SimpleMemuAccordion(string _ClientStaticID, List<Memu> _MemuInfoList = null, string[] _UserAuthorityArray = null, int _Width = 200, string _CssClass = "")
        {
            this.ClientStaticID = _ClientStaticID;
            this.Width = _Width;
            this.CssClass = _CssClass;
            this.MemuInfoList = _MemuInfoList;
            this.UserAuthorityArray = _UserAuthorityArray;
        }

        #region --- JS CSS Begin ---

        /// <summary>
        /// 创建JS
        /// </summary>
        /// <returns></returns>
        private string getSimpleMemuAccordionScriptString()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + this.ClientStaticID + "Script.js") == false)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("function SimpleMemuAccordionFun() {");
                sb.AppendLine("if (!document.getElementById || !document.getElementsByTagName) { return false; }");
                sb.AppendFormat("this.menu = document.getElementById(\"{0}\");", this.ClientStaticID);
                sb.AppendLine();
                sb.AppendLine("this.submenus = this.menu.getElementsByTagName(\"div\");");
                sb.AppendLine("this.remember = true;");
                sb.AppendLine("this.speed = 1;");
                sb.AppendLine("this.markCurrent = true;");
                sb.AppendLine("this.oneSmOnly = false;");
                sb.AppendLine("}");
                sb.AppendLine("SimpleMemuAccordionFun.prototype.Init = function () {");
                sb.AppendLine("var mainInstance = this;");
                sb.AppendLine("for (var i = 0; i < this.submenus.length; i++) {");
                sb.AppendLine("this.submenus[i].getElementsByTagName(\"span\")[0].onclick = function () {");
                sb.AppendLine("mainInstance.ToggleMenu(this.parentNode);");
                sb.AppendLine("};");
                sb.AppendLine("}");
                sb.AppendLine("if (this.markCurrent) {");
                sb.AppendLine("var links = this.menu.getElementsByTagName(\"a\");");
                sb.AppendLine("for (var i = 0; i < links.length; i++)");
                sb.AppendLine("if (links[i].href == document.location.href) {");
                sb.AppendLine("links[i].className = \"current\";");
                sb.AppendLine("break;");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("if (this.remember) {");
                sb.AppendLine("var regex = new RegExp(\"smaf_\" + encodeURIComponent(this.menu.id) + \"=([01]+)\");");
                sb.AppendLine("var match = regex.exec(document.cookie);");
                sb.AppendLine("if (match) {");
                sb.AppendLine("var states = match[1].split(\"\");");
                sb.AppendLine("for (var i = 0; i < states.length; i++) {");
                sb.AppendLine("this.submenus[i].className = (states[i] == 0 ? \"collapsed\" : \"\");");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("};");
                sb.AppendLine("SimpleMemuAccordionFun.prototype.ToggleMenu = function (submenu) {");
                sb.AppendLine("if (submenu.className == \"collapsed\") {");
                sb.AppendLine("this.Expand_Menu(submenu);");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("this.Collapse_Menu(submenu);");
                sb.AppendLine("}");
                sb.AppendLine("};");
                sb.AppendLine("SimpleMemuAccordionFun.prototype.Expand_Menu = function (submenu) {");
                sb.AppendLine("var fullHeight = submenu.getElementsByTagName(\"span\")[0].offsetHeight;");
                sb.AppendLine("var links = submenu.getElementsByTagName(\"a\");");
                sb.AppendLine("for (var i = 0; i < links.length; i++) {");
                sb.AppendLine("fullHeight += links[i].offsetHeight;");
                sb.AppendLine("}");
                sb.AppendLine("var moveBy = Math.round(this.speed * links.length);");
                sb.AppendLine("var mainInstance = this;");
                sb.AppendLine("var intId = setInterval(function () {");
                sb.AppendLine("var curHeight = submenu.offsetHeight;");
                sb.AppendLine("var newHeight = curHeight + moveBy;");
                sb.AppendLine("if (newHeight < fullHeight) {");
                sb.AppendLine("submenu.style.height = newHeight + \"px\";");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("clearInterval(intId);");
                sb.AppendLine("submenu.style.height = \"\";");
                sb.AppendLine("submenu.className = \"\";");
                sb.AppendLine("mainInstance.Memorize_Status();");
                sb.AppendLine("}");
                sb.AppendLine("}, 1);");
                sb.AppendLine("this.Collapse_Others(submenu);");
                sb.AppendLine("};");
                sb.AppendLine("SimpleMemuAccordionFun.prototype.Collapse_Menu = function (submenu) {");
                sb.AppendLine("var minHeight = submenu.getElementsByTagName(\"span\")[0].offsetHeight;");
                sb.AppendLine("var moveBy = Math.round(this.speed * submenu.getElementsByTagName(\"a\").length);");
                sb.AppendLine("var mainInstance = this;");
                sb.AppendLine("var intId = setInterval(function () {");
                sb.AppendLine("var curHeight = submenu.offsetHeight;");
                sb.AppendLine("var newHeight = curHeight - moveBy;");
                sb.AppendLine("if (newHeight > minHeight) {");
                sb.AppendLine("submenu.style.height = newHeight + \"px\";");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("clearInterval(intId);");
                sb.AppendLine("submenu.style.height = \"\";");
                sb.AppendLine("submenu.className = \"collapsed\";");
                sb.AppendLine("mainInstance.Memorize_Status();");
                sb.AppendLine("}");
                sb.AppendLine("}, 1);");
                sb.AppendLine("};");
                sb.AppendLine("SimpleMemuAccordionFun.prototype.Collapse_Others = function (submenu) {");
                sb.AppendLine("if (this.oneSmOnly) {");
                sb.AppendLine(" for (var i = 0; i < this.submenus.length; i++) {");
                sb.AppendLine("if (this.submenus[i] != submenu && this.submenus[i].className != \"collapsed\") {");
                sb.AppendLine("this.Collapse_Menu(this.submenus[i]);");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("};");
                sb.AppendLine("SimpleMemuAccordionFun.prototype.Memorize_Status = function () {");
                sb.AppendLine("if (this.remember) {");
                sb.AppendLine("var states = new Array();");
                sb.AppendLine("for (var i = 0; i < this.submenus.length; i++) {");
                sb.AppendLine("states.push(this.submenus[i].className == \"collapsed\" ? 0 : 1);");
                sb.AppendLine("}");
                sb.AppendLine("var d = new Date();");
                sb.AppendLine("d.setTime(d.getTime() + (24 * 60 * 60 * 1000));");
                sb.AppendLine("document.cookie = \"smaf_\" + encodeURIComponent(this.menu.id) + \"=\" + states.join(\"\") + \"; expires=\" + d.toGMTString() + \"; path=/\";");
                sb.AppendLine("}");
                sb.AppendLine("};");
                try
                {
                    using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + this.ClientStaticID + "Script.js", FileMode.Create))
                    {
                        byte[] buf = Encoding.UTF8.GetBytes(sb.ToString());
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
            StringBuilder js = new StringBuilder();

            js.AppendFormat("<script src=\"{0}://{1}/{2}Script.js\"></script>", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, this.ClientStaticID);

            return js.ToString();
        }

        /// <summary>
        /// 默认CSS
        /// </summary>
        /// <returns></returns>
        private string getCSSString()
        {
            StringBuilder css = new StringBuilder();

            css.AppendLine("<style type=\"text/css\">");

            css.AppendFormat("#{0}", this.ClientStaticID);
            css.AppendLine("{ width: " + this.Width + "px; background: rgba(255, 255, 255, 0.00); height: 100%; float: left; }");


            css.AppendFormat("#{0} div.{1} ", this.ClientStaticID, this.CssClass);
            css.AppendLine("{ height: 25px; }");

            css.AppendFormat("#{0} div.collapsed ", this.ClientStaticID);
            css.AppendLine("{ height: 25px; }");

            css.AppendFormat("#{0} div.{1} ", this.ClientStaticID, this.CssClass);
            css.AppendLine("{ overflow: hidden; }");

            css.AppendFormat("#{0} div ", this.ClientStaticID);
            css.AppendLine("{ overflow: hidden; }");

            css.AppendFormat("#{0} span", this.ClientStaticID);
            css.AppendLine("{ background: #a9e2f6; border: 1px solid; border-left: 6px solid; border-color: #8ED6E3; width: 100%; height: 23px; display: block; line-height: 23px; padding-left: 20px;  }");

            css.AppendFormat("#{0} a", this.ClientStaticID);
            css.AppendLine("{ padding: 4px 0px 4px 20px; display: block; color: #636363; text-decoration: none; }");

            css.AppendLine("</style>");

            return css.ToString();
        }

        #endregion --- JS CSS End ---

        /// <summary>
        /// Html 数据
        /// </summary>
        /// <returns></returns>
        public string GetHtmlString()
        {
            if (MemuInfoList == null || MemuInfoList.Count == 0 || string.IsNullOrWhiteSpace(this.ClientStaticID) == true)
            {
                return string.Empty;
            }
            if (UserAuthorityArray == null || UserAuthorityArray.Length == 0)
            {
                UserAuthorityArray = new[] { "*" };
            }
            StringBuilder html = new StringBuilder();
            string currentPage = HttpContext.Current.Request.Url.AbsolutePath;
            if (currentPage.Equals("/", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                currentPage = "/Home/Index";
            }
            else if (currentPage.Equals("/Home", StringComparison.InvariantCultureIgnoreCase) == true
                || currentPage.Equals("/Home/", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                currentPage = "/Home/Index";
            }
            else if (currentPage.LastIndexOf("/") == currentPage.Length - 1)
            {
                currentPage = currentPage.Substring(0, currentPage.Length - 1);
            }
            if (this.Width < 1)
            {
                this.Width = 200;
            }
            //开始
            html.AppendLine(this.getSimpleMemuAccordionScriptString());
            if (string.IsNullOrWhiteSpace(this.CssClass) == true)
            {
                this.CssClass = "SimpleMemuAccordionStyle";
                html.AppendLine();
                html.AppendLine(this.getCSSString());
                html.AppendLine();
            }
            html.AppendFormat("<div id=\"{0}\" class=\"{1}\">", this.ClientStaticID, this.CssClass);
            html.AppendLine();
            foreach (var itemMemu in this.MemuInfoList)
            {
                if (this.validationAuthority(itemMemu.Authority) == false)
                {
                    continue;
                }

                html.AppendLine("<div>");
                html.AppendFormat("<span>{0}</span>", itemMemu.Title);
                html.AppendLine();

                foreach (var itemMemuItem in itemMemu.Items)
                {
                    if (this.validationAuthority(itemMemuItem.Authority) == false)
                    {
                        continue;
                    }
                    html.AppendFormat("<a href=\"{1}\">{0}</a>", itemMemuItem.Title, itemMemuItem.NavigateUrl);
                    html.AppendLine();
                }
                html.AppendLine("</div>");
            }
            //结束
            html.AppendLine("		</div>");
            html.AppendLine();
            html.AppendLine(this.getSimpleMemuAccordionScriptString());
            html.AppendLine();
            html.AppendLine("<script type=\"text/javascript\"> new SimpleMemuAccordionFun().Init();</script>");
            html.AppendLine();
            return html.ToString();
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="memuAuthority"></param>
        /// <returns></returns>
        private bool validationAuthority(string memuAuthority)
        {
            if (memuAuthority.Contains("*") == false)
            {
                string[] memuAuthorityArray = memuAuthority.Split('|');
                var s = memuAuthorityArray.Intersect(this.UserAuthorityArray).ToArray();
                if (s.Length < 1)
                {
                    return false;
                }
            }

            return true;
        }


    }
}
