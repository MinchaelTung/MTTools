using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiDemoServer.Models;

namespace WebApiDemoServer.Controllers
{
    //更改过的类 Global.cs 和 WebApiConfig.cs
    //自动更改的配置 Web.config
    //更新为 Microsoft.AspNet Web Api 2.2 
    public class UserInfoController : ApiController
    {
        //不使用
        [NonAction]
        public string GetEmpty()
        {
            return string.Empty;
        }
        //特殊情况可以使用 HttpResponseMessage 
        //public HttpResponseMessage Get()
        //{
        //    // Get a list of products from a database.
        //    IEnumerable<Product> products = GetProductsFromDB();

        //    // Write the list to the response body.
        //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, products);
        //    //HttpStatusCode 返回状态
        //    return response;
        //}



        public IEnumerable<UserInfo> GetAll()
        {
            UserInfo.GetDate();
            return UserInfo.list;
        }
        //Action 别名 在路由配置规则中添加 {action} 节点
        //[ActionName("getuserbyid")]
        public UserInfo Get(int id)
        {
            UserInfo.GetDate();
            var result = UserInfo.list.SingleOrDefault(s => s.ID == id);

            return result;
        }

        //[HttpPost]
        public UserInfo Post(UserInfo user)
        {

            UserInfo.GetDate();
            UserInfo.list.Add(user);

            //用于添加
            // int id = user.ID;

            return user;
        }

        public void Put(UserInfo user)
        {
            //用于Update
            UserInfo edit = UserInfo.list.SingleOrDefault(s => s.ID == user.ID);
            edit.Name = user.Name;
            edit.Age = user.Age;
            edit.Book = user.Book;
            edit.Url = edit.Url;
        }

        public void Delete(int id)
        {
            //删除
            UserInfo user = UserInfo.list.SingleOrDefault(s => s.ID == id);

            UserInfo.list.Remove(user);

        }



    }
}
