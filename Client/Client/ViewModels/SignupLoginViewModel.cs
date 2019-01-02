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

        public SignupLoginViewModel(ISignupLoginService service)
        {
            _viewService = service;
            API_Client = new SignupLoginHttpClient();
        }


        public async void Signup()
        {
            if (Username == null || Username == "" || Password == null || Password == "")
            {
                Message = "Insert username or password";
            }
            else
            {
                Tuple<string, ErrorEnum> tuple = await API_Client.Signup(Username, Password);
                if (tuple.Item2 == ErrorEnum.EverythingIsGood)
                {
                    _viewService.NavigateToMainPage(tuple.Item1);
                }
                else
                {
                    Message = ManageError(tuple.Item2);
                }
            }
        }

        public async void Login()
        {
            Tuple<string, ErrorEnum> tuple = await API_Client.Login(Username, Password);

            if (tuple.Item2 == ErrorEnum.EverythingIsGood)
            {
                _viewService.NavigateToMainPage(tuple.Item1);
            }
            else
            {
                Message = ManageError(tuple.Item2);
            }
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
