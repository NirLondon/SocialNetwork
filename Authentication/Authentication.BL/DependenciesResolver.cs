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

            _container.Register<IUsersRepository>(() => new UsersRepository());
            _container.Register<ITokensRepository>(() => new TokensRepository());
            _container.Register<IUsersManager, UsersManager>();
            _container.Register<ITokensValidator, TokensValidator>();
            _container.Register<IUserState, UserStateManager>();

            _container.Verify();
        }

        public T GetInstanceOf<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}
