using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Eagle.Web.UI {

	public class MenuHyperLink : HyperLink{
		/// <summary>
		/// 当前页，超链接样式
		/// </summary>
		public string CurrentCssClass {
			get {
				object o = ViewState["CurrentCssClass"];
				return (o == null) ? String.Empty : (string)o;
			}
			set {
				ViewState["CurrentCssClass"] = value;
			}
		}

		/// <summary>
		/// 是否为当前页
		/// </summary>
		protected virtual bool IsCurrentPage {
			get {
				string currentUrl = Page.Request.RawUrl;
				string linkUrl = this.ResolveUrl(this.NavigateUrl);

				return linkUrl.StartsWith(currentUrl);
			}
		}

		//protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer) {
		//  if (IsCurrentPage) {
		//    string cls = CurrentCssClass;
		//    if (!String.IsNullOrEmpty(this.CssClass)) {
		//      cls = cls + " " + this.CssClass;
		//    }

		//    this.CssClass = cls;
		//  }

		//  base.AddAttributesToRender(writer);
		//}


		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
			if (IsCurrentPage) {
				//string cls = CurrentCssClass;
				//if (!String.IsNullOrEmpty(this.CssClass)) {
				//  cls = cls + " " + this.CssClass;
				//}

				//this.CssClass = cls;

				writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentCssClass);
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			base.Render(writer);
			writer.RenderEndTag();
		}
	}
}
