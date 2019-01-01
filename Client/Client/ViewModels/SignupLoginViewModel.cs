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
        public string Message { get; set; }

        public SignupLoginViewModel(ISignupLoginService service)
        {
            _viewService = service;
            API_Client = new SignupLoginHttpClient();
        }


        public async void Signup()
        {
            bool result = await API_Client.Signup(Username, Password);
            Message = result.ToString();
        }

        public async void Login()
        {
            await API_Client.Login(Username, Password);
        }
    }
}
