using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.Web.Models.API
{
    public class SendResultModel
    {
        public List<int> SuccessMessageIds { get; set; }
        public List<string> FailedMessages { get; set; }
    }
}
