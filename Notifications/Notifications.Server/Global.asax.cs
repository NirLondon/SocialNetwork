using Notifications.BL;
using Notifications.Common.BL;
using Notifications.Common.DAL;
using Notifications.DAL;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;

namespace Notifications.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            InitDependencies();
        }

        public void InitDependencies()
        {
            var container = new Container();

            container.RegisterSingleton<INotificationsRepository, DynamoDBNotificationsRepository>();
            container.RegisterSingleton<INotifier, SignalRNotifier>();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
