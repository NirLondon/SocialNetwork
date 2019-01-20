using Authentication.BL;
using Authentication.Common.BL;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;

namespace Authentication.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SetDependencies();
        }

        private void SetDependencies()
        {
            var BlResolver = new DependenciesResolver();

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterSingleton<IUsersManager>(() => BlResolver.GetInstanceOf<IUsersManager>());
            container.RegisterSingleton<IUserStateManager>(() => BlResolver.GetInstanceOf<IUserStateManager>());
            container.RegisterSingleton<ITokensValidator>(() => BlResolver.GetInstanceOf<ITokensValidator>());
            container.RegisterSingleton<INotifier>(() => BlResolver.GetInstanceOf<INotifier>());

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver 
                = new SimpleInjectorWebApiDependencyResolver(container);
        }

    }
}
