using Client.DataProviders;
using Client.Models;
using Client.ServicesInterfaces;
using Client.WUP.UserControls;

namespace Client.WUP.Services
{
    public class FollowedUserService : IFollowedUsersService
    {
        public void GoToUserProfile(UserDetails userDetails, ISocialDataProvider dataProvider)
        {
            MainPageService.Instance.stackPanelContent.Children.Clear();
            MainPageService.Instance.stackPanelContent.Children.Add(new UserProfileUserControl(userDetails, dataProvider));
        }

        public void LogOut()
        {
            MainPageService.Instance.LogOut();
        }
    }
}
