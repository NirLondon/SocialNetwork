using Client.Common;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.ViewModels
{
    public class FeedViewModel
    {
        public IPostService _viewService { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public ObservableCollection<PostViewModel> PostViewModels { get; set; }
        public string PostText { get; set; }
        public BitmapImage Image { get; set; }

        public FeedViewModel(IPostService service)
        {
            _viewService = service;
            InitPosts();
            InitPostsViewModels();
        }
       

        private void InitPostsViewModels()
        {
            PostViewModels = new ObservableCollection<PostViewModel>();
            foreach (var item in Posts)
            {
                PostViewModels.Add(new PostViewModel(_viewService) { CurrentPost = item});
            }
        }

        private void InitPosts()
        {
            Posts = new ObservableCollection<Post>();
            Post p1 = new Post()
            {
                Publisher = "oded",
                IMGURL = "https://static.boredpanda.com/blog/wp-content/uploads/2018/04/5acb63d83493f__700-png.jpg",
                Text = "this is the new post. bla bla",
                PublishDate = DateTime.Now,
                Likes = 20
            };
            Post p2 = new Post()
            {
                Publisher = "nir",
                IMGURL = "https://media.mnn.com/assets/images/2018/07/cat_eating_fancy_ice_cream.jpg.838x0_q80.jpg",
                Text = "this is nir post, just another post",
                PublishDate = DateTime.Now - new TimeSpan(100, 0, 0, 0, 0),
                Likes = 12
            };
            Posts.Add(p1);
            Posts.Add(p2);

            Comment c1 = new Comment()
            {
                PublishDate = DateTime.Now,
                Text = "just a comment without an image"
            };
            p1.Comments.Add(c1);
        }
    }
}
