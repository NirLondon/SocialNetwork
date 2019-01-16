using Client.Models;
using Client.DataProviders;
using Client.Enum;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.ViewModels
{
    public class PostViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IPostService _viewService { get; set; }
        private readonly ISocialDataProvider _socialDataProvider;
        private readonly IEditDetailsDataProvider _editDetailsDataProvider;
        public Post CurrentPost { get; set; }
        public string CommentText { get; set; }
        public byte[] Image { get; set; }
        public List<string> Tags { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChange(); }
        }


        public PostViewModel(IPostService service, ISocialDataProvider dataprovider, IEditDetailsDataProvider editDetailsDataProvider)
        {
            _viewService = service;
            _socialDataProvider = dataprovider;
            _editDetailsDataProvider = editDetailsDataProvider;
        }

        public async void PublishComment()
        {
            try
            {
                var comment = await _socialDataProvider.PublishComment(CommentText, Image, Tags.ToArray());
                CurrentPost.Comments.Add(comment);
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredToken();
            }
        }

        public async void ChooseImage()
        {
            Image = await _viewService.ChooseImage();
        }

        public async void GoToProfile()
        {
            try
            {
                var details = await _editDetailsDataProvider.GetUserDetails();
                _viewService.GoToProfile(details, _socialDataProvider);
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredToken();
            }

        }

        public async void Like()
        {
            try
            {
                await _socialDataProvider.LikePost(CurrentPost.ID);
                CurrentPost.Likes++;
                CurrentPost.DidLiked = true;
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredToken();
            }
        }

        private void ExpiredToken()
        {
             _viewService.LogOut();
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
