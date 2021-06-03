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
            kernel.Load("CrossNull.*.dll");//��������� ������� ����� � ��� �������� ������
            //���� � ��� ����� �� ����� rege� ���.��������� �������� � ������ � ������ ����������� �� ������� �����
            //��� ����������� �� NinjectModule ���� ���� �� ������� ��� ���������� � �������� ����� Load().
            //������������� �������. ��� �����������.

            DependencyResolver.SetResolver(new Ninject.Web.Mvc.NinjectDependencyResolver(kernel));// ��������� � �������� DI ���������� ����������� ��� MVC
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);//������������� ����������� DI ��������� ��� WebApi
        }
    }
}
