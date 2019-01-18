namespace Authentication.Common.BL
{
    public interface INotifier
    {
        void NotifyToSocailService(string username);

        void NotifyToIdentityService(string username);
    }
}
