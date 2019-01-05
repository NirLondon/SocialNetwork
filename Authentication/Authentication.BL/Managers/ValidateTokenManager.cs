using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.BL;
using Authentication.Common.Enums;
using Authentication.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.BL.Managers
{
    public class ValidateTokenManager : IValidateToken
    {
        private int tokenExpirationMniutes { get; set; }
        AuthenticationRepository repository;

        public ValidateTokenManager()
        {
            tokenExpirationMniutes = 15;
            repository = new AuthenticationRepository();
        }

        public string ValidateToken(string token)
        {
            Document tokendoc = repository.GetToken(token);
            if (tokendoc != null)
            {
                var created = (DateTime)tokendoc["CreatedDate"];
                var expire = created + new TimeSpan(0, tokenExpirationMniutes, 0);
                var finalExpire = expire + new TimeSpan(0, tokenExpirationMniutes, 0);
                if (DateTime.Now > finalExpire)
                {
                    token = null;
                }
                else if (DateTime.Now > expire && DateTime.Now < finalExpire)
                {
                    tokendoc["State"] = TokenStateEnum.Expired.ToString();
                    Document newTokendoc = GenerateNewToken(tokendoc["AssignedUser"]);
                    repository.PutToken(tokendoc);
                    repository.PutToken(newTokendoc);
                    token = newTokendoc["Token"];
                }
            }
            else
            {
                token = null;
            }
            return token;
        }

        private Document GenerateNewToken(string username)
        {
            Document doc = new Document();
            doc["Token"] = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            doc["AssignedUser"] = username;
            doc["State"] = Common.Enums.TokenStateEnum.Valid.ToString();
            return doc;
        }
    }
}
