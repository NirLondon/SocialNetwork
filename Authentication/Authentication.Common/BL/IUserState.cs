namespace Authentication.Common.BL
{
    public interface IUserState
    {
        void BlockUser(string username);

        void UnblockUser(string username);
    }
}
