using Client.DataProviders;
using Client.HttpClinents;
using Client.ViewModels;
using Client.WUP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Client.WUP.UserControls
{
    public sealed partial class FollowersUserControl : UserControl
    {
        public FollowersViewModel viewModel { get; set; }
        public ISocialDataProvider dataProvider { get; set; }
        public FollowedUserService followedService { get; set; }
        public FollowersUserControl()
        {
            this.InitializeComponent();
            dataProvider = new SocialHttpClient();
            followedService = new FollowedUserService();
            viewModel = new FollowersViewModel(dataProvider, followedService);
        }
    }
}
