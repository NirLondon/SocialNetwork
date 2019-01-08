using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Common.DAL
{
    public interface IIdentitiesRepository
    {
        Task<string> GetDetailsJsonAsync(string userId);

        Task EditUser(string userId, Dictionary<string, object> userDetails);

        void AddUser(string userId);
    }
}
