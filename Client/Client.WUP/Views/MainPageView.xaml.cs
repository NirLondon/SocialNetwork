﻿using Client.ViewModels;
using Client.WUP.Services;
using Client.WUP.UserControls;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client.WUP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageView : Page
    {
        private MainPageViewModel viewModel { get; set; }
        private MainPageService service { get; set; }

        public MainPageView()
        {
            this.InitializeComponent();
            service = new MainPageService(contentSP);
            viewModel = new MainPageViewModel(service);
        }
    }
}
