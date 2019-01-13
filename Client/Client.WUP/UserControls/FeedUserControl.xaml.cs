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
    public sealed partial class FeedUserControl : UserControl
    {
        private FeedViewModel viewModel { get; set; }
        public PostService postService { get; set; }
        public SocialHttpClient dataProvider { get; set; }
        public FeedUserControl()
        {
            this.InitializeComponent();
            postService = new PostService();
            dataProvider = new SocialHttpClient();
            viewModel = new FeedViewModel(postService, dataProvider);
        }
    }
}
