using Identity.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Common.DAL
{
    public interface IIdentitiesRepository
    {
        Task<UserDetails> GetIdentityAsync(string userId);

        void EditUser(Dictionary<string, object> userDetails);

        void AddUser(string userId);
    }
}
