using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Models
{
    public class ComposeModel
    {
        public List<int> Users { get; set; }
        [DisplayName("Groups")]
        public List<int> Events { get; set; }
        public int Provider { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}