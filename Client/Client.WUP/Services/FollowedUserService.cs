using Client.DataProviders;
using Client.Models;
using Client.Models.ReturnedDTOs;
using Client.ServicesInterfaces;
using Client.WUP.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WUP.Services
{
    public class FollowedUserService : IFollowedUsersService
    {
        public void GoToUserProfile(UserMention user, ISocialDataProvider dataProvider)
        {
            MainPageService.Instance.stackPanelContent.Children.Clear();
            MainPageService.Instance.stackPanelContent.Children.Add(new UserProfileUserControl(user, dataProvider));
        }

        public void LogOut()
        {
            MainPageService.Instance.LogOut();
        }
    }
}
