using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    public class ApplicationProduct : ModelBase
    {
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public DateTime? SleepFromTime { get; set; }
        public DateTime? SleepToTime { get; set; }
    }
}
