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
        private IPostService _viwService { get; set; }
        private readonly ISocialDataProvider _dataProvider;
        public Post CurrentPost { get; set; }
        public string CommentText { get; set; }
        public byte[] Image { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChange(); }
        }


        public PostViewModel(IPostService service, ISocialDataProvider dataprovider)
        {
            _viwService = service;
            _dataProvider = dataprovider;
        }

        public async void PublishComment()
        {
            var tuple = await _dataProvider.PublishComment(CommentText, Image);
            if (tuple.Item1 == ErrorEnum.EverythingIsGood)
                CurrentPost.Comments.Add(tuple.Item2);
            else
                ManageError(tuple.Item1);
        }

        public async void ChooseImage()
        {
            Image = await _viwService.ChooseImage();
        }

        public async void GoToProfile()
        {
            var response = await _dataProvider.GetUserDetails(CurrentPost.Publisher);
            if (response.Item1 == ErrorEnum.EverythingIsGood)
            {
                _viwService.GoToProfile(response.Item2, _dataProvider);
            }
            else
            {
                ManageError(response.Item1);
            }
        }

        public async void Like()
        {
            var result = await _dataProvider.LikePost(CurrentPost.ID);
            if (result == ErrorEnum.EverythingIsGood)
            {
                CurrentPost.Likes++;
                CurrentPost.DidLiked = true;
            }
            else
                ManageError(result);
        }


        private void ManageError(ErrorEnum eror)
        {
            switch (eror)
            {
                case ErrorEnum.WrongUsernameOrPassword:
                    Message = "Wrong username or password";
                    return;
                case ErrorEnum.ConectionFailed:
                    Message = "Bad internet conection";
                    return;
                case ErrorEnum.UsernameAlreadyExist:
                    Message = "Username already exist";
                    return;
            }
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
