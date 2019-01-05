using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.BL
{
    public interface IValidateToken
    {
        string ValidateToken(string token);
    }
}
