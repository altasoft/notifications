using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models
{
    public class ComposeModel
    {
        public List<string> To { get; set; }
        public List<string> Provider { get; set; }
        public string message { get; set; }
    }
}