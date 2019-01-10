using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServicesInterfaces
{
    public interface ISignupLoginService
    {
        void NavigateToMainPage();

        Task<string> LoginWithFacebook();

        Task<bool> SwitchToFacebookMessage();
    }
}
