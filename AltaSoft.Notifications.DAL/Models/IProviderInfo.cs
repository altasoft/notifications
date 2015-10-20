using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    public interface IProviderInfo
    {
        int ProviderId { get; }
        bool IsTest { get; }
    }
}
