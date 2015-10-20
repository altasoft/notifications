using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    public class ApplicationProduct : ModelBase
    {
        public int ApplicationId { get; set; }

        [StringLength(20)]
        public string Key { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Template { get; set; }

        public bool IsActive { get; set; }

        public int Priority { get; set; }

        public TimeSpan? SleepFromTime { get; set; }

        public TimeSpan? SleepToTime { get; set; }
    }
}
