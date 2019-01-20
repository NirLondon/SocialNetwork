using Client.DataProviders;
using Client.Models;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Client.ViewModels
{
    public class BlockedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ISocialDataProvider _dataProvider { get; set; }
        public ObservableCollection<UserDetails> Users { get; set; }
        private IFollowedUsersService _followedService { get; set; }

        private UserDetails _selectedUser;
        public UserDetails SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (!IsItemSelected)
                    IsItemSelected = true;
                _selectedUser = value;
            }
        }

        private bool _isItemSelected;
        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set { _isItemSelected = value; OnPropertyChange(); }
        }

        public BlockedViewModel(ISocialDataProvider dataProvider, IFollowedUsersService followedService)
        {
            _dataProvider = dataProvider;
            _followedService = followedService;
            InitUsers();
        }


        public async void UnBlock()
        {
            try
            {
                await _dataProvider.UnBlock(SelectedUser.UserID);
                Users.Remove(SelectedUser);
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredTpken();
            }
        }

        public void GoToProfile()
        {
            _followedService.GoToUserProfile(SelectedUser, _dataProvider);
        }


        private async void InitUsers()
        {
            Users = new ObservableCollection<UserDetails>();
            try
            {
                var result = await _dataProvider.GetBlocked();
                foreach (var user in Users)
                {
                    Users.Add(user);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredTpken();
            }
        }

        private void ExpiredTpken()
        {
            _followedService.LogOut();
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}