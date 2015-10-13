using AltaSoft.Notifications.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.Service.Common
{
    public interface IProviderManager<TMessage>
    {
        int Id { get; }

        Task<ProviderProcessResult> Process(TMessage message);
    }
}
