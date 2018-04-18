using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Eagle.Web.UI {
	public class MenuItem : HyperLink{
		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			base.Render(writer);
			writer.RenderEndTag();
		}
	}
}
