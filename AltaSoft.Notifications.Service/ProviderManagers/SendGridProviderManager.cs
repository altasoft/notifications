using AltaSoft.Notifications.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltaSoft.Notifications.DAL;
using SendGrid;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.ComponentModel.Composition;

namespace AltaSoft.Notifications.Service.ProviderManagers
{
    public class SendGridProviderManager : IProviderManager<Message>
    {
        public int Id
        {
            get { return 1; }
        }

        public async Task<ProviderProcessResult> Process(Message message)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.AddTo(message.To);
                msg.From = new MailAddress(message.Application.EmailFromAddress, message.Application.EmailFromFullName);
                msg.Subject = message.Subject;
                msg.Html = message.Content;

                var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridSecretKey"]);

                await transportWeb.DeliverAsync(msg);

                return new ProviderProcessResult { IsSuccess = true };
            }
            catch (Exceptions.InvalidApiRequestException ex)
            {
                return new ProviderProcessResult { IsSuccess = false, ErrorCode = String.Join(",", ex.Errors), ErrorMessage = ex.ToString() };
            }
            catch (Exception ex)
            {
                return new ProviderProcessResult { IsSuccess = false, ErrorCode = ex.Message, ErrorMessage = ex.ToString() };
            }
        }
    }
}
