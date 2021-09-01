using CrossNull.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CrossNull.Web.Controllers
{
    public class SuccessController : Controller
    {
        public SuccessViewModel SuccessVM { get; set; }
        //public SuccessController(SuccessViewModel successViewModel)
        //{
        //    SuccessVM = successViewModel;
        //}
        public ActionResult SuccessAct(SuccessViewModel successVM)
        {

            return View(successVM);
        }
    }
}

