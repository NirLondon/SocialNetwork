using Client.DataProviders;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels
{
    public class FollowedUsersViewModel
    {
        public ISocialDataProvider _dataProvider { get; set; }
        public ObservableCollection<UserDetails> Users { get; set; }

        public FollowedUsersViewModel(ISocialDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            InitUsers();
        }

        private void InitUsers()
        {
            Users = new ObservableCollection<UserDetails>();
            var list = _dataProvider.GetFollowed();
            foreach (var user in list)
            {
                Users.Add(item);
            }
        }
    }
}
