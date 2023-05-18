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

      
        [Queue("secondary_queue")]
        public void Job1(int JobId)
        {
            Thread.Sleep(45000);

            Console.WriteLine($"{DateTime.Now.ToString()} - This is a PromoCodeWalletJob job!");
            _logger.LogError("Exec PWC Job#" + JobId);
        }

      
        public void Job2(int JobId)
        {
            Thread.Sleep(45000);

            Console.WriteLine($"{DateTime.Now.ToString()} - This is a PushNotificationJob job!");
            _logger.LogError("Exec PN Job#" + JobId);
        }

        
        public void Job3(int JobId)
        {
            Thread.Sleep(45000);            
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a BookingCancelJob job!");
            _logger.LogError("Exec Job3 Job#" + JobId);
        }
       

    }
}