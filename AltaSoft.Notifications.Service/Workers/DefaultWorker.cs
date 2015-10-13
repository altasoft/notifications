using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.Service.ProviderManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.Service.Workers
{
    public class DefaultWorker : Worker<Message>
    {
        public DefaultWorker(int priority) : base(priority)
        {
            RegisterProvider<SendGridProviderManager>();
            RegisterProvider<SmtpMailProviderManager>();
            RegisterProvider<SignalrProviderManager>();
        }


        protected override Task<List<Message>> GetMessagesToBeProceed()
        {
            using (var db = new MessageBusinessObject())
                return db.GetListToBeProceeded(this.Priority == 1 ? MessagePriority.High : MessagePriority.Normal);
        }

        protected override void MessageSendFailed(Message message, double durationInMS, string errorCode, string errorMessage)
        {
            message.ProcessingDuration = Convert.ToInt32(durationInMS);
            message.State = MessageStates.Failed;
            message.RetryCount++;
            message.ErrorMessage = errorCode;
            message.ErrorDetails = errorMessage;

            using (var db = new MessageBusinessObject())
                db.Update(message);
        }

        protected override void MessageSendSuccessfully(Message message, double durationInMS)
        {
            message.ProcessingDuration = Convert.ToInt32(durationInMS);
            message.State = MessageStates.Success;
            message.RetryCount++;

            using (var db = new MessageBusinessObject())
                db.Update(message);
        }
    }
}
