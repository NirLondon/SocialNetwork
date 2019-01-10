using Client.DataProviders;
using Client.Enum;
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
                ErrorEnum result = await _dataProvider.Signup(Username, Password);

                if (result == ErrorEnum.EverythingIsGood)
                    _viewService.NavigateToMainPage();
                else ManageError(result);

                Sending = false;
            }
        }

        public async void Login()
        {
            if (ValidateInput())
            {
                ErrorEnum result = await _dataProvider.Login(Username, Password);

                if (result == ErrorEnum.EverythingIsGood)
                    _viewService.NavigateToMainPage();
                else ManageError(result);
            }
        }

        public async void LoginWithFacebook()
        {
            string facebookToken = await _viewService.LoginWithFacebook();
            if (facebookToken != null)
            {
                ErrorEnum result = await _dataProvider.LoginWithFacebook(facebookToken);
                if (result == ErrorEnum.EverythingIsGood)
                    _viewService.NavigateToMainPage();
                else
                {
                    if (result == ErrorEnum.WrongUsernameOrPassword)
                        ManageUserSwitch();
                    else ManageError(result);
                }
            }
        }

        private async void ManageUserSwitch()
        {
            if (ValidateInput())
            {
                bool wantToSwitch = await _viewService.SwitchToFacebookMessage();
                if (wantToSwitch)
                {
                    ErrorEnum result = await _dataProvider.SwitchToFacebookUser(Username, Password);
                    if (result == ErrorEnum.EverythingIsGood)
                        Message = "User converted to facebook user!";
                    else ManageError(result);
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

        private void ManageError(ErrorEnum eror)
        {
            switch (eror)
            {
                case ErrorEnum.WrongUsernameOrPassword:
                    Message = "Wrong username or password";
                    return;
                case ErrorEnum.ConectionFailed:
                    Message = "Bad internet conection";
                    return;
                case ErrorEnum.UsernameAlreadyExist:
                    Message = "Username already exist";
                    return;
            }
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
