using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.Driver.V1;
using Social.Common.Models;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.UploadedDTOs;
using Social.DAL;

namespace Social.Tests
{
    [TestClass]
    public class DALTests
    {
        SocialRepository repo;
        User[] users;

        public DALTests()
        {
            repo = new SocialRepository();
            users = new User[3];
            for (int i = 0; i < users.Length; i++)
            {
                users[i] = new User
                {
                    UserId = $"user{i}",
                    FirstName = $"first{i}",
                    LastName = $"last{i}"
                };
            }
        }

        [TestMethod]
        public void AddUser()
        {
            for (int i = 0; i < users.Length; i++)
                repo.AddUser(users[i]);
        }

        [TestMethod]
        public void Follow()
        {
            for (int i = 0; i < users.Length; i++)
            {
                repo.SetFollow(users[i].UserId, users[(i + 1) % users.Length].UserId);
                repo.SetFollow(users[i].UserId, users[(i + 2) % users.Length].UserId);
            }
        }

        [TestMethod]
        public void Block()
        {
            for (int i = 0; i < users.Length; i++)
            {
                repo.Block(users[i].UserId, users[(i + 3) % users.Length].UserId);
                repo.Block(users[i].UserId, users[(i + 4) % users.Length].UserId);
            }
        }

        [TestMethod]
        public void Unblock()
        {
            for (int i = 0; i < users.Length; i++)
            {
                repo.Unblock(users[i].UserId, users[(i + 2) % users.Length].UserId);
                repo.Unblock(users[i].UserId, users[(i + 3) % users.Length].UserId);
            }
        }

        [TestMethod]
        public void Unfollow()
        {
            for (int i = 0; i < users.Length; i++)
            {
                repo.RemoveFollow(users[i].UserId, users[(i + 1) % users.Length].UserId);
                repo.RemoveFollow(users[i].UserId, users[(i + 2) % users.Length].UserId);
            }
        }

        [TestMethod]
        public void Followed()
        {
            var res = repo.UsersFollowedBy(users[0].UserId);
        }

        [TestMethod]
        public void Followers()
        {
            var res = repo.FollowersOf(users[0].UserId);
        }

        [TestMethod]
        public void BlockedBy()
        {
            var res = repo.BlockedBy(users[0].UserId);
        }

        [TestMethod]
        public void Post()
        {
            for (int i = 0; i < users.Length; i++)
            {
                repo.PutPost(users[i].UserId, new DataBasePost
                {
                    Content = $"post{i * 2}",
                    ImageURL = $"image{i * 2}",
                    TagedUsersIds = new[] { $"user{(i + 5) % users.Length}", $"user{(i + 6) % users.Length}" }
                });

                repo.PutPost(users[i].UserId, new DataBasePost
                {
                    Content = $"post{i * 2 + 1}",
                    ImageURL = $"image{i * 2 + 1}",
                    TagedUsersIds = new[] { $"user{(i + 4) % users.Length}", $"user{(i + 7) % users.Length}" }
                });

            }
        }

        [TestMethod]
        public void Comment()
        {
            var ids =Ids("Post");

            for (int i = 0; i < ids.Length; i++)
            {
                repo.PutComment(users[i / 2].UserId, new DataBaseComment
                {
                    ImagURL = $"imageURL{i}",
                    Content = $"comment{i}",
                    PostId = ids[i],
                    TagedUsersIds = new[] { $"user{(i + 4) % users.Length}", $"user{(i + 6) % users.Length}" }
                });

                repo.PutComment(users[i / 2].UserId, new DataBaseComment
                {
                    ImagURL = $"imageURL{i}",
                    Content = $"comment{i}",
                    PostId = ids[i],
                    TagedUsersIds = new[] { $"user{(i + 4) % users.Length}", $"user{(i + 6) % users.Length}" }
                });
            }
        }

        [TestMethod]
        public void LikePosts()
        {
            var posts = Ids("Post");

            for (int i = 0; i < users.Length; i++)
            {
                repo.Like(posts[i], users[(i + 1) % users.Length].UserId, LikeOptions.Post);
                repo.Like(posts[(i + 1) % users.Length], users[(i + 1) % users.Length].UserId, LikeOptions.Post);
            }
        }

        [TestMethod]
        public void LikeComments()
        {
            var comments = Ids("Comment");

            for (int i = 0; i < users.Length; i++)
            {
                repo.Like(comments[i], users[(i + 1) % users.Length].UserId, LikeOptions.Comment);
                repo.Like(comments[(i + 1) % users.Length], users[(i + 1) % users.Length].UserId, LikeOptions.Comment);
            }
        }

        [TestMethod]
        public void CommentsOfPost()
        {
            var x = repo.CommentsOfPost(Ids("Post")[0]);
        }

        [TestMethod]
        public void PostsForUser()
        {
            var x = repo.PostsForUser("user0", 20, 0);
        }

        private Guid[] Ids(string pc)
        {
            IDriver _driver =
                        GraphDatabase.Driver("bolt://34.244.74.143:7687", AuthTokens.Basic("neo4j", "123456"));

            using (var session = _driver.Session())
            {
                return
                session.Run(
                    "MATCH\n" +
                    $"\t(cp:{pc})\n" +
                    "RETURN\n" +
                    $"\t cp.{pc}ID AS g")
                    .Select(r => Guid.Parse(r["g"].As<string>()))
                    .ToArray();
            }
        }
    }
}
