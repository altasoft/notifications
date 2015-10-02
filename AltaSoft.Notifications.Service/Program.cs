using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using Topshelf;

namespace AltaSoft.Notifications.Service
{
    class Program
    {
        static WorkerManager normalWorker, highPriorityWorker;

        static void Main(string[] args)
        {
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
            var url = ConfigurationManager.AppSettings["SignalrUrl"];

            normalWorker = new WorkerManager(DAL.MessagePriority.Normal);
            highPriorityWorker = new WorkerManager(DAL.MessagePriority.High);

            normalWorker.Start();
            highPriorityWorker.Start();

            WebApp.Start(url);
        }

        static void Stop()
        {
            normalWorker.Stop();
            highPriorityWorker.Stop();
            Environment.Exit(0);
        }
    }
}
