using Ninject;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CrossNull.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);//
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var kernel = new StandardKernel();
            kernel.Load("CrossNull.*.dll");//сканирует текущую папку в кот запущено прилож
            //ищет в ней файлы по маске regeх рег.выражения загрузит в память и начнет сканировать на наличие типов
            //кот наследуются от NinjectModule если есть он создает экз наследника и вызывает метод Load().
            //автоматически регистр. все зависимости.

            DependencyResolver.SetResolver(new Ninject.Web.Mvc.NinjectDependencyResolver(kernel));// установка в качестве DI контейнера поумолчанию для MVC
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);//дополнительно регстрирует DI контейнер для WebApi
        }
    }
}
