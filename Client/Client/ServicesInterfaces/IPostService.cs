using Client.DataProviders;
using Client.Models;
using Client.Models.ReturnedDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.ServicesInterfaces
{
    public interface IPostService
    {
        Task<byte[]> ChooseImage();

        void GoToProfile(UserMention userDetails, ISocialDataProvider dataProvider);

        void LogOut();

        List<string> TagUser(object list);

        bool PostsScrolled(object scroll);
    }
}
