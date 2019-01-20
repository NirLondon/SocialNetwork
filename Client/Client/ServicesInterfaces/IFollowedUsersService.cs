using Client.DataProviders;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ServicesInterfaces
{
    public interface IFollowedUsersService
    {
        void LogOut();

        void GoToUserProfile(UserDetails userDetails, ISocialDataProvider dataProvider);
    }
}
