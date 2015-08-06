using AltaSoft.Notifications.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models.API
{
    public class MessageResult
    {
        public string ExternalUserId { get; set; }

        public string ProviderKey { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public int RetryCount { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDetails { get; set; }

        public MessageStates State { get; set; }

        public MessagePriority Priority { get; set; }

        public DateTime? ProcessDate { get; set; }
    }
}