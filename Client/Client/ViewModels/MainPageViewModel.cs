using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class MainPageViewModel
    {
        private IMainPageService _viewService { get; set; }
        private bool _loggedWithFacebook { get; set; }

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

        public void logOut()
        {
             _viewService.LogOut(_loggedWithFacebook);
        }
    }
}
