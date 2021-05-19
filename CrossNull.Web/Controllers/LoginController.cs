using CrossNull.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrossNull.Web.Controllers
{
    public class LoginController : Controller
    {
        private IStatisticService _statisticService;
        private string Name;
        public LoginController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
            //Name = login;
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login1(string login, string password)
        {

            return Content($"Login: {login}   Password: {password}");
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            string authData = $"Login: {login}   Password: {password}";
            ViewBag.Fignj = authData;
            ViewBag.Message = "List of Games";
            var stat = _statisticService.LoadGameStatistic().
                Where(w => w.Value.PlayerOne.Name == login
            || w.Value.PlayerTwo.Name == login);
            return View("PlayerResult", stat);
        }

    }
}