using Identity.Common.Models;

namespace Identity.Common.BL
{
    public interface INotifier
    {
        string Token { get; set; }

        void NotifyToSocialService(UserToSocial user);
    }
}
