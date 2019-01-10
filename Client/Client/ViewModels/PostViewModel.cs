using Client.Common;
using Client.DataProviders;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.ViewModels
{
    public class PostViewModel
    {
        private IPostService _viwService { get; set; }
        private readonly ISocialDataProvider _dataProvider;
        public Post CurrentPost { get; set; }
        public string CommentText { get; set; }
        public byte[] Image { get; set; }


        public PostViewModel(IPostService service, ISocialDataProvider dataprovider)
        {
            _viwService = service;
            _dataProvider = dataprovider;
        }

        public void PublishComment()
        {
            Comment newComment = new Comment
            {
                Text = CommentText
            };

            CurrentPost.Comments.Add(newComment);
        }

        public async void ChooseImage()
        {
             Image = await _viwService.ChooseImage();
        }
    }
}
