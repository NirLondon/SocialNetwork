using Client.ServicesInterfaces;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Client.Models;
using Client.WUP.UserControls;
using Client.DataProviders;

namespace Client.WUP.Services
{
    public class PostService : IPostService
    {
        public FileOpenPicker picker { get; set; }

        public PostService()
        {
            InitPicker();
        }

        public async Task<byte[]> ChooseImage()
        {
            //BitmapImage image = null;
            StorageFile file = await picker.PickSingleFileAsync();
            byte[] arr = null;
            if (file != null)
            {
                //var stream = await file.OpenAsync(FileAccessMode.Read);
               // image = new BitmapImage();
               // image.SetSource(stream);
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        arr = memoryStream.ToArray();
                    }
                }
            }
            return arr;
        }

        public void GoToProfile(UserDetails userDetails, ISocialDataProvider dataProvider)
        {
            MainPageService.Instance.stackPanelContent.Children.Clear();
            MainPageService.Instance.stackPanelContent.Children.Add(new UserProfileUserControl(userDetails, dataProvider));
        }

        public void LogOut()
        {
            MainPageService.Instance.LogOut();
        }

        private void InitPicker()
        {
            picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
        }
    }
}
