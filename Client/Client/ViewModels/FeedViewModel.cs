using Client.Common;
using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels
{
    public class FeedViewModel
    {
        public IPostService _viewService { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public ObservableCollection<PostViewModel> PostViewModels { get; set; }
        public string CommentText { get; set; }

        public FeedViewModel(IPostService service)
        {
            _viewService = service;
            Posts = new ObservableCollection<Post>();
            Post p1 = new Post()
            {
                Publisher = "oded",
                IMGURL = "https://static.boredpanda.com/blog/wp-content/uploads/2018/04/5acb63d83493f__700-png.jpg",
                Text = "this is the new post. bla bla"
            };
            Post p2 = new Post()
            {
                Publisher = "nir",
                IMGURL = "https://media.mnn.com/assets/images/2018/07/cat_eating_fancy_ice_cream.jpg.838x0_q80.jpg",
                Text = "this is nir post, just another post"
            };
            Posts.Add(p1);
            Posts.Add(p2);

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
    }
}
