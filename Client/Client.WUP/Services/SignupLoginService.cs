using Client.Enum;
using Client.ServicesInterfaces;
using Client.WUP.Views;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Client.WUP.Services
{
    public class SignupLoginService : ISignupLoginService
    {
        public void NavigateToMainPage(string token)
        {
            Window.Current.Content = new MainPageView(token);
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
    }
}
