using CrossNull.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Result AddUser(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                //error
            }
            if (_gameContext.Users.Any(a => a.Email == registerModel.Email))
            {
                //error
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
                return Result.Failure("User doesn't added");
            }

            return Result.Success();//
        }
    }
}
