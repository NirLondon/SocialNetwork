using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Neo4j.Driver.V1;
using Newtonsoft.Json;
using Social.Common.DAL;
using Social.Common.Models;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Social.DAL
{
    public class SocialRepository : ISocialRepository
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
            return UsersQueryParse(
                $"MATCH (u:User)<-[:Blocks]-(:User{{ UserID: \"{blockerId}\" }}) " +
                 "RETURN u");
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
                session.Run(
                   "MATCH " +
                   $"(p: Post{{ PostID: \"{comment.PostId}\" }}), " +
                   $"(u: User{{ UserID: \"{commenterId}\" }}), " +
                   (hasTags ? tagedUsersMatch + "\n" : string.Empty) +
                   "CREATE " +
                   $"(c: Comment{{ CommentID: \"{Guid.NewGuid()}\", Content: \"{comment.Content}\", ImageURL: \"{comment.ImagURL}\", UploadingTime: \"{DateTime.Now}\" }}), " +
                   "(c) < -[:Has] - (p), " +
                   "(c) -[:Uploaded_by]->(u), " +
                   (hasTags ? tagsCreate : string.Empty));
            }
        }

        public IEnumerable<UserMention> UsersFollowedBy(string userId)
        {
            return UsersQueryParse(
            "MATCH (:User{ UserID: \"" + userId + "\"})-[:Follows]->(u:User) " +
            "RETURN u");
        }

        public IEnumerable<UserMention> FollowersOf(string userId)
        {
            return UsersQueryParse(
                "MATCH (:User{ UserID: \"" + userId + "\" })<-[:Follows]-(u:User) " +
                "RETURN u");
        }

        public IEnumerable<ReturnedPost> PostsForUser(string userId, int amount, int skip)
        {
            using (var session = _driver.Session())
            {
                return
                    session.Run(
                        "MATCH\n" +
                            $"\t(u: User{{ UserID: \"{userId}\" }})-[:Follows]->(:User)-[:Uploaded]->(p: Post),\n" +
                            "\t(u)<-[:Tags]-(p2: Post),\n" +
                            "\t(p3: Post)-[:Has]->(c: Comment)-[:Tags]->(u)\n" +
                        "WITH\n" +
                            "\tcollect(p) +\n" +
                            "\tcollect(p2) +\n" +
                            "\tcollect(p3)\n" +
                            "\tAS res, u\n" +
                        "UNWIND res AS Post\n" +
                        "OPTIONAL MATCH(Post)<-[:Uploaded]-(poster: User)\n" +
                        "RETURN DISTINCT\n" +
                            "\tPost.PostID AS PostId,\n" +
                             "\t{\n" +
                                "\t\tUserId: poster.UserID,\n" +
                                "\t\tFullName: poster.firstName + \" \" + poster.lastName\n" +
                             "\t} AS Poster,\n" +
                             "\tPost.Content AS Content,\n" +
                             "\tPost.UploadingTime AS UploadingTime,\n" +
                             "\tPost.ImageURL AS ImageURL,\n" +
                             "\t[(Post)-[:Liked_by]->(liker:User) |\n" +
                             "\t\t{\n" +
                                "\t\t\tUserId : liker.UserID,\n" +
                                "\t\t\tFullName : liker.firstName + \" \" + liker.lastName\n" +
                             "\t\t}] AS Likes,\n" +
                             "\t[(Post) -[:Tags]->(taged: User) |\n" +
                                     "\t\t{\n" +
                                         "\t\t\tUserId : taged.UserID,\n" +
                                        "\t\t\tFullName : taged.firstName + \" \" + taged.lastName\n" +
                                    "\t\t}] AS Tags\n" +
                        "ORDER BY Post.UploadingTime\n" +
                        $"SKIP {skip}\n" +
                        $"LIMIT {amount}\n")
                        .Select(record =>
                            new ReturnedPost
                            {
                                PostId = Guid.Parse(record["PostId"].ToString()),
                                Poster = ConvertTo<UserMention>(record["Poster"].As<Dictionary<string, object>>()),
                                Content = record["Content"].ToString(),
                                ImageURL = record["ImageURL"].ToString(),
                                UploadingTime = DateTime.Parse(record["UploadingTime"].ToString()),
                                Likes = ConvertTo<UserMention[]>(record["Likes"].As<List<Dictionary<string, object>>>()),
                                Tags = ConvertTo<UserMention[]>(record["Tags"].As<List<Dictionary<string, object>>>()),
                            });
            }
        }

        public void PutPost(string posterID, DataBasePost post)
        {
            var tagedUsersMatch = string.Join(",\n\t",
                post.TagedUsersIds
                .Select((userId, i) => $"(t{i}:User{{ UserID: \"{userId}\" }})"));

            var tagsCreate = string.Join(",\n\t",
                post.TagedUsersIds
                .Select((userId, i) => $"(t{i})<-[:Tags]-(p)"));

            bool hasTags = post.TagedUsersIds.Any();

            using (var session = _driver.Session())
            {
                session.Run(
                    $"MATCH\n\t(u:User{{ UserID: \"{posterID}\" }}),\n\t" +
                    (hasTags ? tagedUsersMatch + "\n" : string.Empty) +
                    $"CREATE\n" +
                    "\t(p:Post\n\t{\n" +
                    $"\t\t\tPostID: \"{Guid.NewGuid()}\",\n" +
                    $"\t\t\tContent:\n\t\t\t\"{post.Content}\",\n" +
                    $"\t\t\tUploadingTime: \"{DateTime.Now}\",\n" +
                    $"\t\t\tImageURL: \"{post.ImageURL}\",\n" +
                    $"\t\t\tVisibility: \"{(byte)post.Visibility}\"\n" +
                    "\t}),\n" +
                    "\t(u)-[:Uploaded]->(p),\n\t" +
                    (hasTags ? tagsCreate : string.Empty));
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

        public void AddUser(User user)
        {
            var query = "CREATE (:User{ UserID: \"" + user.UserId + "\", firstName: \"" + user.FirstName + "\", lastName: \"" + user.LastName + "\" })";

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

        private IEnumerable<UserMention> UsersQueryParse(string query)
        {
            using (var session = _driver.Session())
            {
                return session.Run(query).Select(r =>
                {
                    var node = r["u"].As<INode>();
                    return new UserMention
                    {
                        UserId = node["UserID"].ToString(),
                        FullName = node["firstName"].ToString() + node["lastName"].ToString()
                    };
                });
            }
        }

        public IEnumerable<RetunredComment> CommentsOfPost(Guid postId)
        {
            using (var session = _driver.Session())
            {
                return
                    session.Run(
                           "MATCH\n" +
                               $"\t(:Post{{ PostID: \"{postId}\" }})-[:Has]->(comment: Comment)-[:Uploaded_by]->(u: User)\n" +
                           "RETURN\n" +
                               "\tcomment,\n" +
                               "\t{ UserId: u.UserID, FullName: u.firstName + \" \" + u.lastName } as commenter,\n" +
                               "\t[(comment)-[:Tags]->(t) | { UserId : t.UserID, FullName : t.firstName + \" \" + t.lastName }] as taged,\n" +
                               "\t[(comment)-[:Liked_by]->(l) | { UserId : l.UserID, FullName : l.firstName + \" \" + l.lastName }] as likes")
                           .Select(r =>
                           {
                               var comment = r["comment"].As<INode>();
                               return new RetunredComment
                               {
                                   CommentId = Guid.Parse(comment["CommentID"].ToString()),
                                   Content = comment["Content"].ToString(),
                                   ImageURL = comment["ImageURL"].ToString(),
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
    }
}
