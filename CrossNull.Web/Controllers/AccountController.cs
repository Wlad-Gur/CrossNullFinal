using CrossNull.Logic.Services;
using CrossNull.Web.Model;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CrossNull.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        private string condCorrect = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        private string condDif = @"^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$";
        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            string email = registerViewModel.Email;
            Result tf;
            if (ModelState.IsValid)
            {
                RegisterModel registerModel = new RegisterModel()
                {
                    Email = email,
                    Password = registerViewModel.Password,
                    UserName = registerViewModel.UserName
                };
                tf = _userService.AddUser(registerModel);
                if (tf.IsSuccess) return RedirectToAction("Login", "Login");
                //TODO добавить кастьмную ошибку в ModelState.AddError
            }
            return View(registerViewModel);

        }
    }
}