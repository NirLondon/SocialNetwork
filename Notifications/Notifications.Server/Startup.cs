using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using System.Configuration;
using SimpleInjector;
using Notifications.Common.DAL;
using Notifications.DAL;
using Notifications.Common.BL;
using Notifications.Server.Hubs;

namespace Notifications.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(InitDepndencies());
        }

        private HubConfiguration InitDepndencies()
        {
            var container = new Container();

            container.Register<INotificationsRepository, DynamoDBNotificationsRepository>();
            container.Register<INotifier, NotificationsHub>();

            container.Verify();

            return new HubConfiguration
            {
                Resolver = new SimpleInjectorDependencyResolver(container)
            };
        }
    }
}
