using CrossNull.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic.Helpers
{
    static class UserManagerHelper
    {

        internal static UserManager<IdentityUser> CreateUserManager()
        {
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new GameContext()));
            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 3,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };
            return manager;
        }
    }
}
