using Client.DataProviders;
using Client.Exeptions;
using Client.Models;
using Client.Models.ReturnedDTOs;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Client.ViewModels
{
    public class FollowedUsersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ISocialDataProvider _dataProvider { get; set; }
        public ObservableCollection<UserMention> Users { get; set; }
        private IFollowedUsersService _followedService { get; set; }

        private UserMention _selectedUser;
        public UserMention SelectedUser
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

        public FollowedUsersViewModel(ISocialDataProvider dataProvider, IFollowedUsersService followedService)
        {
            _dataProvider = dataProvider;
            _followedService = followedService;
             InitUsers();
        }


        public async void UnFollow()
        {
            try
            {
                await _dataProvider.UnFollow(SelectedUser.UserId);
                Users.Remove(SelectedUser);
            }
            catch (TokenExpiredExeption e)
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
            Users = new ObservableCollection<UserMention>();
            try
            {
                var result = await _dataProvider.GetFollowed();
                foreach (var user in result)
                {
                    Users.Add(user);
                }
            }
            catch (TokenExpiredExeption e)
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
