using System;
using System.Threading.Tasks;
using CrossNull.Logic.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CrossNull.Web.Startup))]

namespace CrossNull.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //если запросили <UserManager<IdentityUser>> то вызови UserManagerHelper.CreateUserManager и верни результат тому кро вызвал метод Get у OwinContext
            app.CreatePerOwinContext<UserManager<IdentityUser>>(UserManagerHelper.CreateUserManager);
            app.CreatePerOwinContext<SignInManager<IdentityUser, string>>(CreateSignInManager);


            //TODO сконфигурировать настройки кукисов
            app.UseCookieAuthentication
                (new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
                {
                    LoginPath = new PathString ("/Login/Login"),
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
                });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }



        private static SignInManager<IdentityUser, string> CreateSignInManager
            (IdentityFactoryOptions<SignInManager<IdentityUser, string>> opt, IOwinContext ctx)
        {
            var userManager = ctx.GetUserManager<UserManager<IdentityUser>>();
            return new SignInManager<IdentityUser, string>(userManager, ctx.Authentication) { AuthenticationType = "ApplicationCookie" };
        }

    }
}
