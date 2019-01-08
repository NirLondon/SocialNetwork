using Client.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.DataProviders
{
    public interface IEditDetailsDataProvider
    {
        Task<Tuple<ErrorEnum, IDictionary<string, object>>> GetUserDetails();

        Task UpdateUserDetails(IDictionary<string, object> userDetails);
    }
}
