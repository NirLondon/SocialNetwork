using Client.HttpClinents;
using Client.Models.ReturnedDTOs;
using Client.ViewModels;
using Client.WUP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class NotificationsUserControl : UserControl
    {
        public NotificationViewModel viewModel { get; set; }
        public NotificationPusher Pusher { get; set; }
        public NotificationService service { get; set; }
        public NotificationsUserControl(ObservableCollection<Notification> notifications)
        {
            this.InitializeComponent();
            Pusher = new NotificationPusher();
            service = new NotificationService();
            viewModel = new NotificationViewModel(notifications, Pusher, service);
        }
    }
}
