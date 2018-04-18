using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// 分页插件事例 扩展方法模式
    /// </summary>
    /// <remarks>
    /// 注意命名空间 System.Web.Mvc
    /// 使用静态类
    /// </remarks>
    public static class SimplePagerExtensions
    {
		//注意必须要有 this HtmlHelper htmlHelper 该参数
        public static MvcHtmlString PrintPager(this HtmlHelper htmlHelper, int pageSize, int totalCount)
        {
            SimplePager pager = new SimplePager();
            pager.PageSize = pageSize;
            pager.VirtualCount = totalCount;

            return new MvcHtmlString(pager.GetHtml());
        }
    }
    
    //=====================分页输出内容==========================

    /// <summary>
    /// 分页工具类
    /// </summary>
    public class SimplePager
    {
        private static string KEY_PAGE = "page";
        private static readonly Regex RX = new Regex(@"&page=\d+", RegexOptions.Compiled);

        #region 控制分页、导航的属性
        private string _cssClass = "pager";
        private int _pageSize = 10;
        private int _numberCount = 10;
        private int _virtualCount = 0;

        private string _prevText = "&lt;";
        private string _nextText = "&gt;";
        private string _firstText = "&lt;&lt;";
        private string _lastText = "&gt;&gt;";

        /// <summary>
        /// 获取或设置控件关联的样式类
        /// </summary>
        public string CssClass
        {
            get
            {
                return _cssClass;
            }
            set
            {
                _cssClass = value;
            }
        }

        /// <summary>
        /// 获取或设置“上一页”在分页导航条中显示的文本，默认值“上一页”
        /// </summary>
        public string PrevText
        {
            get
            {
                return _prevText;
            }
            set
            {
                _prevText = value;
            }
        }

        /// <summary>
        /// 获取或设置“下一页”在分页导航条中显示的文本，默认值“下一页”
        /// </summary>
        public string NextText
        {
            get
            {
                return _nextText;
            }
            set
            {
                _nextText = value;
            }
        }

        /// <summary>
        /// 获取或设置“第一页”在分页导航条中显示的文本，默认值“第一页”
        /// </summary>
        public string FirstText
        {
            get
            {
                return _firstText;
            }
            set
            {
                _firstText = value;
            }
        }

        /// <summary>
        /// 获取或设置“最末页”在分页导航条中显示的文本，默认值“最末页”
        /// </summary>
        public string LastText
        {
            get
            {
                return _lastText;
            }
            set
            {
                _lastText = value;
            }
        }

        /// <summary>
        /// 获取或设置分页的大小，默认值10
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 获取或设置分页导航条中显示的页码数量，默认10
        /// </summary>
        public int NumberCount
        {
            get
            {
                return _numberCount;
            }
            set
            {
                _numberCount = value;
            }
        }

        /// <summary>
        /// 获取或设置查询得到的总记录数
        /// </summary>
        public int VirtualCount
        {
            get
            {
                return _virtualCount;
            }
            set
            {
                _virtualCount = value;
            }
        }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (HttpContext.Current == null)
                    return 10;

                if (VirtualCount < 1)
                    return 0;

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
        public int CurrentPage
        {
            get
            {
                if (HttpContext.Current == null)
                    return 1;

                string tempPage = HttpContext.Current.Request.QueryString[KEY_PAGE];
                int _currPage = 1;
                if (!Int32.TryParse(tempPage, out _currPage))
                {
                    _currPage = 1;
                }
                return _currPage;
            }
        }

        public string GetHtml()
        {
            if (PageCount < 1)
                return String.Empty;

            StringBuilder html = new StringBuilder();

            string query = "";
            if (HttpContext.Current != null)
            {
                query = HttpContext.Current.Request.Url.Query.Replace('?', '&');
            }
            query = RX.Replace(query, String.Empty, -1);
            query = "<li><a href='?page={0}" + query + "'>{1}</a></li>";

            // Prepare the necessary number
            int page = CurrentPage;
            int count = PageCount;
            int nums = NumberCount - 1;
            int center = nums / 2;
            int beginIndex = 1;

            // Calculate the first page number in the pagger bar
            if (count > nums && page > center)
            {
                beginIndex = page - center;
                if ((count - beginIndex) <= nums)
                    beginIndex = count - nums;
            }

            // Calculate the last page number in the pagger bar
            int endIndex = beginIndex + nums;
            if (endIndex > count)
                endIndex = count;

            html.AppendFormat("<ul class=\"{0}\">", _cssClass);

            // Records
            html.AppendFormat("<li><span>Total Records : {0}</span></li>", VirtualCount);

            // First Page
            html.AppendFormat(query, 1, FirstText);
            // Previous Page
            html.AppendFormat(query, page > 1 ? (page - 1) : 1, PrevText);
            // Page Loop...
            for (int i = beginIndex; i <= endIndex; i++)
            {
                if (page == i)
                {
                    html.AppendFormat("<li><span>{0}</span></li>", i);
                }
                else
                {
                    html.AppendFormat(query, i, i);
                }
            }
            // Next Page
            html.AppendFormat(query, page < count ? (page + 1) : page, NextText);
            // Last Page
            html.AppendFormat(query, count, LastText);

            // Pager Summary
            html.AppendFormat("<li><span>{0}&nbsp;/&nbsp;{1}</span></li>", page, count);

            html.Append("</ul>");

            return html.ToString();
        }

    }
}