using System.Linq;
using Client.DataProviders;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;
using Client.Exeptions;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class FeedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IPostService _viewService { get; set; }
        private readonly ISocialDataProvider _socialDataProvider;
        private readonly IEditDetailsDataProvider _editDetailsDataProvider;

        private ObservableCollection<PostViewModel> _postViewModels = new ObservableCollection<PostViewModel>();
        public ObservableCollection<PostViewModel> PostViewModels
        {
            get => _postViewModels;
            set
            {
                _postViewModels = value;
                OnPropertyChange();
            }
        }

        public string PostText { get; set; } = "";
        private byte[] Image { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public ObservableCollection<string> PostVisibility { get; set; } =
            new ObservableCollection<string>(Enum.GetNames(typeof(PostVisibility)));
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
            InitPosts();
        }

        public async void ChooseImage()
        {
            Image = await _viewService.ChooseImage();
        }

        public async void PublishPost()
        {
            try
            {
                var post = await _socialDataProvider.Post(GeneratePostToUpload());
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

        private async Task InitPosts()
        {
            try
            {
                var posts =
                    (await _socialDataProvider.GetPosts())
                    ?.Select(p =>
                       new PostViewModel(_viewService, _socialDataProvider, _editDetailsDataProvider)
                       {
                           CurrentPost = p
                       });

                if (posts != null)
                    PostViewModels = new ObservableCollection<PostViewModel>(posts);
            }
            catch (TokenExpiredExeption e)
            {
                ExpiredToken();
            }
        }

        private void ExpiredToken()
        {
            _viewService.LogOut();
        }

        private UploadedPost GeneratePostToUpload()
        {
            UploadedPost post = new UploadedPost
            {
                Content = PostText,
                Image = Image,
                TagedUsersIds = Tags.ToArray()
            };
            return post;
        }

        private void OnPropertyChange(string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
