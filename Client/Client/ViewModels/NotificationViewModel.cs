using Client.DataProviders;
using Client.Exeptions;
using Client.Models.ReturnedDTOs;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Client.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INotificationsPusher _notificator { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }
        public INotificationService _notificaionService { get; set; }

        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get { return _selectedNotification; }
            set
            {
                if (!IsItemSelected)
                    IsItemSelected = true;
                _selectedNotification = value;
                if (Notifications.Count == 0)
                    IsItemSelected = false;
            }
        }

        private bool _isItemSelected;
        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set { _isItemSelected = value; OnPropertyChange(); }
        }


        public NotificationViewModel(ObservableCollection<Notification> _notifications, INotificationsPusher notificator, INotificationService notificationService)
        {
            Notifications = _notifications;
            _notificator = notificator;
            _notificaionService = notificationService;
        }


        public async void MarkAsRead()
        {
            try
            {
                await _notificator.RemoveNotification(SelectedNotification);
                Notifications.Remove(SelectedNotification);
            }
            catch (TokenExpiredExeption e)
            {
                ExpireToken();
            }
        }

        private void ExpireToken()
        {
            _notificaionService.LogOut();
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
