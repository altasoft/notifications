using AltaSoft.Notifications.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltaSoft.Notifications.DAL;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace AltaSoft.Notifications.Service.ProviderManagers
{
    [Export(typeof(IProviderManager))]
    public class SMSProviderManager : IProviderManager
    {
        public int Id
        {
            get { return 2; }
        }

        public async Task<ProviderProcessResult> Process(Message message)
        {
            // 1. Validations
            if (String.IsNullOrEmpty(message.Provider.WebUrl))
                throw new Exception("Provider Configuration Issue: WebUrl not defined for SMS service");

            if (!message.Application.SMSServiceCustomerId.HasValue)
                throw new Exception("Application Configuration Issue: SMSServiceCustomerId not defined for SMS service");

            if (String.IsNullOrEmpty(message.Application.SMSFromName))
                throw new Exception("Application Configuration Issue: SMSFromName not defined for SMS service");



            // 2. Message
            var sms = new SMS
            {
                product = (message.ForceSendingNow == true) ? message.Application.SMSServiceProductAnyTime : message.Application.SMSServiceProductLimited,
                refNum = message.Id.ToString(),
                smsPhone = message.To,
                smsText = message.Content,
                source_number_or_name = message.Application.SMSFromName,
                for_test = false
            };

            if (message.ProcessDate.HasValue)
                sms.sendFromTime = message.ProcessDate;



            // 3. Send
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-AS-CID", message.Application.SMSServiceCustomerId.ToString());

            var response = await client.PutAsJsonAsync(message.Provider.WebUrl, sms);
            var result = await response.Content.ReadAsStringAsync();

            if (result != null && result.ToLower() == "ok")
                return new ProviderProcessResult { IsSuccess = true };


            return new ProviderProcessResult { IsSuccess = false, ErrorMessage = result, ErrorDetails = result };
        }


        class SMS
        {
            public string product { get; set; }
            public string refNum { get; set; }
            public string smsPhone { get; set; }
            public string smsText { get; set; }
            public string source_number_or_name { get; set; }
            public int? ttl { get; set; }
            public DateTime? sendFromTime { get; set; }
            public DateTime? sendToTime { get; set; }
            public int? state { get; set; }
            public bool? for_test { get; set; }
        }
    }
}
