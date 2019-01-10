using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using System.Drawing;
using System.IO;

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

        private void InitPicker()
        {
            picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
        }
    }
}
