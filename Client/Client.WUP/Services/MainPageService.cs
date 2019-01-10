using Client.HttpClinents;
using Client.ServicesInterfaces;
using Client.WUP.UserControls;
using Client.WUP.Views;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.WUP.Services
{
    public class MainPageService : IMainPageService
    {
        public StackPanel stackPanelContent { get; set; }

        public MainPageService(StackPanel sp)
        {
            stackPanelContent = sp;
        }

        public void GoToFeed()
        {
            stackPanelContent.Children.Clear();
            stackPanelContent.Children.Add(new FeedUserControl());
        }

        public void GoToIdentity()
        {
            stackPanelContent.Children.Clear();
            stackPanelContent.Children.Add(new EditUserDetailsUserControl());
        }

        public async void LogOut()
        {
            HttpHelper.DeleteToken();
            if (FacebookService.Instance.Provider != null)
                await FacebookService.Instance.LogoutAsync();
            Window.Current.Content = new SignupLoginView();
        }
    }
}
