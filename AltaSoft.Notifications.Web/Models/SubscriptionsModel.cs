using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models
{
    public class SubscriptionsModel
    {
        public int? EventId { get; set; }
        public string EventDescription { get; set; }
        public List<int> UserIds { get; set; }
        public List<string> UserFullNames { get; set; }
    }
}