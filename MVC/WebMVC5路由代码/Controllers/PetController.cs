using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    //Attribute Routing 简单使用
    public class PetController : Controller
    {
        //
        // GET: /Pet/
        [Route("Pet/{petkey?}")]
        public ActionResult GetPet(string petKey)
        {
            //~/pet/123
            this.ViewBag.Msg = petKey;
            return View();
        }

        [Route("pet/breed/{petkey?}")]
        public ActionResult GetSpecificPet(string petkey)
        {
            //~pet/breed/123
            this.ViewBag.Msg = petkey;
            return View();
        }
	}
}