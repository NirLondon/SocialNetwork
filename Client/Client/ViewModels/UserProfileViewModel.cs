using Client.DataProviders;
using Client.Models;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Client.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        public UserDetails Details { get; set; }
        public ISocialDataProvider _dataProvider { get; set; }
        private IFollowedUsersService _followedService { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string tbxBlock;
        public string TBXBlock
        {
            get { return tbxBlock; }
            set { tbxBlock = value; OnPropertyChange(); }
        }

        private string tbxFollow;
        public string TBXFollow
        {
            get { return tbxFollow; }
            set { tbxFollow = value; OnPropertyChange(); }
        }



        public UserProfileViewModel(UserDetails _details, ISocialDataProvider dataProvider, IFollowedUsersService followedService)
        {
            _followedService = followedService;
            Details = _details;
            _dataProvider = dataProvider;
        }


        public async void Follow()
        {
            try
            {
                if (Details.IsFollowed)
                {
                    await _dataProvider.UnFollow(Details.UserID);
                    TBXFollow = "Follow";
                }
                else
                {
                    await _dataProvider.Follow(Details.UserID);
                    TBXBlock = "UnFollow";
                }
                Details.IsFollowed = !Details.IsFollowed;
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredTpken();
            }
        }

        public async void Block()
        {
            try
            {
                if (Details.IsBlocked)
                {
                    await _dataProvider.UnBlock(Details.UserID);
                    TBXBlock = "Block";
                }
                else
                {
                    await _dataProvider.Block(Details.UserID);
                    TBXBlock = "UnBlock";
                }
                Details.IsBlocked = !Details.IsBlocked;
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
