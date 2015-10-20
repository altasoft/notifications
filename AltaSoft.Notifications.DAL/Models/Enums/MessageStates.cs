using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    public enum MessageStates
    {
        Pending = 0,
        Success = 1,
        Processing = 2,
        Failed = 3,
        ProviderManagerNotFound = 4,
        WaitingForReview = 5
    }
}
