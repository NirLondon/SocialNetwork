using System;
using System.Threading.Tasks;
using Client.Enum;

namespace Client.DataProviders
{
    public interface ISignupLoginDataProvider
    {
        Task<ErrorEnum> Login(string username, string password);
        Task<ErrorEnum> LoginWithFacebook(string facebookToken);
        Task<ErrorEnum> Signup(string username, string password);
        Task<ErrorEnum> SwitchToFacebookUser(string username, string password);
    }
}