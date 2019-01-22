using Client.Models;
using Client.DataProviders;
using Client.Enums;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using Client.Models.UploadedDTOs;
using Client.Models.ReturnedDTOs;

namespace Client.ViewModels
{
    public class FeedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IPostService _viewService { get; set; }
        private readonly ISocialDataProvider _socialDataProvider;
        private readonly IEditDetailsDataProvider _editDetailsDataProvider;
        public ObservableCollection<ReturnedPost> Posts { get; set; }
        public ObservableCollection<PostViewModel> PostViewModels { get; set; }
        public ObservableCollection<UserMention> Followed { get; set; }
        public string PostText { get; set; } = "";
        private byte[] Image { get; set; }
        public object Tags { get; set; }
        public ObservableCollection<string> PostVisibility { get; set; }
        public int VisibilityIndex { get; set; } = -1;

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChange(); }
        }


        public FeedViewModel(IPostService service, ISocialDataProvider socialDataProvider, IEditDetailsDataProvider editDetailsDataProvider)
        {
            _viewService = service;
            _socialDataProvider = socialDataProvider;
            _editDetailsDataProvider = editDetailsDataProvider;
            InitFollowed();
            InitPosts();
        }


        public async void ChooseImage()
        {
            Image = await _viewService.ChooseImage();
        }

        public async void PublishPost()
        {
            var tagsList = _viewService.TagUser(Tags);
            try
            {
                var post = await _socialDataProvider.Post(GeneratePostToUpload(tagsList));
                PostViewModels.Add(new PostViewModel(_viewService, _socialDataProvider, _editDetailsDataProvider) { CurrentPost = post });
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredToken();
            }
        }

        public void SelectVisibility(object sender, object e)
        {

        }

        public void TagUser(object usersLIst, object e)
        {
            Tags = usersLIst;
        }

        private async void InitPosts()
        {
            UserMention u1 = new UserMention
            {
                FullName = "oded bartov",
                UserId = "1234"
            };
            UserMention u2 = new UserMention
            {
                FullName = "nir london",
                UserId = "5678"
            };
            Tags = new List<string>();
            Posts = new ObservableCollection<ReturnedPost>();
            ReturnedPost p1 = new ReturnedPost()
            {
                Poster = u1,
                ImageURL = "https://static.boredpanda.com/blog/wp-content/uploads/2018/04/5acb63d83493f__700-png.jpg",
                Content = "this is the new post. bla bla",
                UploadingTime = DateTime.Now,
                IsLiked = true,
                PostId = new Guid(new byte[16])
            };
            ReturnedPost p2 = new ReturnedPost()
            {
                Poster = u2,
                ImageURL = "https://media.mnn.com/assets/images/2018/07/cat_eating_fancy_ice_cream.jpg.838x0_q80.jpg",
                Content = "this is nir post, just another post",
                UploadingTime = DateTime.Now - new TimeSpan(100, 0, 0, 0, 0),
                IsLiked = false,
                PostId = new Guid(new byte[16])
            };
            Posts.Add(p1);
            Posts.Add(p2);

            PostVisibility = new ObservableCollection<string>();
            var arr = Enum.GetNames(typeof(PostVisibility));
            foreach (var item in arr)
            {
                PostVisibility.Add(item);
            }
            InitPostsViewModels(Posts);

            //var tuple = await _dataProvider.GetPosts();
            //if (tuple.Item1 == ErrorEnum.EverythingIsGood)
            //    InitPostsViewModels(tuple.Item2);
            //else
            //    ManageError(tuple.Item1);
        }

        private async void InitFollowed()
        {
            try
            {
                var _followed = await _socialDataProvider.GetFollowed();
                Followed = new ObservableCollection<UserMention>();
                foreach (var item in _followed)
                {
                    Followed.Add(item);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                ExpiredToken();
            }
        }

        private void InitPostsViewModels(IEnumerable<ReturnedPost> postsList)
        {
            PostViewModels = new ObservableCollection<PostViewModel>();
            foreach (var item in postsList)
            {
                PostViewModels.Add(new PostViewModel(_viewService, _socialDataProvider, _editDetailsDataProvider) { CurrentPost = item, Followed = this.Followed });
            }
        }

        private void ExpiredToken()
        {
            _viewService.LogOut();
        }

        private UploadedPost GeneratePostToUpload(List<string> tagsList)
        {
            UploadedPost post = new UploadedPost
            {
                Content = PostText,
                Image = Image, 
                TagedUsersIds = tagsList.ToArray()
            };
            return post;
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
