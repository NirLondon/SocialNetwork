using System;
using System.Threading.Tasks;
using Client.Enum;

namespace Client.DataProviders
{
    public interface ISignupLoginDataProvider
    {
        Task Login(string username, string password);
        Task LoginWithFacebook(string facebookToken);
        Task Signup(string username, string password);
        Task SwitchToFacebookUser(string username, string password);
    }
}