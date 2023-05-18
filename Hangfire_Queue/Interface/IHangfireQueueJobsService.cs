using HangfireTest1.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireQueueJobs.Interface
{
    public interface IHangfireQueueJobsService
    {
        void AddFireAndForgetJob();
        void AddReccuringJob();
        void AddDelayedJob();
        void AddContinuationJob();
        [Queue("a")] 
        void BookingJob(int id);
        [Queue("a")] 
        void PushNotificationJob(int id);
        [Queue("b")] 
        void MasterPromoCodeScheduleJob(int id);
        [Queue("b")] 
        void PromoCodeWalletJob(int id);

        
        Task DoQueueJob( int JobId);

        Task WrapperJob(string jobId);
    }
}
