using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace Eagle.Web.UI {
  [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [DefaultProperty("Class")]
  [ToolboxData("<{0}:HtmlPagger runat=\"server\"></{0}:HtmlPagger>")]
  public class HtmlPagger : Control{

    private static string KEY_PAGE = "page";
    private static readonly Regex RX = new Regex(@"&page=\d+", RegexOptions.Compiled);

    #region 控制分页、导航的属性
    private string _class;
    private int _pageSize = 10;
    private int _numberCount = 10;
    private int _virtualCount = 0;

    private string _prevText = "上一页";
    private string _nextText = "下一页";
    private string _firstText = "第一页";
    private string _lastText = "最末页";

    /// <summary>
    /// 获取或设置控件关联的样式类
    /// </summary>
    [Category("Behavior")]
    [Description("Css的样式类名称")]
    public string Class {
      get {
        return _class;
      }
      set {
        _class = value;
      }
    }

    /// <summary>
    /// 获取或设置“上一页”在分页导航条中显示的文本，默认值“上一页”
    /// </summary>
    [Category("Behavior")]
    [Description("上一页文本")]
    public string PrevText {
      get {
        return _prevText;
      }
      set {
        _prevText = value;
      }
    }

    /// <summary>
    /// 获取或设置“下一页”在分页导航条中显示的文本，默认值“下一页”
    /// </summary>
    [Category("Behavior")]
    [Description("下一页文本")]
    public string NextText {
      get {
        return _nextText;
      }
      set {
        _nextText = value;
      }
    }

    /// <summary>
    /// 获取或设置“第一页”在分页导航条中显示的文本，默认值“第一页”
    /// </summary>
    [Category("Behavior")]
    [Description("第一页文本")]
    public string FirstText {
      get {
        return _firstText;
      }
      set {
        _firstText = value;
      }
    }

    /// <summary>
    /// 获取或设置“最末页”在分页导航条中显示的文本，默认值“最末页”
    /// </summary>
    [Category("Behavior")]
    [Description("最末页文本")]
    public string LastText {
      get {
        return _lastText;
      }
      set {
        _lastText = value;
      }
    }

    /// <summary>
    /// 获取或设置分页的大小，默认值10
    /// </summary>
    [Category("Behavior")]
    [Description("页大小")]
    public int PageSize {
      get {
        return _pageSize;
      }
      set {
        _pageSize = value;
      }
    }

    /// <summary>
    /// 获取或设置分页导航条中显示的页码数量，默认10
    /// </summary>
    [Category("Behavior")]
    [Description("分页中要显示的页码数量")]
    public int NumberCount {
      get {
        return _numberCount;
      }
      set {
        _numberCount = value;
      }
    }

    /// <summary>
    /// 获取或设置查询得到的总记录数
    /// </summary>
    [Browsable(false)]
    public int VirtualCount {
      get {
        return _virtualCount;
      }
      set {
        _virtualCount = value;
      }
    }

    /// <summary>
    /// 获取总页数
    /// </summary>
    [Browsable(false)]
    public int PageCount {
      get {
        if (Context == null)
          return 10;

        int count = (VirtualCount - 1) / PageSize + 1;
        if (count <= 0)
          count = 1;

        return count;
      }
    }
    #endregion

    /// <summary>
    /// 获取当前页码
    /// </summary>
    [Browsable(false)]
    public int CurrentPage {
      get {
        if (Context == null)
          return 1;

        string tempPage = Page.Request.QueryString[KEY_PAGE];
        int _currPage = 1;
        if (!Int32.TryParse(tempPage, out _currPage)) {
          _currPage = 1;
        }
        return _currPage;
      }
    }

    protected override void Render(HtmlTextWriter writer) {
      // Prepare the query string
      string query = "";
      if (Context != null) {
        query = Page.Request.Url.Query.Replace('?', '&');
      }
      
      query = RX.Replace(query, String.Empty, -1);
      string href = base.ResolveClientUrl(Page.Request.Url.AbsolutePath) + "?page={0}" + query;

      query = "<a href='{0}'>[{1}]</a>&nbsp;";

      // Prepare the necessary number
      int page = CurrentPage;
      int count = PageCount;
      int nums = NumberCount - 1;
      int center = nums / 2;
      int beginIndex = 1;

      // Calculate the first page number in the pagger bar
      if (count > nums && page > center) {
        beginIndex = page - center;
        if ((count - beginIndex) <= nums)
          beginIndex = count - nums;
      }

      // Calculate the last page number in the pagger bar
      int endIndex = beginIndex + nums;
      if (endIndex > count)
        endIndex = count;

      // Render the pagger bar

      writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
      if (!String.IsNullOrEmpty(_class)) {
        writer.AddAttribute(HtmlTextWriterAttribute.Class, _class);
      } else {
        writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "18px");
        writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px");
        writer.AddStyleAttribute("word-spacing", "4px");
      }
      writer.RenderBeginTag(HtmlTextWriterTag.Div);
      
      writer.Write(String.Format(query, UrlMapping.AspxToHtml(String.Format(href, 1)), FirstText));
      writer.Write(String.Format(query, UrlMapping.AspxToHtml(String.Format(href, page > 1 ? (page - 1) : 1)), PrevText));
      for (int i = beginIndex; i <= endIndex; i++) {
        if (page == i) {
          writer.Write(i);
          writer.Write("&nbsp;");
        } else {
          writer.Write(String.Format(query, UrlMapping.AspxToHtml(String.Format(href, i)), i));
        }
      }
      writer.Write(String.Format(query, UrlMapping.AspxToHtml(String.Format(href, page < count ? (page + 1) : page)), NextText));
      writer.Write(String.Format(query, UrlMapping.AspxToHtml(String.Format(href, count)), LastText));
      writer.Write(String.Format("&nbsp;{0}&nbsp;/&nbsp;{1}", page, count));

      writer.RenderEndTag();
    }
  }
}
