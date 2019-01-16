using Client.HttpClinents;
using Client.ServicesInterfaces;
using Client.WUP.UserControls;
using Client.WUP.Views;
using Client.Models;
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
using Client.ViewModels;

namespace Client.WUP.Services
{
    public class MainPageService : IMainPageService
    {
        public StackPanel stackPanelContent { get; set; }

        private static readonly object _lock = new object();
        private static MainPageService _instance;

        public static MainPageService Instance
        {
            get
            {
                if (_instance == null)
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new MainPageService();
                    }
                return _instance;
            }
        }

        private MainPageService() { }

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
            if (MainPageViewModel._loggedWithFacebook)
                await FacebookService.Instance.LogoutAsync();

            Window.Current.Content = new SignupLoginView();
        }

        public void GoToFollowed()
        {
            stackPanelContent.Children.Clear();
            stackPanelContent.Children.Add(new FollowedUsersUserControl());
        }
    }
}
