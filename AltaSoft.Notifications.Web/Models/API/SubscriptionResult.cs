using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models.API
{
    public class SubscriptionResult
    {
        public string ProviderKey { get; set; }
        public string EventKey { get; set; }
        public string EventDescription { get; set; }
        public string ExternalUserId { get; set; }
    }
}