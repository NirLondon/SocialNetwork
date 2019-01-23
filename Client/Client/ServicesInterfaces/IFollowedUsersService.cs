using Client.DataProviders;
using Client.Models;
using Client.Models.ReturnedDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ServicesInterfaces
{
    public interface IFollowedUsersService
    {
        void LogOut();

        void GoToUserProfile(UserMention user, ISocialDataProvider dataProvider);
    }
}
