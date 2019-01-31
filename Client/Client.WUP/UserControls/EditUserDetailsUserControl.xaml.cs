using Client.ViewModels;
using Client.WUP.Services;
using Windows.UI.Xaml.Controls;

namespace Client.WUP.Views
{
    public sealed partial class EditUserDetailsUserControl : UserControl
    {
        public EditUserDetailsViewModel ViewModel { get; set; }

        public EditUserDetailsUserControl()
        {
            ViewModel = new EditUserDetailsViewModel(new FollowedUserService());

            this.InitializeComponent();
        }
    }
}
