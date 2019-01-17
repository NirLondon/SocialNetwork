using Client.Enum;
using Client.Models;
using System.Threading.Tasks;

namespace Client.DataProviders
{
    public interface IEditDetailsDataProvider
    {
        Task<UserDetails> GetUserDetails();

        Task UpdateUserDetails(UserDetails userDetails);
    }
}
