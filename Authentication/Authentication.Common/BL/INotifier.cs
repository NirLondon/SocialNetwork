namespace Authentication.Common.BL
{
    public interface INotifier
    {
        string Token { get; set; }

        void NotifyToSocailService(string userId);

        void NotifyToIdentityService(string userId);
    }
}
