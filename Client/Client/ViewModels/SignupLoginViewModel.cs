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
                bool result = await API_Client.Signup(Username, Password);
                Message = result.ToString();
            }
        }

        public async void Login()
        {
            bool logged = await API_Client.Login(Username, Password);

            if (logged)
            {
                _viewService.NavigateToMainPage();
            }
            else
            {
                Message = logged.ToString();
            }
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
