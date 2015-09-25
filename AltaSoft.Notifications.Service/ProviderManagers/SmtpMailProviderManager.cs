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
    [Export(typeof(IProviderManager))]
    public class SmtpMailProviderManager : IProviderManager
    {
        public int Id
        {
            get { return 4; }
        }

        public async Task<ProviderProcessResult> Process(Message message)
        {
            try
            {
                var mail = new MailMessage();
                var SmtpServer = new SmtpClient();

                mail.To.Add(message.To);
                mail.Subject = message.Subject;
                mail.Body = message.Content;
                mail.IsBodyHtml = true;

                await SmtpServer.SendMailAsync(mail);

                return new ProviderProcessResult { IsSuccess = true };
            }
            catch (Exceptions.InvalidApiRequestException ex)
            {
                return new ProviderProcessResult { IsSuccess = false, ErrorMessage = String.Join(",", ex.Errors), ErrorDetails = ex.ToString() };
            }
            catch (Exception ex)
            {
                return new ProviderProcessResult { IsSuccess = false, ErrorMessage = ex.Message, ErrorDetails = ex.ToString() };
            }
        }
    }
}
