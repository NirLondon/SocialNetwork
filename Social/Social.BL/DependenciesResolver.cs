using SimpleInjector;
using Social.Common.DAL;
using Social.DAL;
using System;

namespace Social.BL
{
    public class DependenciesResolver
    {
        private readonly Container _container;

        private DependenciesResolver(Action<DependenciesResolver> registrator)
        {
            _container = new Container();

            registrator(this);

            _container.Verify();
        }

        public DependenciesResolver(params (Type service, Type imp)[] ps)
        : this(ds =>
        {
            foreach (var item in ps)
                ds._container.Register(item.service, item.imp);
        })
        { }

        public DependenciesResolver(params (Type service, Func<object> imp)[] ps)
        : this(ds =>
        {
            foreach (var item in ps)
                ds._container.Register(item.service, item.imp);
        })
        { }

        public T GetInstanceOf<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}
