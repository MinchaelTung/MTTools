using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using WebApiDemoServer.Models;

namespace WebApiDemoServer.Controllers
{

    //更改过的类 Global.cs 和 WebApiConfig.cs
    //自动更改的配置 Web.config
    //安装Json.Net  Newtonsoft.Json.dll(可选，作用 Object 和 Json String 互转工具)
    public class HController : Controller
    {
        // GET: H/GetAll     
        public JsonResult GetAll()
        {


            UserInfo.GetDate();

            //JsonRequestBehavior.AllowGet 表示可以使用GET方式访问 ,DenyGet表示Post访问
            return Json(UserInfo.list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            UserInfo.GetDate();
            return Json(UserInfo.list.SingleOrDefault(s => s.ID == id), JsonRequestBehavior.AllowGet);
        }


        public JsonResult Add(string jsonStr)
        {
            //Newtonsoft.Json.dll
            UserInfo user = JsonConvert.DeserializeObject<UserInfo>(jsonStr);
            UserInfo.GetDate();
            UserInfo.list.Add(user);
            return Json(user, JsonRequestBehavior.DenyGet);
        }

    }
}