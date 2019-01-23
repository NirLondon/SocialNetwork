using Client.Exeptions;
using Client.HttpClinents;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class SearchUserViewModel
    {
        public string SearchedUser { get; set; }
        public SocialHttpClient _socialDataProvider { get; set; }

        private UserDetails _foundUser;
        public UserDetails FoundUser
        {
            get { return _foundUser; }
            set { _foundUser = value; }
        }


        public SearchUserViewModel()
        {
            _socialDataProvider = new SocialHttpClient();
        }


        public async void Search()
        {
            try
            {
                _foundUser = await _socialDataProvider.Search(SearchedUser);
            }
            catch (TokenExpiredExeption e)
            {
                ExpiredToken();
            }
        }

        private void ExpiredToken()
        {
            
        }
    }
}
