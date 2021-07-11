using CrossNull.Logic.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult>  Login(string login, string password)
        {
            var context = HttpContext.GetOwinContext();
            var manager = context.Get<SignInManager<IdentityUser, string>>();
            var signInResult = manager.PasswordSignIn(login, password, true, false);
            if (signInResult != SignInStatus.Success)
            {
                return View("Login");
            }
            var user = await manager.UserManager.FindByNameAsync(login);
            manager.SignIn(user, true, true);
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        public ActionResult LogOut()
        {
            var context = HttpContext.GetOwinContext();
            context.Authentication.SignOut();

            return RedirectToAction("Login", "Login");
        }

    }
}