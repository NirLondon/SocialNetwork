using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Social.BL;
using Social.Common.BL;

namespace Social.Server
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
            var blResolver = new BLDependenciesResolver();

            var container = new Container();

            container.RegisterSingleton<IAuthentiacator, Authenticator>();
            container.RegisterSingleton<ISocialManager>(() => blResolver.GetInstanceOf<ISocialManager>());

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver
                = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
