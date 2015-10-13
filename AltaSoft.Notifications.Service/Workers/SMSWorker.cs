using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.Service.ProviderManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.Service.Workers
{
    public class SMSWorker : Worker<SMS>
    {
        public SMSWorker(int priority) : base(priority)
        {
            RegisterProvider<GeocellSMSProviderManager>();
        }


        protected override Task<List<SMS>> GetMessagesToBeProceed()
        {
            using (var bo = new SMSBusinessObject())
                return bo.GetListToBeProceeded(Priority);
        }

        protected override void MessageSendFailed(SMS message, double durationInMS, string errorCode, string errorMessage)
        {
            using (var bo = new SMSBusinessObject())
                bo.UpdateMessageStatusToFailed(message.Id, durationInMS, errorCode, errorMessage);
        }

        protected override void MessageSendSuccessfully(SMS message, double durationInMS)
        {
            using (var bo = new SMSBusinessObject())
                bo.UpdateMessageStatusToSuccess(message.Id, durationInMS);
        }
    }
}
