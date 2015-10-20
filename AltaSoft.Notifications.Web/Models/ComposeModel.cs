using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AltaSoft.Notifications.Web.Models
{
    public class ComposeModel
    {
        public List<int> Users { get; set; }
        public List<string> Groups { get; set; }
        public int Provider { get; set; }
        public DateTime? ProcessDate { get; set; }
        public bool ForceNow { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        [AllowHtml]
        public string FileContent { get; set; }
    }
}