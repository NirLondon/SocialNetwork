using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class MainPageViewModel
    {
        private IMainPageService _viewService { get; set; }

        public MainPageViewModel(IMainPageService service)
        {
            _viewService = service;
        }
    }
}
