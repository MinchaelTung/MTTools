using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;

namespace Eagle.Web.UI {
  /// <summary>
  /// 该类实现aspx页面中，指向aspx页面的超链接转变为指向html页面的超链接。
  /// </summary>
  /// <remarks>
  /// HtmlPanel是一个服务器端控件，用于转换需要静态化的超链接；<br/>
  /// 在aspx页面中，该控件所有超链接子控件将被静态化。
  /// </remarks>
  public class HtmlPanel : Control{
    private bool _isHtml = true;
    private bool _includeSubControls = true;
  
    /// <summary>
    /// 获取或设置一个bool值，指示是否允许静态化。
    /// </summary>
    public bool IsHtml {
      get {
        return _isHtml;
      }
      set {
        _isHtml = value;
      }
    }

    /// <summary>
    /// 获取或设置一个bool值，指示静态化时是否包含子控件。
    /// </summary>
    public bool IncludeSubControls {
      get {
        return _includeSubControls;
      }
      set {
        _includeSubControls = value;
      }
    }
  
    protected override void OnPreRender(EventArgs e) {
      if (IsHtml) {
        ParseLink();
      }

      base.OnPreRender(e);
    }

    /// <summary>
    /// 执行超链接aspx到html的转变，可被子类重写。
    /// </summary>
    protected virtual void ParseLink() {
      Parse(this);
    }

    private void Parse(Control ctr) {
      ControlCollection children = ctr.Controls;

      foreach (Control item in children) {
        if (item is HyperLink) {
          HyperLink link = (HyperLink)item;
          link.NavigateUrl = UrlMapping.AspxToHtml(base.ResolveClientUrl(link.NavigateUrl));
        } else if (item is HtmlAnchor) {
          HtmlAnchor link = (HtmlAnchor)item;
          link.HRef = UrlMapping.AspxToHtml(base.ResolveClientUrl(link.HRef));
        } else {
          if (IncludeSubControls && item.Controls.Count > 0) {
            Parse(item);
          }
        }
      }
    }
  }
}
