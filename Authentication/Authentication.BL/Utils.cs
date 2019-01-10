using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.BL
{
    class Utils
    {
        public static string GetNewToken()
        {
            byte[] arr = new byte[16];
            new Random().NextBytes(arr);
            return Convert.ToBase64String(arr);
        }

        public static bool IsValid(string username, string password)
        {
            return
                !string.IsNullOrEmpty(username)
                && !string.IsNullOrEmpty(password);
        }
    }
}
