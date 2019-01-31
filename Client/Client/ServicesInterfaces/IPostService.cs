using Client.DataProviders;
using Client.Models;
using System.Threading.Tasks;

namespace Client.ServicesInterfaces
{
    public interface IPostService
    {
        Task<byte[]> ChooseImage();

        void GoToProfile(UserDetails userDetails, ISocialDataProvider dataProvider);

        void LogOut();
    }
}
