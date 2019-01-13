using System;
using Client.DataProviders;
using Client.HttpClinents;
using System.Threading.Tasks;
using System.ComponentModel;
using Client.Models;

namespace Client.ViewModels
{
    public class EditUserDetailsViewModel : INotifyPropertyChanged
    {
        private readonly IEditDetailsDataProvider _dataProvider;

        public EditUserDetailsViewModel(IEditDetailsDataProvider dataProvider)
        {
            this._dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));

            UserDetails = new UserDetails();
            InitializeUserDetails();
        }

        public EditUserDetailsViewModel() : this(new EditDetailsHttpClient()) { }

        private async Task InitializeUserDetails()
        {
            UserDetails = await _dataProvider.GetUserDetails();
        }

        private UserDetails _userDetails;
        public UserDetails UserDetails
        {
            get => _userDetails; private set
            {
                _userDetails = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveChanges()
        {
            _dataProvider.UpdateUserDetails(UserDetails);
        }
    }
}
