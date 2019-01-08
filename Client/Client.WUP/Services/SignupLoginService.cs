using Client.Enum;
using Client.ServicesInterfaces;
using Client.WUP.Views;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Client.WUP.Services
{
    public class SignupLoginService : ISignupLoginService
    {
        public void NavigateToMainPage()
        {
            Window.Current.Content = new MainPageView();
        }

        public async Task<string> LoginWithFacebook()
        {
            string token = null;
            FacebookService.Instance.Initialize("2204925279571954", FacebookPermissions.PublicProfile);
            var logged = await FacebookService.Instance.LoginAsync();
            if (logged)
            {
                token = FacebookService.Instance.Provider.AccessTokenData.AccessToken;
            }
            return token;
        }

        public async Task<bool> SwitchToFacebookMessage()
        {
            bool flag = false;
            string msg = "There was a conflict with your facebook id and a user. If it is your user you can switch it to facebook user, just fill the username and password";
            MessageDialog showDialog = new MessageDialog(msg);
            showDialog.Commands.Add(new UICommand("No") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Yes") { Id = 1 });
            showDialog.CancelCommandIndex = 0;
            showDialog.DefaultCommandIndex = 1;
            var result = await showDialog.ShowAsync();

            if ((int)result.Id == 1)
            {
                flag = true;
            }

            return flag;
        }
    }
}
