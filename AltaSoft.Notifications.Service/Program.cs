using AltaSoft.Notifications.Service.Workers;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using Topshelf;

namespace AltaSoft.Notifications.Service
{
    class Program
    {
        static List<IWorker> workers;

        static void Main(string[] args)
        {
            workers = new List<IWorker>();
            workers.Add(new SMSWorker(1));
            workers.Add(new SMSWorker(0));
            workers.Add(new DefaultWorker(1));
            workers.Add(new DefaultWorker(0));

            HostFactory.Run(x =>
            {
                x.Service<object>(s =>
                {
                    s.ConstructUsing(name => new object());
                    s.WhenStarted(tc => Start());
                    s.WhenStopped(tc => Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Manages communication to providers.");
                x.SetDisplayName("AltaSoft Notifications Service");
                x.SetServiceName("Notifications.Service");
            });
        }

        static void Start()
        {
            workers.ForEach(x => x.Start());

            WebApp.Start(ConfigurationManager.AppSettings["SignalrUrl"]);
        }

        static void Stop()
        {
            workers.ForEach(x => x.Stop());

            Environment.Exit(0);
        }
    }
}
