using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Social.Common.Models;
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
            users = new User[15];
            for (int i = 0; i < users.Length; i++)
            {
                users[i] = new User
                {
                    UserId = $"user{i}",
                    FirstName = $"first{i}",
                    LastName = $"last{4}"
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
    }
}
