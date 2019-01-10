using Client.Common;
using Client.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.DataProviders
{
    public interface ISocialDataProvider
    {
        Task<Tuple<ErrorEnum, Post>> PublishPost(string text, byte[] arr);
    }
}
