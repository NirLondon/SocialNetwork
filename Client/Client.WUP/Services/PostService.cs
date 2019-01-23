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
using Client.ViewModels;
using Client.Models;
using Windows.UI.Xaml;
using Client.WUP.UserControls;
using Client.DataProviders;
using Windows.UI.Xaml.Controls;
using Client.Models.ReturnedDTOs;
using Windows.UI.Xaml.Controls;

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

        public void GoToProfile(UserMention user, ISocialDataProvider dataProvider)
        {
            MainPageService.Instance.stackPanelContent.Children.Clear();
            MainPageService.Instance.stackPanelContent.Children.Add(new UserProfileUserControl(user, dataProvider));
        }

        public void LogOut()
        {
            MainPageService.Instance.LogOut();
        }

        public List<string> TagUser(object tagsList)
        {
            List<string> l = new List<string>();
            try
            {
                foreach (UserMention user in ((ListBox)tagsList).SelectedItems)
                {
                    l.Add(user.UserId);
                }
                return l;
            }
            catch (Exception e)
            {

            }
            return l;
        }

        private void InitPicker()
        {
            picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
        }

        public bool PostsScrolled(object scroll)
        {
            var scroller = (ScrollViewer)scroll;
            if (scroller.VerticalOffset == scroller.Height)
                return true;
            return false;
        }
    }
}
