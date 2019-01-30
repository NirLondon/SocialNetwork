using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Notifications.Server.Startup))]

namespace Notifications.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            InitDepndencies();

            app.MapSignalR();
        }

        private void InitDepndencies()
        {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider),() => new UserIdProvider());
        }
    }
}
