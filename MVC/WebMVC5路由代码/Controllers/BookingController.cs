using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
   
    public class BookingController : Controller
    {
        
        
        //如何使用路由前缀
        
        // eg: /Booking
        [Route("Booking")]
        public ActionResult Index()
        {
            //~booking
            return View();
        }

        // eg: /Booking/5
        [Route("Booking/{bookId}")]
        public ActionResult Show(int bookId)
        {
            //~booking/123
            return View();
        }

        // eg: /Booking/5/Edit
        [Route("Booking/{bookId}/Edit")]
        public ActionResult Edit(int bookId)
        {
            //~booking/123/edit
            return View();
        }

        //如何覆盖公用路由前缀
        //可以在标记前面添加一个波浪线 (~) 来覆盖公用前缀； - 例如： [Route("~/PetBooking")]
        [Route("~/PetBooking")]
        public ActionResult PetBooking() {
            //~/PetBooking
            return View(); 
        }
    }
}