using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models.API
{
    public class EventResult
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public DateTime RegDate { get; set; }
    }
}