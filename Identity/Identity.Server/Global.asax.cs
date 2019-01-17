﻿using Identity.Common.DAL;
using Identity.Common.BL;
using Identity.DAL;
using Identity.BL;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;

namespace Identity.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ConfigureDependencies();
        }

        private void ConfigureDependencies()
        {
            var container = new Container();

            container.RegisterSingleton<IIdentitiesRepository, IdentitiesRpository>();
            container.RegisterSingleton<IAuthentiacator, Authenticator>();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
