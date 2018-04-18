using System.Collections.Generic;
using System.IO;
using System.Web.Mvc.MemuInfo;
using System.Web.Mvc.Pager;

namespace System.Web.Mvc
{
    public static class MvcExtensions
    {
        /// <summary>
        /// 分页控件
        /// </summary>
        /// <param name="htmlHelper">当前页面类对象</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="cssClassName">样式类名</param>
        /// <param name="pageButtonText">上下页第一页最后一页字符串</param>
        /// <param name="mode">显示上下页第一页最后一页显示模式 1 = 显示第一页 上一页 下一页 最后一页;2 = 上一页 下一页 3 = 第一页 最后一页</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int totalCount, string cssClassName, string[] pageButtonText = null, int mode = 1)
        {
            SimplePager pager = new SimplePager();
            pager.PageSize = pageSize;
            pager.VirtualCount = totalCount;
            pager.CssClass = cssClassName;
            if (pageButtonText != null)
            {
                switch (mode)
                {
                    case 1:
                        if (pageButtonText.Length > 0)
                        {
                            pager.PrevText = pageButtonText[0];
                        }
                        if (pageButtonText.Length > 1)
                        {
                            pager.NextText = pageButtonText[1];
                        }
                        if (pageButtonText.Length > 2)
                        {
                            pager.FirstText = pageButtonText[2];
                        }
                        if (pageButtonText.Length > 3)
                        {
                            pager.LastText = pageButtonText[3];
                        }
                        break;
                    case 2:
                        if (pageButtonText.Length > 0)
                        {
                            pager.PrevText = pageButtonText[0];
                        }
                        if (pageButtonText.Length > 1)
                        {
                            pager.NextText = pageButtonText[1];
                        }
                        break;
                    case 3:
                        if (pageButtonText.Length > 0)
                        {
                            pager.FirstText = pageButtonText[0];
                        }
                        if (pageButtonText.Length > 1)
                        {
                            pager.LastText = pageButtonText[1];
                        }
                        break;
                    default:
                        break;
                }
            }
            return new MvcHtmlString(pager.GetHtmlString());
        }

        /// <summary>
        /// 手风琴式菜单
        /// </summary>
        /// <param name="htmlHelper">当前页面类对象</param>
        /// <param name="clientStaticID">控件ID</param>
        /// <param name="memuXMLPath">菜单路径</param>
        /// <param name="_UserAuthorityArray">用户访问权限列表</param>
        /// <param name="width">宽度</param>
        /// <param name="cssClassName">样式</param>
        /// <returns></returns>
        public static MvcHtmlString MemuAccordion(this HtmlHelper htmlHelper, string memuXMLPath = null, string[] userAuthorityArray = null, int width = 200, string cssClassName = null)
        {
            if (string.IsNullOrWhiteSpace(memuXMLPath) == true)
            {
                memuXMLPath = AppDomain.CurrentDomain.BaseDirectory + "SimpleMemuAccordion.xml";
            }

            if (File.Exists(memuXMLPath) == false)
            {

                List<Memu> memu = new List<Memu>() {
                new Memu(){ Title="首页" , Authority="*" , Items=new List<MemuItem>(){new MemuItem(){ Title="首页", Authority="*", NavigateUrl="/#/"} }},
                new Memu(){ Title="首页" , Authority="*" , Items=new List<MemuItem>(){new MemuItem(){ Title="首页", Authority="*", NavigateUrl="/#/"} }}
                };
                Memu.Save(memu, memuXMLPath);
                return new MvcHtmlString("");
            }

            string clientStaticID = "SimpleMemuAccordion265342390D5A4BFFB298E4CCD934BCBE";

            dynamic memuList = Memu.Load(memuXMLPath);

            SimpleMemuAccordion simpleMemuAccordion = new SimpleMemuAccordion(_ClientStaticID: clientStaticID, _MemuInfoList: memuList, _UserAuthorityArray: userAuthorityArray, _Width: width, _CssClass: cssClassName);
            return new MvcHtmlString(simpleMemuAccordion.GetHtmlString());
        }
    }
}
