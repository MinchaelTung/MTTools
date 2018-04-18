using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace Eagle.Article.Web.Controls {
  /// <summary>
  /// �򵥵ķ�ҳ��ʾ�ؼ���
  /// </summary>
  /// <example>
  /// <![CDATA[
  ///   <ctr:SimplePagger Id="MyPagger" runat="server" PageSize="10" NumberCount="15"/>
  ///   ....
  ///   MyPagger.VirtualCount = 50;
  /// ]]>
  /// </example>
  [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.InheritanceDemand,Level = AspNetHostingPermissionLevel.Minimal)]
  [DefaultProperty("Text")]
  [ToolboxData("<{0}:SimplePager runat=\"server\"> </{0}:SimplePager>")]
  public class SimplePager : Control {
    private static string KEY_PAGE = "page";
    private static readonly Regex RX = new Regex(@"&page=\d+", RegexOptions.Compiled);
    
    #region ���Ʒ�ҳ������������
    private string _class;
    private int _pageSize = 10;
    private int _numberCount = 10;
    private int _virtualCount = 0;

    private string _prevText = "��һҳ";
    private string _nextText = "��һҳ";
    private string _firstText = "��һҳ";
    private string _lastText = "��ĩҳ";

    /// <summary>
    /// ��ȡ�����ÿؼ���������ʽ��
    /// </summary>
    [Category("Behavior")]
    [Description("Css����ʽ������")]
    public string Class {
      get {
        return _class;
      }
      set {
        _class = value;
      }
    }

    /// <summary>
    /// ��ȡ�����á���һҳ���ڷ�ҳ����������ʾ���ı���Ĭ��ֵ����һҳ��
    /// </summary>
    [Category("Behavior")]
    [Description("��һҳ�ı�")]
    public string PrevText {
      get {
        return _prevText;
      }
      set {
        _prevText = value;
      }
    }

    /// <summary>
    /// ��ȡ�����á���һҳ���ڷ�ҳ����������ʾ���ı���Ĭ��ֵ����һҳ��
    /// </summary>
    [Category("Behavior")]
    [Description("��һҳ�ı�")]
    public string NextText {
      get {
        return _nextText;
      }
      set {
        _nextText = value;
      }
    }

    /// <summary>
    /// ��ȡ�����á���һҳ���ڷ�ҳ����������ʾ���ı���Ĭ��ֵ����һҳ��
    /// </summary>
    [Category("Behavior")]
    [Description("��һҳ�ı�")]
    public string FirstText {
      get {
        return _firstText;
      }
      set {
        _firstText = value;
      }
    }

    /// <summary>
    /// ��ȡ�����á���ĩҳ���ڷ�ҳ����������ʾ���ı���Ĭ��ֵ����ĩҳ��
    /// </summary>
    [Category("Behavior")]
    [Description("��ĩҳ�ı�")]
    public string LastText {
      get {
        return _lastText;
      }
      set {
        _lastText = value;
      }
    }
    
    /// <summary>
    /// ��ȡ�����÷�ҳ�Ĵ�С��Ĭ��ֵ10
    /// </summary>
    [Category("Behavior")]
    [Description("ҳ��С")]
    public int PageSize {
      get {
        return _pageSize;
      }
      set {
        _pageSize = value;
      }
    }

    /// <summary>
    /// ��ȡ�����÷�ҳ����������ʾ��ҳ��������Ĭ��10
    /// </summary>
    [Category("Behavior")]
    [Description("��ҳ��Ҫ��ʾ��ҳ������")]
    public int NumberCount {
      get {
        return _numberCount;
      }
      set {
        _numberCount = value;
      }
    }

    /// <summary>
    /// ��ȡ�����ò�ѯ�õ����ܼ�¼��
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
    /// ��ȡ��ҳ��
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
    /// ��ȡ��ǰҳ��
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
      //writer.Write("Hello World:");
      // Prepare the query string
      string query = "";
      if (Context != null) {
        query = Page.Request.Url.Query.Replace('?', '&');
      }

      query = RX.Replace(query, String.Empty,-1);
      query = "<li><a href='?page={0}" + query + "'>{1}</a></li>";

      // Prepare the necessary number
      int page = CurrentPage;
      int count = PageCount;
      int nums = NumberCount-1;
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
      writer.RenderBeginTag(HtmlTextWriterTag.Ul);

      writer.Write(String.Format(query, 1, FirstText));
      writer.Write(String.Format(query, page>1?(page-1):1, PrevText));
      for (int i = beginIndex; i <= endIndex; i++) {
        if (page == i) {
			writer.Write("<li>");
          writer.Write(i);
		  writer.Write("</li>");
        } else {
          writer.Write(String.Format(query, i, i));
        }
      }
      writer.Write(String.Format(query, page < count ? (page + 1) : page, NextText));
      writer.Write(String.Format(query, count, LastText));
      writer.Write(String.Format("<li>{0}&nbsp;/&nbsp;{1}</li>", page, count));

      writer.RenderEndTag();
    }
  }
}
