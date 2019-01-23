﻿using Social.Common.BL;
using Social.Common.DAL;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;
using System;
using System.Collections.Generic;

namespace Social.BL
{
    public class SocialManager : ISocialManager
    {
        private readonly ISocialRepository _repository;
        private readonly IPhotosStorage _photosStorage;
        private readonly INotifier _notifier;

        public SocialManager(ISocialRepository repository, IPhotosStorage photosStorage, INotifier notifier)
        {
            _repository = repository;
            _photosStorage = photosStorage;
            _notifier = notifier;
        }

        public void AddUser(string userId)
        {
            _repository.AddUser(userId);
        }

        public void Block(string blockingId, string blockedId)
        {
            _repository.Block(blockingId, blockedId);
        }

        public IEnumerable<UserMention> BlockedBy(string userId)
        {
            return _repository.BlockedBy(userId);
        }

        public void Comment(string commenterId, UploadedComment comment)
        {
            string photoURL = null;
            if (comment.Image != null)
                _photosStorage.UploadPhoto(comment.Image, out photoURL);

            _repository.PutComment(commenterId, new DataBaseComment
            {
                Content = comment.Content,
                ImagURL = photoURL,
                PostId = comment.PostId,
                TagedUsersIds = comment.TagedUsersIds
            });
        }

        public IEnumerable<RetunredComment> CommentsOf(string userId, Guid postId)
        {
            return _repository.CommentsOfPost(postId, userId);
        }

        public void EditUser(User user)
        {
            _repository.EditUser(user);
        }

        public IEnumerable<UserMention> GetFollowedBy(string userId)
        {
            return _repository.UsersFollowedBy(userId);
        }

        public IEnumerable<UserMention> GetFollowersOf(string userId)
        {
            return _repository.FollowersOf(userId);
        }

        public IEnumerable<ReturnedPost> GetPostsFor(string userId, int amount, int skip)
        {
            return _repository.PostsForUser(userId, amount, skip);
        }

        public void Like(Guid id, string likerId, LikeOptions likeOption)
        {
            _repository.Like(id, likerId, likeOption);
        }

        public void Post(string userId, UploadedPost post)
        {
            string photoURL = null;
            if (post.Image != null)
                _photosStorage.UploadPhoto(post.Image, out photoURL);

            _repository.PutPost(userId, new DataBasePost
            {
                Content = post.Content,
                ImageURL = photoURL,
                TagedUsersIds = post.TagedUsersIds,
                Visibility = post.Visibility
            });
        }

        public void Remove(Guid id, string uploaderId, RemoveOptions removeOption)
        {
            _repository.Remove(id, uploaderId, removeOption);
        }

        public void RemoveFollow(string followerId, string followedId)
        {
            _repository.RemoveFollow(followerId, followedId);
        }

        public IEnumerable<UserMention> Search(string searchedUsername)
        {
            return _repository.Search(searchedUsername);
        }

        public void SetFollow(string followerId, string followedId)
        {
            _repository.SetFollow(followerId, followedId);
        }

        public void Unblock(string blockingId, string blockedId)
        {
            _repository.Unblock(blockingId, blockedId);
        }

        public void Unlike(Guid id, string likerId, LikeOptions likeOption)
        {
            _repository.Unlike(id, likerId, likeOption);
        }
    }
}
