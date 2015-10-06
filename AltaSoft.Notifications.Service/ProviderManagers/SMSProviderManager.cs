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
using System.Globalization;

namespace AltaSoft.Notifications.Service.ProviderManagers
{
    [Export(typeof(IProviderManager))]
    public class SMSProviderManager : IProviderManager
    {
        static CultureInfo EN_US_CULTURE = new CultureInfo("en-US");

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

            if (message.ForceSendingNow != true && String.IsNullOrEmpty(message.Application.SMSServiceProductLimited))
                throw new Exception("Application Configuration Issue: SMSServiceProductLimited not defined for SMS service");

            if (message.ForceSendingNow == true && String.IsNullOrEmpty(message.Application.SMSServiceProductAnyTime))
                throw new Exception("Application Configuration Issue: SMSServiceProductAnyTime not defined for SMS service");


            // 2. Message
            var sms = new SMS
            {
                product = (message.ForceSendingNow == true) ? message.Application.SMSServiceProductAnyTime : message.Application.SMSServiceProductLimited,
                refNum = message.Id.ToString(),
                smsPhone = message.To.Trim('+', ' '),
                smsText = message.Content,
                source_number_or_name = message.Application.SMSFromName,
                ttl = 11,
                sendFromTime = DateTime.Now.AddDays(-1).ToString(EN_US_CULTURE),
                sendToTime = DateTime.Now.AddDays(1).ToString(EN_US_CULTURE),
                state = 1,
                for_test = false
            };

            if (message.ProcessDate.HasValue)
            {
                sms.sendFromTime = message.ProcessDate.Value.ToString(EN_US_CULTURE);
                sms.sendToTime = message.ProcessDate.Value.AddDays(1).ToString(EN_US_CULTURE);
            }

            if (sms.smsPhone.Length > 9)
                sms.smsPhone = sms.smsPhone.Substring(sms.smsPhone.Length - 9, 9);


            // 3. Send
            var url = message.Provider.WebUrl + "?" + sms.GetQueryString();

            var result = this.CallServiceGet(url, message.Application.SMSServiceCustomerId.Value, HttpMethod.Put);

            if (result != null && result.Trim('"').ToLower() == "ok")
                return new ProviderProcessResult { IsSuccess = true };


            return new ProviderProcessResult { IsSuccess = false, ErrorMessage = result, ErrorDetails = result };
        }


        string CallServiceGet(string url, int customerID, HttpMethod method = null)
        {
            HttpClient client = new HttpClient();

            if (method == null)
                method = HttpMethod.Get;

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = method,
            };
            request.Headers.Add("X-AS-CID", customerID.ToString());

            var response = client.SendAsync(request).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

        class SMS
        {
            public string product { get; set; }
            public string refNum { get; set; }
            public string smsPhone { get; set; }
            public string smsText { get; set; }
            public string source_number_or_name { get; set; }
            public int? ttl { get; set; }
            public string sendFromTime { get; set; }
            public string sendToTime { get; set; }
            public int? state { get; set; }
            public bool? for_test { get; set; }
        }
    }
}
