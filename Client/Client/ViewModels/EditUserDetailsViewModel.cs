using System;
using Client.DataProviders;
using Client.HttpClinents;
using System.Threading.Tasks;
using System.ComponentModel;
using Client.Models;
using Client.ServicesInterfaces;

namespace Client.ViewModels
{
    public class EditUserDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IEditDetailsDataProvider _dataProvider;
        public IFollowedUsersService _followedService { get; set; }

        private UserDetails _userDetails;
        public UserDetails UserDetails
        {
            get => _userDetails; private set
            {
                _userDetails = value;
                OnPropertyChanged();
            }
        }

        public EditUserDetailsViewModel(IFollowedUsersService followService)
            : this(new EditDetailsHttpClient(), followService) { }

        public EditUserDetailsViewModel(IEditDetailsDataProvider dataProvider, IFollowedUsersService followService)
        {
            _dataProvider = dataProvider;
            _followedService = followService;
            UserDetails = new UserDetails();
            InitializeUserDetails();
        }

        public async void SaveChanges()
        {
            try
            {
                await _dataProvider.UpdateUserDetails(UserDetails);
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredTpken();
            }
        }

        private async void InitializeUserDetails()
        {
            try
            {
                UserDetails = await _dataProvider.GetUserDetails();
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

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}