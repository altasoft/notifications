using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.DAL.Common;
using AltaSoft.Notifications.Service.Common;
using AltaSoft.Notifications.Service.ProviderManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace AltaSoft.Notifications.Service
{
    public abstract class Worker<TMessage> : IWorker
        where TMessage : ModelBase, IProviderInfo
    {
        public const string EventLogKey = "AltaSoft.Notifications.Service";

        bool IsStarted;
        protected List<IProviderManager<TMessage>> Providers { get; private set; }
        protected int Priority { get; private set; }

        protected virtual TimeSpan Delay
        {
            get
            {
                if (Priority > 0 && Priority < 10)
                    return TimeSpan.FromSeconds(Priority);

                return TimeSpan.FromSeconds(10);
            }
        }

        protected virtual int ParallelTasksCount
        {
            get { return 50; }
        }


        public Worker(int priority)
        {
            Priority = priority;
            Providers = new List<IProviderManager<TMessage>>();
        }

        protected void RegisterProvider<T>()
            where T : class, IProviderManager<TMessage>, new()
        {
            Providers.Add(new T());
        }




        public async Task Start()
        {
            IsStarted = true;

            while (IsStarted)
            {
                try
                {
                    await Process();
                    await Task.Delay(Delay);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(EventLogKey, String.Format("Process Error {0}", ex));
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }

        public async Task Process()
        {
            // 1. Get messages to be proceeded
            var items = await GetMessagesToBeProceed();


            // 2. Process them all
            Parallel.ForEach(items, new ParallelOptions { MaxDegreeOfParallelism = ParallelTasksCount }, async item =>
            {
                try
                {
                    var processStartDate = DateTime.Now;
                    try
                    {
                        var pm = Providers.FirstOrDefault(x => x.Id == item.ProviderId);
                        if (pm == null)
                            throw new Exception("Provider worker not found: " + item.ProviderId.ToString());

                        ProviderProcessResult result;

                        if (item.IsTest)
                            result = new ProviderProcessResult { IsSuccess = true };
                        else
                            result = await pm.Process(item);


                        var duration = DateTime.Now - processStartDate;

                        if (result.IsSuccess)
                        {
                            MessageSendSuccessfully(item, duration.TotalMilliseconds);
                        }
                        else
                        {
                            MessageSendFailed(item, duration.TotalMilliseconds, result.ErrorCode, result.ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        var duration = DateTime.Now - processStartDate;
                        MessageSendFailed(item, duration.TotalMilliseconds, "Internal Exception", ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(EventLogKey, String.Format("Processing Item: {0}, Error: {1}", item.Id, ex));
                }
            });
        }

        public async Task Stop()
        {
            IsStarted = false;
        }


        protected abstract Task<List<TMessage>> GetMessagesToBeProceed();

        protected abstract void MessageSendSuccessfully(TMessage message, double durationInMS);

        protected abstract void MessageSendFailed(TMessage message, double durationInMS, string errorCode, string errorMessage);
    }

    public interface IWorker
    {
        Task Start();
        Task Stop();
    }
}
