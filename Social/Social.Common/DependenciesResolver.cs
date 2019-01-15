using SimpleInjector;
using System;

namespace Social.Common
{
    public abstract class DependenciesResolver
    {
        private readonly Container _container;

        private DependenciesResolver(Action<DependenciesResolver> registrator)
        {
            _container = new Container();

            registrator(this);

            _container.Verify();
        }

        public DependenciesResolver(params (Type service, Type imp)[] registrations)
        : this(ds =>
        {
            foreach (var item in registrations)
                ds._container.Register(item.service, item.imp);
        })
        { }

        public DependenciesResolver(params (Type service, Func<object> imp)[] registrations)
        : this(ds =>
        {
            foreach (var item in registrations)
                ds._container.Register(item.service, item.imp);
        })
        { }

        public T GetInstanceOf<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}
