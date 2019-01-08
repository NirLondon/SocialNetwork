using Authentication.Common.BL;
using Authentication.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.BL
{
    public class UserStateManager : IUserState
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
