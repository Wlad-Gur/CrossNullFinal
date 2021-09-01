using CrossNull.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrossNull.Logic.Services
{
    class UserService : IUserService
    {
        private readonly GameContext _gameContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityMessageService emailService;

        public UserService(GameContext gameContext, UserManager<IdentityUser> userManager,
            IIdentityMessageService emailService)
        {
            this._gameContext = gameContext;
            this._userManager = userManager;
            this.emailService = emailService;
        }

        public BinaryReader Age { get; private set; }

        public Result AddUser(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return Result.Failure("Fill in all the fields and click Send.");
            }
            if (_gameContext.Users.Any(a => a.Email == registerModel.Email))
            {
                return Result.Failure("Email error. Try changing your email.");
            }

            if (_gameContext.Users.Any(a => a.UserName == registerModel.UserName))
            {
                //проверить юзера на повторение, существует ли, с пощью коптекста  и юзер менеджера
                return Result.Failure("UserName is uncorrect");
            }


            IdentityUser identityUser = new IdentityUser()
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email
            };

            if (!_userManager.Create(identityUser, registerModel.Password).Succeeded)
            {
                //проверить результат операции, и сообщить успешно или нет.
                return Result.Failure("User couldn't be added");
            }

            var task = emailService.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage()
            {
                Destination = registerModel.Email,
                Subject = "Сonfirmation your email",
                Body = "Is it you?",
            });

            // генерируем токен для подтверждения регистрации
            var code = _userManager.GenerateEmailConfirmationTokenAsync(identityUser.Id);
            // создаем ссылку для подтверждения
            var callbackUrl = new Url("https://localhost:44335/Login/Login");
            // отправка письма
            _userManager.SendEmailAsync(identityUser.Id, "Email confirmation",
                       "To complete registration follow the link:: <a href=\""
                                                       + callbackUrl + "\">complete registration</a>");


            if (registerModel.Age.HasValue)
            {
                _userManager.AddClaim(identityUser.Id, new System.Security.Claims.Claim("Age", $"{registerModel.Age}"));

            }
            return Result.Success();//
        }

        public Result ResetPassword(string email)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure("Email is empty.");
            }
            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
            {
                return Result.Failure("Invalid email.");
            }

            var user = _userManager.FindByEmail(email);

            if (user == null)
            {
                return Result.Failure("Invalid email.");
            }
            if (!user.EmailConfirmed)
            {
                return Result.Failure("User must confirm his email.");
            }

            var userToken = _userManager.GeneratePasswordResetToken(user.Id);
            string url = $"/Account/ChangePassword?token={userToken}&userId={user.Id}";
            _userManager.SendEmail(user.Id, "Change password", url);
            return Result.Success();
        }

        public Result ResetPassword(string userId, string token, string password)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
            {
                return Result.Failure("Incorrect user");
            }
            var identityRes = _userManager.ResetPassword(userId, token, password);
            if (identityRes.Succeeded)
            {
                return Result.Success();
            }
            return Result.Failure("Can't change password");
        }

        public Result SendCode(string userId)
        {
            throw new NotImplementedException();
        }

        public Result VerifyEmailToken(string token, string userId)
        {
            var result = _userManager.ConfirmEmail(userId, token);
            if (!result.Succeeded)
            {
                return Result.Failure("Email no corfirm");
            }
            return Result.Success();
        }
    }
}
