using Client.DataProviders;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Social.Common.Models.ReturnedDTOs;
using System.Collections.ObjectModel;

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
        public string CommentText { get; set; }
        public byte[] Image { get; set; }
        public List<string> Tags { get; set; }
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
            Comments = new ObservableCollection<RetunredComment>();
        }

        public async void PublishComment()
        {
            try
            {
                var comment = await _socialDataProvider.Comment(CommentText, Image, Tags.ToArray());
                Comments.Add(comment);
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
            catch (UnauthorizedAccessException e)
            {
                ExpiredToken();
            }
        }

        public async void ExpandComments()
        {
            if (ShowComments) Comments.Clear();
            else
            {
                try
                {
                    Comments = new ObservableCollection<RetunredComment>(
                        await _socialDataProvider.GetComments(CurrentPost.PostId));
                }
                catch (UnauthorizedAccessException e)
                {
                    ExpiredToken();
                }
            }
            ShowComments = !ShowComments;
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
