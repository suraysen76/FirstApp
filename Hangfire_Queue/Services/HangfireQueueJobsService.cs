using Hangfire;
using Hangfire.States;
using HangfireQueueJobs.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HangfireQueueJobs.Services
{
    public class HangfireQueueJobsService : IHangfireQueueJobsService
    {
        private readonly ILogger _logger;
        public HangfireQueueJobsService(ILogger<HangfireQueueJobsService> logger)
        {
            _logger = logger;
        }
        public void AddContinuationJob()
        {
            Thread.Sleep(15000);
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a continuation job!");
        }

        public void AddDelayedJob()
        {
            Thread.Sleep(15000);
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a Delayed job!");
        }

       
        public void AddFireAndForgetJob()
        {
            Thread.Sleep(15000);
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a Fire and Forget job!");
        }

        public void AddReccuringJob()
        {
            Thread.Sleep(15000);
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a Recurring job!");
        }

        [Queue("lesscritical")]
        public void MasterPromoCodeScheduleJob(int JobId)
        {
            Thread.Sleep(45000);

            Console.WriteLine($"{DateTime.Now.ToString()} - This is a MasterPromoCodeScheduleJob job!");
            _logger.LogError("Exec MPCS Job#" + JobId);
        }
        
        [Queue("lesscritical")]
        public void PromoCodeWalletJob(int JobId)
        {
            Thread.Sleep(45000);

            Console.WriteLine($"{DateTime.Now.ToString()} - This is a PromoCodeWalletJob job!");
            _logger.LogError("Exec PWC Job#" + JobId);
        }

      
        public void PushNotificationJob(int JobId)
        {
            Thread.Sleep(45000);

            Console.WriteLine($"{DateTime.Now.ToString()} - This is a PushNotificationJob job!");
            _logger.LogError("Exec PN Job#" + JobId);
        }

        
        public void BookingJob(int JobId)
        {
            Thread.Sleep(45000);            
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a BookingCancelJob job!");
            _logger.LogError("Exec BC Job#" + JobId);
        }
        public Task DoQueueJob(int JobId)
        {
                        
            Thread.Sleep(15000);
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a DoQueueJob job!");
            _logger.LogError("Exec BC Job#" + JobId);
            return Task.CompletedTask;
        }

        public Task WrapperJob(string JobId)
        {
            Thread.Sleep(5000);
             
            var lesscriticalQ_State = new EnqueuedState("lesscritical");
            IBackgroundJobClient _backgroundJobClient = new BackgroundJobClient();
            var jobId= _backgroundJobClient.Enqueue(() => this.MasterPromoCodeScheduleJob(1));
            _backgroundJobClient.ChangeState(jobId, lesscriticalQ_State);
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a WrapperJob job!");
            _logger.LogError("Exec WrapperJob Job#" + JobId);
            Thread.Sleep(5000);
            return Task.CompletedTask;
        }



    }
}