using System.Linq;

namespace System.Web.Mvc
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string[] roles = this.Roles.Split('|');
            dynamic obj = filterContext.HttpContext.Session[filterContext.HttpContext.User.Identity.Name];
            if (obj == null)
            {
                return;
            }
            string[] userrole = ((string)obj.RolesToString).Split('|');
            bool isok = false;

            foreach (var item in roles)
            {
                if (userrole.Contains(item) == true)
                {
                    isok = true;
                    break;
                }
            }
            if (isok == false)
            {
                filterContext.HttpContext.Response.Redirect("~/Error/Index?msg=没有权限访问该页面");
            }

        }
    }
}