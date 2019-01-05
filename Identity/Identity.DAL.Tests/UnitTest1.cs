using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Identity.Common.DAL;
using Identity.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Identity.DAL.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repo = new IdentitiesRpository();

            repo.AddUser("Nir");
        }

        [TestMethod]
        public void EditIdentity()
        {
            var repo = new IdentitiesRpository();

            repo.EditUser(new Common.Models.UserDetails
            {
                UserID = "Nir",
                Adress = "Yehud",
                BirthDate = new DateTime(1995, 5, 13),
                Company = "Sela",
                Email = "aaa",
                FirstName = "Nir",
                LastName = "London"
            });
        }

        [TestMethod]
        public void GetIdentity()
        {
            var repo = new IdentitiesRpository();

            var res = repo.GetIdentityAsync("Nir").Result;
        }

        [TestMethod]
        public void EditWithToken()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:63276/") };

            using (client)
            {
                client.PutAsJsonAsync($"api/users/editdetails/", new UserDetails
                {
                    Adress = "a",
                    FirstName = "bb",
                    LastName = "cc",
                    Email = "dd",
                    BirthDate = DateTime.Now,
                    Company = "ee",
                    UserID = "Avi"
                }); 
            }
        }

        [TestMethod]
        public void DictionaryToJson()
        {
            var d = new Dictionary<string, object>
            {
                {"a", 1 },
                {"b", 2 }
            };

            var s = JsonConvert.SerializeObject(d);
        }
    }
}
