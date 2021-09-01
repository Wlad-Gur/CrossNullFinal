using CrossNull.Logic.Services;
using CrossNull.Web.Model;
using CSharpFunctionalExtensions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            // TODO раскоментировать и убедиться, что не нал
            // var manager = HttpContext.GetOwinContext().Get<SignInManager<IdentityUser, string>>();

            string email = registerViewModel.Email;
            Result tf;
            if (ModelState.IsValid)
            {
                RegisterModel registerModel = new RegisterModel()
                {
                    Email = email,
                    Password = registerViewModel.Password,
                    UserName = registerViewModel.UserName,
                    Age = registerViewModel.Age,
                    //  Id = registerViewModel.Id
                };

                tf = _userService.AddUser(registerModel);
                if (tf.IsSuccess) return RedirectToAction("Login", "Login");
                ViewBag.MessageIf = tf.Error;
                ModelState.AddModelError("", tf.Error);//уровень модели?
                //TODO добавить кастoмную ошибку в ModelState.AddError
            }

            ViewBag.Message = "Repeat to fill form again";
            return View(registerViewModel);

        }

        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string email)
        {
            var result = _userService.ResetPassword(email);
            if (result.IsFailure)
            {
                ViewBag.Error = result.Error;
                return View();
            }
            ViewBag.Success = "Success";
            return View("Success", new SuccessViewModel("We sent you email with instructions."));
        }

        public ActionResult ConfirmEmail(string token, string userId)
        {
            //TODO проверка входных параметров
            if (token == null)
            {
                return View(Result.Failure("Token is empty"));
            }
            if (userId == null)
            {
                return View(Result.Failure("UserId incorrect"));
            }
            var result = _userService.VerifyEmailToken(token, userId);
            if (result.IsFailure)
            {

                return View(result);// TODO если ошибка выводить ошибку в созданном VIEW
            }
            return RedirectToAction("ChangePassword", "AccountController") /*View(result)*/;// Если все хорошо написать спасибо емаил подтвержден
        }

        [HttpGet]
        public ActionResult ChangePassword(string token, string userId)
        {
            //TODO валидация
            ChangePassViewModel changePassViewModel = new ChangePassViewModel()
            {
                Token = token,
                UserId = userId,
            };
            return View(changePassViewModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassViewModel changePassViewModel)
        {
            if (!ModelState.IsValid)
            {
                //TODO выкинуть ошибку
                return View();
            }
            //TODO вызвать метод ResetPassword(string userId, string token, string password)
            // пранализировать результат и принять решение что выводить
            var resetPassword = _userService.ResetPassword(changePassViewModel.UserId,
                changePassViewModel.Token, changePassViewModel.Password);
            if (resetPassword.IsFailure)
            {
                ViewBag.resPassFailure = resetPassword.Error;
                return View(new ChangePassViewModel()
                {
                    Token = changePassViewModel.Token,
                    UserId = changePassViewModel.UserId,
                });
            }
            CrossNull.Web.Model.SuccessViewModel successVM = new SuccessViewModel("Password changed successful");
            return RedirectToRoute(new { controller = "Success", action = "SuccessAct",
               successVM});
        }
    }
}