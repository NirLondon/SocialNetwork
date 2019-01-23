using Microsoft.AspNet.SignalR;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace Notifications.Server
{
    public class SimpleInjectorDependencyResolver : DefaultDependencyResolver
    {
        private readonly Container _container;

        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        public override void Register(Type serviceType, Func<object> activator)
        {
            _container.Register(serviceType, activator);
        }
    }
}