using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    //如何使用默认路由
    //[RoutePrefix("BB")] 可以更改当前默认路由名字 如下原来是Bk 更改为BB
    //可以在 Controller 上使用 [Route] 标记， 并将 Action 作为参数；
    //路由会在所有的 Action 方法中启用；
    //例如： [Route("{action=index}")]
    [RoutePrefix("BB")]
    [Route("{action=index}")]
    public class BkController : Controller
    {
     //~\BB
 
        public ActionResult Index() { return View(); }

     //~\BB\Show
        public ActionResult Show() { return View(); }
        //~\BB\new
        public ActionResult New() { return View(); }

        //覆盖路由
        [Route("Edit/{bookId:int}")]
        public ActionResult Edit(int bookId) { 
            //~\BB\edit\123
            return View();
        }

        //如何为路由指定名称 

        [Route("Booking",Name = "Payments")]
        public ActionResult Payments()
        {
            //~/BB/Booking
            //然后可以使用 Url.RouteUrl 方法来生成链接
            //就像这样： <a href="@Url.RouteUrl("Payments")">Payments Screen</a>
            return View();
        }
	}
}