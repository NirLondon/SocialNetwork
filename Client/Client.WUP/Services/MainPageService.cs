using Client.ServicesInterfaces;
using Client.WUP.UserControls;
using Client.WUP.Views;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        public void GoToFollowers()
        {
            stackPanelContent.Children.Clear();
            stackPanelContent.Children.Add(new FollowersUserControl());
        }

        public void GoToBlocked()
        {
            stackPanelContent.Children.Clear();
            stackPanelContent.Children.Add(new BlockedUserControl());
        }
    }
}
