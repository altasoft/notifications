using AltaSoft.Notifications.Service;
using AltaSoft.Notifications.Service.Workers;
using System.Collections.Generic;
using System.Configuration;

namespace AltaSoft.Notifications.Web
{
    public class WorkersConfig
    {
        public static void Register()
        {
            if (ConfigurationManager.AppSettings["NotificationServiceWorkerEnabled"] == "true")
            {
                var workers = new List<IWorker>();
                workers.Add(new SMSWorker(1));
                workers.Add(new SMSWorker(0));
                workers.Add(new DefaultWorker(1));
                workers.Add(new DefaultWorker(0));

                workers.ForEach(x => x.Start());
            }
        }
    }
}