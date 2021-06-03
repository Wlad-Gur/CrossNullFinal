using CrossNull.Data;
using CrossNull.Logic.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic
{
    public class LogicModule : Ninject.Modules.NinjectModule
    {

        public override void Load()
        {
            Bind<GameContext>().ToSelf();
            Bind<IStatisticService>().To<StatisticService>();
            Bind<IGameService>().To<GameService>();
            Bind<IUserStore<IdentityUser>>().To<UserStore<IdentityUser>>().
                WithConstructorArgument<GameContext>(new GameContext());
            Bind<UserManager<IdentityUser>>().ToSelf();
            Bind<IUserService>().To<UserService>();
                //ToMethod(ctx => new UserStore<IdentityUser>( это лучше
                //(System.Data.Entity.DbContext)ctx.Kernel.GetService(typeof(GameContext))));//разобраться, создаем автоматом экземпляр GameContext

        }
    }
}
