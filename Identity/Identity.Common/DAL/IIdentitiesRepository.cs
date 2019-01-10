using Identity.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Common.DAL
{
    public interface IIdentitiesRepository
    {
        Task<UserDetails> GetUserDetailsAsync(string userId);

        Task EditUser(string userId, UserDetails userDetails);

        void AddUser(string userId);
    }
}
