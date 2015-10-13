using AltaSoft.Notifications.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models.API
{
    public class SendModel : ApplicationCredentialsModel
    {
        public string ExternalUserId { get; set; }
        public List<string> ExternalUserIds { get; set; }
        public string To { get; set; }
        public string EventKey { get; set; }

        public string ProviderKey { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int Priority { get; set; }
        public bool IsTest { get; set; }
    }
}