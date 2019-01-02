using Client.ServicesInterfaces;
using Client.WUP.Views;
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
    }
}
