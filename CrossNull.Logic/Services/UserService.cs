using CrossNull.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrossNull.Logic.Services
{
    class UserService : IUserService
    {
        private readonly GameContext _gameContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(GameContext gameContext, UserManager<IdentityUser> userManager)
        {
            this._gameContext = gameContext;
            this._userManager = userManager;
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
                Email = registerModel.Email,
                EmailConfirmed = true //TODO заменить на отправку письма когда заработает отправка писем
            };

            if (!_userManager.Create(identityUser, registerModel.Password).Succeeded)
            {
                //проверить результат операции, и сообщить успешно или нет.
                return Result.Failure("User couldn't be added");
            }
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

            var userToken = _userManager.GeneratePasswordResetToken(user.Id);
            string url = $"/Account/ChangePassword?token={userToken}&userId={user.Id}";
            _userManager.SendEmail(user.Id, "Change password", url);
            return Result.Success();
        }

        public Result SendCode(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
