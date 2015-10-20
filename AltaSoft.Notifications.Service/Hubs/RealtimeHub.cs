using AltaSoft.Notifications.DAL;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.Service.Hubs
{
    public class RealtimeHub : Hub
    {
        const string QUERY_STRING_APPLICATION_KEY = "appid";
        const string QUERY_STRING_TOKEN_KEY = "token";


        public string GroupName { get; set; }


        public override async Task OnConnected()
        {
            var applicationIdString = Context.QueryString[QUERY_STRING_APPLICATION_KEY];
            var externalUserToken = Context.QueryString[QUERY_STRING_TOKEN_KEY];
            var externalUserIPAddress = GetIPAddress();
            int applicationId;
            string checkUserIdUrl;

            if (!Int32.TryParse(applicationIdString, out applicationId))
                return;

            using (var bo = new ApplicationBusinessObject())
            {
                var application = bo.GetById(applicationId);
                if (application == null)
                    return;

                checkUserIdUrl = application.CheckUserIdUrl;
            }

            var externalUserId = await GetExternalUserId(checkUserIdUrl, externalUserToken, externalUserIPAddress);

            if (externalUserId.HasValue)
                return;

            // Check if user exists in our database
            using (var bo = new UserBusinessObject())
            {
                var user = bo.GetList(x => x.ApplicationId == applicationId && x.ExternalUserId == externalUserId).FirstOrDefault();
                if (user == null) return;
            }

            GroupName = applicationId.ToString() + externalUserId.ToString();


            await Groups.Add(Context.ConnectionId, GroupName);

            Clients.Caller.AuthenticationSuccess(applicationId, externalUserId);
            Clients.Group(GroupName).UserConnected(externalUserId);

            await base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await Groups.Remove(Context.ConnectionId, GroupName);

            await base.OnDisconnected(stopCalled);
        }


        async Task<int?> GetExternalUserId(string url, string token, string ipaddress)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync(url + "?token=" + token + "&ipaddress=" + ipaddress);

            var o = JObject.Parse(result);
            var isSuccess = (bool)o["IsSuccess"];
            var userID = (int)o["UserID"];

            return isSuccess ? (int?)userID : null;
        }

        string GetIPAddress()
        {
            var ipAddress = (string)Context.Request.Environment["server.RemoteIpAddress"];
            return ipAddress;
        }
    }
}
