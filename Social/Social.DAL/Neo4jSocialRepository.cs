using Neo4j.Driver.V1;
using Newtonsoft.Json;
using Social.Common.DAL;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Social.DAL
{
    public class Neo4jSocialRepository : ISocialRepository
    {
        private readonly IDriver _driver =
            GraphDatabase.Driver("bolt://34.244.74.143:7687", AuthTokens.Basic("neo4j", "123456"));

        public void SetFollow(string followerId, string followedId)
        {
            var query = "MATCH (u:User{ UserID:\"" + followerId + "\"}), (u2:User{UserID:\"" + followedId + "\"}) " +
                        "MERGE(u)-[:Follows]->(u2)";

            using (var session = _driver.Session())
                session.Run(query);
        }

        public IEnumerable<UserMention> BlockedBy(string blockerId)
        {
            using (var session = _driver.Session())
            {
                return
                    session.Run(
                        $"MATCH (u:User)<-[:Blocks]-(:User{{ UserID: \"{blockerId}\" }}) " +
                        "RETURN u{ UserID: u.UserID, FullName: u.FirstName + \" \" + u.LastName } AS Blocked")
                        .Select(record => ConvertTo<UserMention>(
                            record["Blocked"]
                            .As<Dictionary<string, object>>()));
            }
        }

        public void PutComment(string commenterId, DataBaseComment comment)
        {
            var tagedUsersMatch = string.Join(",\n",
                    comment.TagedUsersIds
                    .Select((userId, i) => $"(t{i}:User{{ UserID: \"{userId}\" }})"));

            var tagsCreate = string.Join(",\n",
                    comment.TagedUsersIds
                    .Select((userId, i) => $"(t{i})<-[:Tags]-(c)"));

            bool hasTags = comment.TagedUsersIds.Any();

            using (var session = _driver.Session())
            {
                var x = session.Run(
                   "MATCH " +
                   $"(p: Post{{ PostID: \"{comment.PostId}\" }}), " +
                   $"(u: User{{ UserID: \"{commenterId}\" }}) " +
                   (hasTags ? ", " + tagedUsersMatch + "\n" : string.Empty) +
                   "CREATE " +
                   "(c: Comment{ CommentID: \"" + $"{Guid.NewGuid()}\", Content: \"{comment.Content}\"" +
                   (comment.ImagURL != null ? $", ImageURL: \"{comment.ImagURL}\"" : string.Empty) +
                   $", UploadingTime: \"{DateTime.Now}\"" + " }), " +
                   "(c) < -[:Has] - (p), " +
                   "(c) -[:Uploaded_by]->(u) " +
                   (hasTags ? ", " + tagsCreate : string.Empty));
            }
        }

        public IEnumerable<UserMention> UsersFollowedBy(string userId)
        {
            using (var session = _driver.Session())
            {
                return
                session.Run(
                    "MATCH (:User{ UserID: \"" + userId + "\"})-[:Follows]->(u:User) " +
                    "RETURN { UserID: u.UserID, FullName: u.FirstName + \" \" + u.LastName } AS Followed")
                    .Select(record => ConvertTo<UserMention>(
                        record["Followed"]
                        .As<Dictionary<string, object>>()));
            }
        }

        public IEnumerable<UserMention> FollowersOf(string userId)
        {
            using (var session = _driver.Session())
            {
                return
                    session.Run(
                        "MATCH (:User{ UserID: \"" + userId + "\" })<-[:Follows]-(u:User) " +
                        "RETURN { UserID: u.UserID, FullName: u.FirstName + \" \" + u.LastName } AS Follower")
                    .Select(record => ConvertTo<UserMention>(
                        record["Follower"]
                        .As<Dictionary<string, object>>()));
            }
        }

        public IEnumerable<ReturnedPost> PostsForUser(string userId, int amount, int skip)
        {
            using (var session = _driver.Session())
            {
                return
                    session.Run(
@"MATCH
    (u: User{ UserID: " + '"' + userId + '"' + @"}), (p: Post) < -[:Uploaded] - (poster: User)
WHERE
	(poster)<-[:Follows]-(u)
    OR
	(p)-[:Tags]->(u)
    OR
    (p)-[:Has]->(:Comment)-[:Tags]->(u)
RETURN    
    	p.PostID AS PostId,
           	{
               	UserId: poster.UserID,
                FullName: poster.FirstName + ' ' + poster.LastName
            } AS Poster,
        p.Content AS Content,
        p.UploadingTime AS UploadingTime,
        p.ImageURL AS ImageURL,
        EXISTS((p)-[:Liked_by]->(u)) AS IsLiked,
        [(p)-[:Liked_by]->(liker:User) |
           			{
                       	UserId: liker.UserID,
                        FullName: liker.FirstName + ' ' + liker.LastName
                    }] AS Likes,
        [(p)-[:Tags]->(taged:User) |
           			{
                  		 UserId: taged.UserID,
                   		 FullName: taged.FirstName + ' ' + taged.LastName
                    }] AS Tags
ORDER BY p.UploadingTime" +
$"\nSKIP {skip}\n" +
$"LIMIT {amount}")
                        .Select(record =>
                            new ReturnedPost
                            {
                                PostId = Guid.Parse(record["PostId"].ToString()),
                                Poster = ConvertTo<UserMention>(record["Poster"].As<Dictionary<string, object>>()),
                                Content = record["Content"]?.ToString(),
                                ImageURL = record["ImageURL"]?.ToString(),
                                IsLiked = bool.Parse(record["IsLiked"].ToString()),
                                UploadingTime = new DateTime(long.Parse(record["UploadingTime"].ToString())),
                                Likes = ConvertTo<UserMention[]>(record["Likes"].As<List<Dictionary<string, object>>>()),
                                Tags = ConvertTo<UserMention[]>(record["Tags"].As<List<Dictionary<string, object>>>()),
                            });
            }
        }

        private (string tagedUsersMatch, string tagsCreate) PutPostQueryComponents(DataBasePost post)
        {
            string tagedUsersMatch, tagsCreate;
            tagedUsersMatch = "\n\t";
            tagsCreate = string.Empty;

            if (post.TagedUsersIds != null && post.TagedUsersIds.Any())
            {
                tagedUsersMatch = ",\n\t" + string.Join(",\n\t",
                    post.TagedUsersIds
                    .Select((userId, i) => $"(t{i}:User{{ UserID: \"{userId}\" }})")) + "\n";

                tagsCreate = ",\n\t" + string.Join(",\n\t",
                    post.TagedUsersIds
                    .Select((userId, i) => $"(t{i})<-[:Tags]-(p)"));
            }
            return (tagedUsersMatch, tagsCreate);
        }

        private string PutPostQuery(string posterId, DataBasePost post, Guid postId, DateTime uploadingTime)
        {
            var (tagedUsersMatch, tagsCreate) = PutPostQueryComponents(post);
            return
            $"MATCH\n\t(u:User{{ UserID: \"{posterId}\" }})" +
            tagedUsersMatch +
            $"CREATE\n" +
            "\t(p:Post\n\t{\n" +
            $"\t\t\tPostID: \"{postId}\",\n" +
            $"\t\t\tContent:\n\t\t\t\"{post.Content}\",\n" +
            $"\t\t\tUploadingTime: {uploadingTime.Ticks},\n" +
            (post.ImageURL != null ? $"\t\t\tImageURL: \"{post.ImageURL}\",\n" : string.Empty) +
            $"\t\t\tVisibility: \"{(byte)post.Visibility}\"\n" +
            "\t}),\n" +
            "\t(u)-[:Uploaded]->(p) " +
            tagsCreate +
            "RETURN " +
            "u.FirstName + \" \" + u.LastName AS FullName," +
            "[(t:user)<-[:Tags]-(p) | { UserId: t.UserID, FullName: t.FirstName + \" \" + t.LastName }] AS Tags";
        }

        public ReturnedPost PutPost(string posterID, DataBasePost post)
        {
            Guid postId = Guid.NewGuid();
            var uploadingTime = DateTime.Now;

            using (var session = _driver.Session())
            {
                var queryResult = session.Run(PutPostQuery(posterID, post, postId, uploadingTime))
                    .FirstOrDefault();

                return new ReturnedPost
                {
                    Content = post.Content,
                    ImageURL = post.ImageURL,
                    IsLiked = false,
                    Likes = null,
                    Poster = new UserMention
                    {
                        UserId = posterID,
                        FullName = queryResult["FullName"].ToString()
                    },
                    PostId = postId,
                    Tags = ConvertTo<UserMention[]>(queryResult["Tags"].As<List<Dictionary<string, object>>>()),
                    UploadingTime = uploadingTime
                };
            }
        }

        public void RemoveFollow(string followerId, string followedId)
        {
            var query =
                "MATCH (:User{ UserID: \"" + followerId + "\"})-[f:Follows]->(:User{ UserID: \"" + followedId + "\"}) " +
                "DELETE f";

            using (var session = _driver.Session())
                session.Run(query);
        }

        public IEnumerable<UserMention> Search(string searchedUsername)
        {
            using (var session = _driver.Session())
            {
                return
                    session.Run(
                        "MATCH(u:User)\n" +
                        "WITH(u.firstName + \" \" + u.lastName) AS FullName, u.UserID AS UserId\n" +
                        $"WHERE FullName CONTAINS \"{searchedUsername}\"\n" +
                        "RETURN UserId, FullName")
                        .Select(record =>
                            new UserMention
                            {
                                UserId = record["UserId"].ToString(),
                                FullName = record["FullName"].ToString()
                            });
            }
        }

        public void AddUser(string userId)
        {
            var query = $"CREATE (:User{{ UserID: \"{userId}\" }})";

            using (var session = _driver.Session())
                session.Run(query);
        }

        public void Block(string blockingId, string blockedId)
        {
            var query =
                "MATCH (u1:User{ UserID: \"" + blockingId + "\" }), (u2:User{ UserID: \"" + blockedId + "\" })" +
                "MERGE (u2)<-[:Blocks]-(u1)";

            using (var session = _driver.Session())
                session.Run(query);
        }

        public void Unblock(string blockingId, string blockedId)
        {
            var query =
                "MATCH (:User{ UserID: \"" + blockingId + "\" })-[b:Blocks]->(:User{ UserID: \"" + blockedId + "\" }) " +
                "DELETE b";

            using (var session = _driver.Session())
                session.Run(query);
        }

        public IEnumerable<RetunredComment> CommentsOfPost(Guid postId, string userId)
        {
            using (var session = _driver.Session())
            {
                return
                     session.Run(
                            "MATCH\n" +
                                $"\t(:Post{{ PostID: \"{postId}\" }})-[:Has]->(comment: Comment)-[:Uploaded_by]->(u: User)\n" +
                            "RETURN\n" +
                                "\tcomment,\n" +
                                "\t{ UserId: u.UserID, FullName: u.firstName + \" \" + u.lastName } AS commenter,\n" +
                                "\t[(comment)-[:Tags]->(t) | { UserId : t.UserID, FullName : t.firstName + \" \" + t.lastName }] AS taged,\n" +
                                "\t[(comment)-[:Liked_by]->(l) | { UserId : l.UserID, FullName : l.firstName + \" \" + l.lastName }] AS likes,\n" +
                                $"\tEXISTS((:User{{ UserID: \"{userId}\" }})<-[:Liked_by]-(comment)) AS IsLiked")
                            .Select(r =>
                           {
                               var comment = r["comment"].As<INode>();
                               return new RetunredComment
                               {
                                   CommentId = Guid.Parse(comment["CommentID"].ToString()),
                                   Content = comment["Content"].ToString(),
                                   ImageURL = comment["ImageURL"].ToString(),
                                   IsLiked = bool.Parse(r["IsLiked"].ToString()),
                                   UploadingTime = DateTime.Parse(comment["UploadingTime"].ToString()),
                                   Tags = ConvertTo<UserMention[]>(r["taged"].As<List<Dictionary<string, object>>>()),
                                   Likes = ConvertTo<UserMention[]>(r["likes"].As<List<Dictionary<string, object>>>()),
                                   Commenter = ConvertTo<UserMention>(r["commenter"].As<Dictionary<string, object>>())
                               };
                           });
            }
        }

        public void Like(Guid id, string likerId, LikeOptions likeOption)
        {
            using (var session = _driver.Session())
            {
                var x = session.Run(
                   "MATCH\n" +
                   $"\t(u:User{{ UserID: \"{likerId}\" }}),\n" +
                   $"\t(l:{likeOption}{{ {likeOption}ID: \"{id}\" }})\n" +
                   "MERGE\n" +
                   "\t(l)-[:Liked_by]->(u)");
            }
        }

        public void Unlike(Guid id, string likerId, LikeOptions likeOption)
        {
            using (var session = _driver.Session())
            {
                session.Run(
                    "MATCH\n" +
                   $"\t(:User{{ UserID: \"{likerId}\" }})<-[l:Liked_by]-" +
                   $"(:{likeOption}{{ {likeOption}ID: \"{id}\" }})\n" +
                   "DELETE l");
            }
        }

        public void Remove(Guid id, string uploaderId, RemoveOptions removeOption)
        {
            using (var session = _driver.Session())
            {
                session.Run(
                    $"MATCH (n:{removeOption}{{ {removeOption}ID : \"{id}\" }})\n" +
                    "DETACH DELETE n");
            }
        }

        private T ConvertTo<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public void EditUser(User user)
        {
            using (var sessoin = _driver.Session())
            {
                sessoin.Run($"MATCH (u:User{{ UserID: \"{user.UserId}\" }})\n" +
                    $"SET u.FirstName = \"{user.FirstName}\",\n" +
                    $"u.LastName = \"{user.LastName}\"");
            }
        }
    }
}
