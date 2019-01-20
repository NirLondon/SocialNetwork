using Authentication.Common.BL;
using Authentication.DAL;

namespace Authentication.BL
{
    public class UserStateManager : IUserStateManager
    {
        UsersRepository repository;

        public UserStateManager()
        {
            repository = new UsersRepository();
        }

        public void BlockUser(string username)
        {
            repository.BlockUser(username);
        }

        public void UnblockUser(string username)
        {
            repository.UnBlockUser(username);
        }
    }
}
