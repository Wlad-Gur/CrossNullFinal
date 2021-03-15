using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CrossNull.Data;

namespace CrossNull.Web.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult NewGame()
        {
            ViewBag.Message = "Start new game";

            return View();
        }

        public ActionResult LoadGame()
        {
            ViewBag.Message = "Load privious game";

            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Development Vlad. Team lead Aleksandr";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Faind us in internet.";

            return View();
        }
    }
}