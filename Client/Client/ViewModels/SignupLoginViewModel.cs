using Client.DataProviders;
using Client.Enums;
using Client.Exeptions;
using Client.HttpClinents;
using Client.ServicesInterfaces;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class SignupLoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISignupLoginDataProvider _dataProvider;
        public ISignupLoginService _viewService { get; set; }
        private bool LoggedWithFacebook = false;

        public SignupLoginViewModel(ISignupLoginService service) 
            : this(new SignupLoginHttpClient(), service) { }

        public SignupLoginViewModel(ISignupLoginDataProvider dataProvider, ISignupLoginService service)
        {
            Sending = false;
            _dataProvider = dataProvider;
            _viewService = service;
        }

        public string Username { get; set; }
        public string Password { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChange(); }
        }

        private bool _sending;
        public bool Sending
        {
            get { return _sending; }
            set { _sending = value; OnPropertyChange(); }
        }

        public async void Signup()
        {
            if (ValidateInput())
            {
                Sending = true;
                try
                {
                    await _dataProvider.Signup(Username, Password);
                    _viewService.NavigateToMainPage(LoggedWithFacebook);
                }
                catch (UsernameAlreadyExistException e)
                {
                    Message = "Username already exist";
                }
                Sending = false;
            }
        }

        public async void Login()
        {
            if (ValidateInput())
            {
                Sending = true;
                try
                {
                    await _dataProvider.Login(Username, Password);
                    _viewService.NavigateToMainPage(LoggedWithFacebook);
                }
                catch (WrongUsernameOrPasswordException e)
                {
                    Message = "Wrong username or password";
                }
                catch (UsernameAlreadyExistException e)
                {
                    Message = "User with this name already exist";
                }
                Sending = false;
            }
        }

        public async void LoginWithFacebook()
        {
            Sending = true;
            string facebookToken = await _viewService.LoginWithFacebook();
            if (facebookToken != null)
            {
                try
                {
                    await _dataProvider.LoginWithFacebook(facebookToken);
                    LoggedWithFacebook = true;
                    _viewService.NavigateToMainPage(LoggedWithFacebook);
                }
                catch (WrongUsernameOrPasswordException e)
                {
                    ManageUserSwitch();
                }
            }
            Sending = false;
        }

        private async void ManageUserSwitch()
        {
            if (ValidateInput())
            {
                bool wantToSwitch = await _viewService.SwitchToFacebookMessage();
                if (wantToSwitch)
                {
                    try
                    {
                        await _dataProvider.SwitchToFacebookUser(Username, Password);
                        Message = "User converted to facebook user!";
                    }
                    catch (UserIsBlockedException e)
                    {
                        Message = "User is blocked!";
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && Username[0] != '_')
                return true;

            Message = "Insert proper username or password";
            return false;
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
