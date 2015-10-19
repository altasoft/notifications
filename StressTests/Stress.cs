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
        const string API_URL = "http://localhost:2183";


        [TestMethod]
        public async Task SendSMS()
        {
            var model = new SendModel
            {
                ApplicationId = 1,
                ApplicationSecretKey = "TEST",

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
    }
}
