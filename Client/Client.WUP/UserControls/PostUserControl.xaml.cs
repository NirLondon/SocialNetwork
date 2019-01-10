using Client.Common;
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

        public Post pvm
        {
            get => (Post)GetValue(pvmProperty);
            set => SetValue(pvmProperty, value);
        }

        public static readonly DependencyProperty pvmProperty =
            DependencyProperty.Register("pvm", typeof(Post), typeof(PostUserControl), null);


        private void PublishComment(object sender, RoutedEventArgs e)
        {
            ((PostViewModel)DataContext).PublishComment();
        }

        private void ChooseImage(object sender, RoutedEventArgs e)
        {
            ((PostViewModel)DataContext).ChooseImage();
        }
    }
}
