using Client.Enum;
using Client.HttpClinents;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Client.ViewModels
{
    public class SignupLoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SignupLoginHttpClient API_Client { get; set; }
        public ISignupLoginService _viewService { get; set; }

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


        public SignupLoginViewModel(ISignupLoginService service)
        {
            Sending = false;
            _viewService = service;
            API_Client = new SignupLoginHttpClient();
        }


        public async void Signup()
        {
            if (ValidateInput())
            {
                Sending = true;
                Tuple<string, ErrorEnum> tuple = await API_Client.Signup(Username, Password);
                if (tuple.Item2 == ErrorEnum.EverythingIsGood)
                {
                    _viewService.NavigateToMainPage(tuple.Item1);
                }
                else
                {
                    Message = ManageError(tuple.Item2);
                }
                Sending = false;
            }
        }

        public async void Login()
        {
            if (ValidateInput())
            {
                Sending = true;
                Tuple<string, ErrorEnum> tuple = await API_Client.Login(Username, Password);

                if (tuple.Item2 == ErrorEnum.EverythingIsGood)
                {
                    _viewService.NavigateToMainPage(tuple.Item1);
                }
                else
                {
                    Message = ManageError(tuple.Item2);
                }
                Sending = false;
            }
        }

        public async void LoginWithFacebook()
        {
            string facebookToken = await _viewService.LoginWithFacebook();
            if (facebookToken != null)
            {
                Sending = true;
                Tuple<string, ErrorEnum> tuple = await API_Client.LoginWithFacebook(facebookToken);
                if (tuple.Item2 == ErrorEnum.EverythingIsGood)
                {
                    _viewService.NavigateToMainPage(tuple.Item1);
                }
                else
                {
                    if (tuple.Item2 == ErrorEnum.WrongUsernameOrPassword)
                    {
                        ManageUserSwitch();
                    }
                    else
                        Message = ManageError(tuple.Item2);
                }
                Sending = false;
            }
        }


        private async void ManageUserSwitch()
        {
            if (ValidateInput())
            {
                bool wantToSwitch = await _viewService.SwitchToFacebookMessage();
                if (wantToSwitch)
                {
                    ErrorEnum success = await API_Client.SwitchToFacebookUser(Username, Password);
                    if (success == ErrorEnum.EverythingIsGood)
                    {
                        Message = "User converted to facebook user!";
                    }
                    else
                    {
                        ManageError(success);
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            bool flag = true;
            if (Username == null || Username == "" || Password == null || Password == "" || Username[0] == '_')
            {
                flag = false;
                Message = "Insert proper username and password";
            }
            return flag;
        }

        private string ManageError(ErrorEnum eror)
        {
            string msg = "";
            switch (eror)
            {
                case ErrorEnum.WrongUsernameOrPassword:
                    msg = "Wrong username or password";
                    break;
                case ErrorEnum.ConectionFailed:
                    msg = "Bad internet conection";
                    break;
                case ErrorEnum.UsernameAlreadyExist:
                    msg = "Username already exist";
                    break;
            }
            return msg;
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
