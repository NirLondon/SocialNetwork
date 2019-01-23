using Client.Models;
using Client.DataProviders;
using Client.Enums;
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
using Client.Models.ReturnedDTOs;
using System.Collections.ObjectModel;
using Client.Models.UploadedDTOs;
using Client.Exeptions;

namespace Client.ViewModels
{
    public class PostViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IPostService _viewService { get; set; }
        private readonly ISocialDataProvider _socialDataProvider;
        private readonly IEditDetailsDataProvider _editDetailsDataProvider;
        public ReturnedPost CurrentPost { get; set; }
        public ObservableCollection<RetunredComment> Comments { get; set; }
        public ObservableCollection<UserMention> Followed { get; set; }
        private List<RetunredComment> _hiddenComments { get; set; }
        public string CommentText { get; set; }
        public byte[] Image { get; set; }
        public object Tags { get; set; }
        public int Likes { get; set; }
        public bool ShowComments { get; set; }

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
            CommentText = string.Empty;
            Image = new byte[32];
            Tags = new List<string>();
            ShowComments = false;
        }

        public async void PublishComment()
        {
            var tagsList = _viewService.TagUser(Tags);
            try
            {
                var comment = await _socialDataProvider.Comment(GenerateCommentToUpload(tagsList));
                Comments.Add(comment);
            }
            catch (TokenExpiredExeption e)
            {
                ExpiredToken();
            }
        }

        public void TagUser(object usersLIst)
        {
            Tags = usersLIst;
        }

        public async void ChooseImage()
        {
            Image = await _viewService.ChooseImage();
        }

        //public async void GoToProfile()
        //{
        //    try
        //    {
        //        var details = await _editDetailsDataProvider.GetUserDetails();
        //        _viewService.GoToProfile(details, _socialDataProvider);
        //    }
        //    catch (UnauthorizedAccessException e)
        //    {
        //        ExpiredToken();
        //    }

        //}

        public async void Like()
        {
            try
            {
                if (CurrentPost.IsLiked)
                {
                    await _socialDataProvider.LikePost(CurrentPost.PostId);
                    Likes--;
                }
                else
                {
                    await _socialDataProvider.DisLikePost(CurrentPost.PostId);
                    Likes++;
                }
                CurrentPost.IsLiked = !CurrentPost.IsLiked;
            }
            catch (TokenExpiredExeption e)
            {
                ExpiredToken();
            }
        }

        public async void ExpandComments()
        {
            if (ShowComments)
            {
                Comments.Clear();
            }
            else
            {
                if (Comments == null)
                {
                    _hiddenComments = new List<RetunredComment>();
                    Comments = new ObservableCollection<RetunredComment>();
                    try
                    {
                        _hiddenComments = await _socialDataProvider.GetComments(CurrentPost.PostId);
                    }
                    catch (TokenExpiredExeption e)
                    {
                        ExpiredToken();
                    }
                }
                foreach (var comment in _hiddenComments)
                {
                    Comments.Add(comment);
                }
            }
            ShowComments = !ShowComments;
        }

        private void ExpiredToken()
        {
            _viewService.LogOut();
        }

        private UploadedComment GenerateCommentToUpload(List<string> tagsList)
        {
            UploadedComment comment = new UploadedComment
            {
                Content = CommentText,
                Image = Image,
                TagedUsersIds = tagsList.ToArray(),
                PostId = CurrentPost.PostId
            };
            return comment;
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
