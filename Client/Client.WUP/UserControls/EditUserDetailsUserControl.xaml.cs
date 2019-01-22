using Client.HttpClinents;
using Client.ServicesInterfaces;
using Client.ViewModels;
using Client.WUP.Services;
using Windows.UI.Xaml.Controls;

namespace Client.WUP.Views
{
    public sealed partial class EditUserDetailsUserControl : UserControl
    {
        public EditUserDetailsViewModel ViewModel { get; set; }
        public FollowedUserService followedService { get; set; }
        public EditDetailsHttpClient editDetailsDataProvider { get; set; }

        public EditUserDetailsUserControl()
        {
            this.InitializeComponent();
            followedService = new FollowedUserService();
            editDetailsDataProvider = new EditDetailsHttpClient();
            ViewModel = new EditUserDetailsViewModel(editDetailsDataProvider, followedService);
        }
    }
}
