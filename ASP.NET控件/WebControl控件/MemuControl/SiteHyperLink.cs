using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MTFramework.Web.Controls
{

    public class SiteHyperLink : HyperLink
    {
        /// <summary>
        /// 当前页，超链接样式
        /// </summary>      
        public string CurrentCssClass
        {
            get
            {
                object o = ViewState["CurrentCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["CurrentCssClass"] = value;
            }
        }

        /// <summary>
        /// 是否为当前页
        /// </summary>
        protected virtual bool IsCurrentPage
        {
            get
            {
                string currentUrl = Page.Request.RawUrl;
                string linkUrl = this.ResolveUrl(this.NavigateUrl);

                return currentUrl.StartsWith(linkUrl, StringComparison.OrdinalIgnoreCase);
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (IsCurrentPage)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentCssClass);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            base.Render(writer);
            writer.RenderEndTag();
        }
    }

}
