using AltaSoft.Notifications.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltaSoft.Notifications.DAL;
using System.ComponentModel.Composition;
using Microsoft.AspNet.SignalR;
using AltaSoft.Notifications.Service.Hubs;
using Microsoft.CSharp.RuntimeBinder;
using System.Runtime.CompilerServices;

namespace AltaSoft.Notifications.Service.ProviderManagers
{
    public class SignalrProviderManager : IProviderManager<Message>
    {
        public int Id
        {
            get { return 3; }
        }

        public async Task<ProviderProcessResult> Process(Message message)
        {
            var groupName = message.ApplicationId + message.To;

            var conn = GlobalHost.ConnectionManager.GetHubContext<RealtimeHub>().Clients.Group(groupName);

            // Calling dynamic object
            var binder = Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, message.Subject, null, typeof(Program), new [] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) });
            var callSite = CallSite<Action<CallSite, object, string>>.Create(binder);
            callSite.Target(callSite, conn, message.Content);


            return new ProviderProcessResult
            {
                IsSuccess = true
            };
        }
    }
}
