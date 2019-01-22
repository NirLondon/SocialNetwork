using Client.DataProviders;
using Client.Models.ReturnedDTOs;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Client.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IMainPageService _viewService { get; set; }
        public static bool _loggedWithFacebook { get; private set; }
        public INotificationsPusher _notificationPusher { get; set; }
        private ObservableCollection<Notification> _notifications { get; set; }

        private bool _isNotification;
        public bool IsNotification
        {
            get { return _isNotification; }
            set { _isNotification = value; OnPropertyChange(); }
        }



        public MainPageViewModel(IMainPageService service, bool loggedWithFacebook, INotificationsPusher notificationPusher)
        {
            _notifications = new ObservableCollection<Notification>();
            _notificationPusher = notificationPusher;
            _viewService = service;
            _loggedWithFacebook = loggedWithFacebook;
            _notificationPusher.Pushed += notificationPushed;
        }

        private void notificationPushed(Notification notification)
        {
            if (!IsNotification)
                IsNotification = true;
            _notifications.Add(notification);
        }

        public void GoToFeed()
        {
            _viewService.GoToFeed();
        }

        public void GoToIdentity()
        {
            _viewService.GoToIdentity();
        }

        public void GoToFollowed()
        {
            _viewService.GoToFollowed();
        }

        public void GoToFollowers()
        {
            _viewService.GoToFollowers();
        }

        public void GoToBlocked()
        {
            _viewService.GoToBlocked();
        }

        public void GoToNotifications()
        {
            _viewService.GoToNotifications(_notifications);
        }

        public void logOut()
        {
            _viewService.LogOut();
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
