namespace Authentication.Common.BL
{
    public interface ITokensValidator
    {
        (string Token, string UserId) ValidateToken(string token);
    }
}
