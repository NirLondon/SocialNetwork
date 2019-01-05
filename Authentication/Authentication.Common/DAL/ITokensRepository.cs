using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.DAL
{
    public interface ITokensRepository
    {
        bool Exists(string token);

        string UserIdOfTokenOrNull(string token);
    }
}
