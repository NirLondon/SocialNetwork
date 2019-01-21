using Authentication.Common.BL;
using Authentication.Common.DAL;
using Authentication.DAL;
using SimpleInjector;

namespace Authentication.BL
{
    public class DependenciesResolver
    {
        private readonly Container _container;

        public DependenciesResolver()
        {
            _container = new Container();

            _container.RegisterSingleton<IUsersRepository>(() => new UsersRepository());
            _container.RegisterSingleton<ITokensRepository>(() => new TokensRepository());

            _container.RegisterSingleton<IUsersManager, UsersManager>();
            _container.RegisterSingleton<ITokensValidator, TokensValidator>();
            _container.RegisterSingleton<IUserStateManager, UserStateManager>();
            _container.Register<INotifier, Notifier>();

            _container.Verify();
        }

        public T GetInstanceOf<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}
