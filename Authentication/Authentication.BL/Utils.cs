using System;

namespace Authentication.BL
{
    class Utils
    {
        public static string GetNewToken()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool IsValid(string username, string password)
        {
            return
                !string.IsNullOrEmpty(username)
                && !string.IsNullOrEmpty(password);
        }
    }
}
