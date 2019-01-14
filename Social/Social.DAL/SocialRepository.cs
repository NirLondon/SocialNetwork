using Neo4j.Driver.V1;
using Social.Common.DAL;
using Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Social.DAL
{
    public class SocialRepository : ISocialRepository
    {
        private readonly IDriver _driver = 
            GraphDatabase.Driver("bolt://34.244.74.143:7687", AuthTokens.Basic("neo4j", "123456"));


        private async Task<TResult> RunQueryAsync<TResult>(string query, Dictionary<string, object> parameters)
        {
            using (var session = _driver.Session())
            {
                var result = await session.RunAsync(query, parameters);

                IRecord current;
                while (await result.FetchAsync())
                {
                    current = result.Current;

                
                }
            }
        }

        public void SetFollow(string followerId, int followedId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["userId"] = 
            }

            var query = "CREATE (:User{UserID: \"{userId}\", firstName: \"{firstName}\"}, lastName: \"{lastName}\")";

            using (var session = _driver.Session())
            {
                var result = session.RunAsync(query, parameters);
            }
        }

        public IEnumerable<User> BlockedBy(string userId)
        {
            throw new NotImplementedException();
        }

        public void PutComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<User> UsersFollowedBy(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FollowersOf(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Post> PostsForUser(string userId, int amount, int skip)
        {
            throw new NotImplementedException();
        }

        public void PutPost(string userId, Post post)
        {
            throw new NotImplementedException();
        }

        public void RemoveFollow(string followerId, int followedId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SearchResultUser> Search(string searchedUsername)
        {
            throw new NotImplementedException();
        }
    }
}
