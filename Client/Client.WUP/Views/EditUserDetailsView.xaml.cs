using Client.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Client.WUP.Views
{
    public sealed partial class EditUserDetailsView : Page
    {
        public EditUserDetailsViewModel ViewModel { get; set; }

        public EditUserDetailsView()
        {
            ViewModel = new EditUserDetailsViewModel();

            this.InitializeComponent();
        }
    }
}
