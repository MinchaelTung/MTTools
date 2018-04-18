using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eagle.Web;
using Eagle.Web.UI;

namespace Test {
  public partial class _Default : HtmlPage {
    protected void Page_Load(object sender, EventArgs e) {
      //Response.Write(UrlMapping.HtmlToAspx("/Default____a=4___b=hello___c=12.xhtml"));
      //Response.Write("<br/>");
      Response.Write(base.ResolveClientUrl("~/abc/a.aspx?a=1"));

      //object obj = Activator.CreateInstance(AppDomain.CurrentDomain.ActivationContext).Unwrap();
      //Response.Write(obj.ToString());

      pager.VirtualCount = 100;
    }
  }
}
