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
using System.Configuration;

namespace AltaSoft.Notifications.Service.ProviderManagers
{
    public class GeocellSMSProviderManager : IProviderManager<SMS>
    {
        static CultureInfo EN_US_CULTURE = new CultureInfo("en-US");

        string GeocellUrl
        {
            get { return ConfigurationManager.AppSettings["SMSGeocellUrl"]; }
        }

        public int Id
        {
            get { return 2; }
        }


        public async Task<ProviderProcessResult> Process(SMS message)
        {
            // 1. Validation
            Validate(message);


            // 2. Send
            var client = new HttpClient();

            var response = await client.GetAsync(BuildUrlForSend(message));
            var res = await response.Content.ReadAsStringAsync();

            var isSuccess = (res == "Y");
            if (isSuccess)
                return new ProviderProcessResult { IsSuccess = isSuccess };

            return new ProviderProcessResult { IsSuccess = isSuccess, ErrorCode = "Geocell Error", ErrorMessage = res };
        }


        void Validate(SMS sms)
        {
            // 1. Validations
            //if (String.IsNullOrEmpty(message.Provider.WebUrl))
            //    throw new Exception("Provider Configuration Issue: WebUrl not defined for SMS service");

            //if (!message.Application.SMSServiceCustomerId.HasValue)
            //    throw new Exception("Application Configuration Issue: SMSServiceCustomerId not defined for SMS service");

            //if (String.IsNullOrEmpty(message.Application.SMSFromName))
            //    throw new Exception("Application Configuration Issue: SMSFromName not defined for SMS service");

            //if (message.ForceSendingNow != true && String.IsNullOrEmpty(message.Application.SMSServiceProductLimited))
            //    throw new Exception("Application Configuration Issue: SMSServiceProductLimited not defined for SMS service");

            //if (message.ForceSendingNow == true && String.IsNullOrEmpty(message.Application.SMSServiceProductAnyTime))
            //    throw new Exception("Application Configuration Issue: SMSServiceProductAnyTime not defined for SMS service");


            if (string.IsNullOrEmpty(GeocellUrl))
                throw new Exception("GeocellUrl not specified");

            if (string.IsNullOrEmpty(sms.SenderNumber))
                throw new Exception("SenderNumber not specified");

            if (string.IsNullOrEmpty(sms.To))
                throw new Exception("To not specified");

            if (string.IsNullOrEmpty(sms.Message))
                throw new Exception("Message not specified");

            if (sms.To.Length != 12 || !sms.To.StartsWith("9955"))
                throw new Exception("Invalid phone number (Limited: Georgian numbers only)");

        }

        string BuildUrlForSend(SMS sms)
        {
            return new StringBuilder(GeocellUrl)
                .Append("?Src=").Append(sms.SenderNumber)
                .Append("&Dst=").Append(sms.To)
                .Append("&Txt=").Append(sms.Message)
                .ToString();
        }

    }
}
