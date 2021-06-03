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

            }
            if (_gameContext.Users.Any(a=> a.Email == registerModel.Email))
            {
                //error
            }
            IdentityUser identityUser = new IdentityUser() { UserName = registerModel.UserName,
             Email = registerModel.Email};
            _userManager.Create(identityUser, registerModel.Password);
            //проверить результат операции, и сообщить успешно или нет.
            //проверить юзера на повторение, существует ли, с пощью коптекста  и юзер менеджера
            //если все хорошо дабавляем юзер,
            return Result.Success();//
        }
    }
}
