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
        void Job3(int id);
        void Job2(int id);
        [Queue("secondary_queue")] 
        void Job1(int id);  
    }
}
