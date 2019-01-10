using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.WUP.Services
{
    public class PostService : IPostService
    {
        public FileOpenPicker picker { get; set; }


        public PostService()
        {
            InitPicker();
        }

        public async Task<BitmapImage> ChooseImage()
        {
            Image image;
            BitmapImage Bitimage = null;
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
               // image = Image.fromfile
                var stream = await file.OpenAsync(FileAccessMode.Read);
                Bitimage = new BitmapImage();
                Bitimage.SetSource(stream);
            }
            return Bitimage;
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
