using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using StressTests.Common;
using System.Threading.Tasks;

namespace StressTests
{
    [TestClass]
    public class Stress
    {
        const string API_URL = "http://notifications.altasoft.ge";


        [TestMethod]
        public async Task SendSMS()
        {
            var model = new SendModel
            {
                ApplicationId = 1,
                ApplicationSecretKey = "TEST",
                ApplicationProductKey = "TEST_PRODUCT",

                To = "995593149115",
                ProviderKey = "sms",
                Content = "Hello, Bye",

                Priority = 1,
                IsTest = true
            };


            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(API_URL + "/API/Send", model);
            var result = await response.Content.ReadAsStringAsync();
        }


        [TestMethod]
        public async Task SaveUser()
        {
            var model = new SaveUserModel
            {
                ApplicationId = 1,
                ApplicationSecretKey = "TEST",

                ExternalUserId = 7777,
                FirstName = "James",
                FullName = "James Bond",
                MobileNumber = "577007007",
                Email = "007@agent.org"
            };


            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(API_URL + "/API/SaveUser", model);
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
