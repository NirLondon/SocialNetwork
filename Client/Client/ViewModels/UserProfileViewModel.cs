using Client.DataProviders;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class UserProfileViewModel
    {
        public UserDetails Details { get; set; }
        public ISocialDataProvider _dataProvider { get; set; }


        public UserProfileViewModel(UserDetails _details, ISocialDataProvider dataProvider)
        {
            Details = _details;
            _dataProvider = dataProvider;
        }


        public async void Follow()
        {
            var result = await _dataProvider.Follow(Details.UserID);
        }

        public async void Block()
        {
            var result = await _dataProvider.Block(Details.UserID);
        }
    }
}
