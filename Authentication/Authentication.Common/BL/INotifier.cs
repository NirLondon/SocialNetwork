namespace Authentication.Common.BL
{
    public interface INotifier
    {
        void NotifyToSocailService(string userId, string token);

        void NotifyToIdentityService(string userId, string token);
    }
}
