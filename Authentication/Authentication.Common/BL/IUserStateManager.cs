namespace Authentication.Common.BL
{
    public interface IUserStateManager
    {
        void BlockUser(string username);

        void UnblockUser(string username);
    }
}
