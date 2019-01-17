using Client.Models;
using Client.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Client.WUP.UserControls
{
    public sealed partial class PostUserControl : UserControl
    {
        public PostUserControl()
        {
            this.InitializeComponent();

        }

        private void PublishComment(object sender, RoutedEventArgs e)
        {
            ((PostViewModel)DataContext).PublishComment();
        }

        private void ChooseImage(object sender, RoutedEventArgs e)
        {
            ((PostViewModel)DataContext).ChooseImage();
        }

        private void GoToProfile(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ((PostViewModel)DataContext).GoToProfile();
        }
    }
}
