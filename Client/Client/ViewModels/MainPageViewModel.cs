using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class MainPageViewModel
    {
        private IMainPageService _viewService { get; set; }
        public static bool _loggedWithFacebook { get; private set; }

        public MainPageViewModel(IMainPageService service, bool loggedWithFacebook)
        {
            _viewService = service;
            _loggedWithFacebook = loggedWithFacebook;
        }

        public void GoToFeed()
        {
            _viewService.GoToFeed();
        }

        public void GoToIdentity()
        {
            _viewService.GoToIdentity();
        }

        public void GoToFollowed()
        {
            _viewService.GoToFollowed();
        }

        public void GoToFollowers()
        {
            _viewService.GoToFollowers();
        }

        public void GoToBlocked()
        {
            _viewService.GoToBlocked();
        }

        public void logOut()
        {
            _viewService.LogOut();
        }
    }
}
