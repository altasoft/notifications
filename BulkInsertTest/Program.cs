using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace BulkInsertTest
{
    class Program
    {
        const string API_URL = "http://localhost:2183";

        static void Main(string[] args)
        {
            var to = new List<string>();
            for (int i = 0; i < 200000; i++)
                to.Add("995593149115");


            var model = new SendModel
            {
                ApplicationId = 1,
                ApplicationSecretKey = "TEST",

                To = String.Join(",", to),
                ProviderKey = "sms",
                Content = "Hello, Bye",

                Priority = 1,
                IsTest = true
            };


            var client = new HttpClient();
            var response = client.PostAsJsonAsync(API_URL + "/API/Send", model).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }


    public class SendModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationSecretKey { get; set; }

        public string ExternalUserId { get; set; }
        public List<string> ExternalUserIds { get; set; }
        public string To { get; set; }
        public string EventKey { get; set; }

        public string ProviderKey { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? Priority { get; set; }
        public bool IsTest { get; set; }
    }
}
