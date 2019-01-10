using Client.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Client.WUP.Views
{
    public sealed partial class EditUserDetailsUserControl : UserControl
    {
        public EditUserDetailsViewModel ViewModel { get; set; }

        public EditUserDetailsUserControl()
        {
            ViewModel = new EditUserDetailsViewModel();

            this.InitializeComponent();
        }
    }
}
