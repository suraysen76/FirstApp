using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using HangfireQueueJobs.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HangfireTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireJobTestController : Controller
    {
        private  int criticalJobcounter;
        private readonly ILogger _logger;
        private readonly IHangfireQueueJobsService _hangfireTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public HangfireJobTestController(IHangfireQueueJobsService hangfireTestService, IBackgroundJobClient backgroundJobClient, ILogger<HangfireJobTestController> logger)
        {
            _hangfireTestService = hangfireTestService;
            _backgroundJobClient = backgroundJobClient;
            _logger = logger;
            criticalJobcounter = 0;
        }
        #region playground APIs
        [HttpGet("/GetAPIist")]
        public IEnumerable<string> GetAPIist()
        {
            IMonitoringApi monitoringApi = JobStorage.Current.GetMonitoringApi();
            IList<ServerDto> servers = monitoringApi.Servers();
            var queues = monitoringApi.Queues();
            
            foreach (var q in queues)
            {
                var nm = q.Name;
                
            }
            var scnt = servers.Count();
            _logger.LogError("Hey, this is a GetAPIist.");
            return new string[] { "Sercer Count", scnt.ToString() };
        }

        [HttpGet("/AddFireAndForgetJob")]
        public ActionResult AddFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _hangfireTestService.AddFireAndForgetJob());
            return Ok();
        }
        [HttpGet("/AddDelayedJob")]
        public ActionResult AddDelayedJob()
        {
            var criticalQ_State = new EnqueuedState("default");
            var time = DateTime.Now.AddMinutes(7);
            var jobId = _backgroundJobClient.Schedule(() => _hangfireTestService.AddDelayedJob(), time);
            //_backgroundJobClient.ChangeState(jobId, criticalQ_State);
            
            return Ok();
        }
        [HttpGet("/AddContinuationJob")]
        public ActionResult AddContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _hangfireTestService.AddFireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _hangfireTestService.AddContinuationJob());
            return Ok();
        }
        #endregion

        #region test hangfire

        [HttpPost("/Step1_MasterPromoCodeJobs")]
        public ActionResult Step1_MasterPromoCodeJobs()
        {            
            DateTime current = DateTime.Now.AddMinutes(2);
            _logger.LogError("Executing Step1_Job1.");
            
            // Create 250 Jobs for default Q
            int numOfDefaultJobs = 50;
            for (int i = 1; i <= numOfDefaultJobs ; i++)
            {
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job1(i), current);                
            };  
           
            _logger.LogError("Executed Step1_MasterPromoCodeJobs Jobs.");
            return Ok();
        }

        [HttpPost("/Step2_BookingJobs")]
        public ActionResult Step2_BookingJobs()
        {


            DateTime current = DateTime.Now.AddMinutes(1);
            _logger.LogError("Executing Step2_BookingJobs.");
       
            int numOfBatch1Jobs = 10;
            
            //Create 10 Jobs for default Q
            for (int i = 1; i <= numOfBatch1Jobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job3(criticalJobcounter), current);
            };
            
            _logger.LogError("Executed Step2_Job3.");
            return Ok();
        }

        [HttpPost("/Step3_PushNotificationJobs")]
        public ActionResult Step3_PushNotificationJobs()
        {

            DateTime current = DateTime.Now.AddMinutes(1);
            _logger.LogError("Executing Step3_PushNotoficationJobs.");
                                    
            int numOfBatchJobs = 20;

            //Create 20 Jobs for default Q
            for (int i = 1; i <= numOfBatchJobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job2(criticalJobcounter), current);
            };

            _logger.LogError("Executed Step3_Job2.");
            return Ok();
        }

        [HttpPost("/Step4_BookingJobs_2")]
        public ActionResult Step4_BookingJobs_2()
        {

            DateTime current = DateTime.Now.AddMinutes(1);
            _logger.LogError("Executing Step4_BookingJobs_2.");

            int numOfBatchJobs = 30;

            //Create 30 Jobs for default Q
            for (int i = 1; i <= numOfBatchJobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job3(criticalJobcounter), current);
            };

            _logger.LogError("Executed Step4_Job3_2.");
            return Ok();
        }
        #endregion

        #region bengs test
        [HttpGet("/DoTest_1")]
        public ActionResult DoTest_1()
        {
            DateTime current = DateTime.Now.AddMinutes(1);
            _logger.LogError("Executing DoTest_1.");
            int numOfBatchJobs = 3;

            //Create 5 Jobs for default Q
            _logger.LogError("Executing 3 Job1.");
            for (int i = 1; i <= numOfBatchJobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job1(criticalJobcounter), current);
            };
                                 

            
            numOfBatchJobs = 2;
            _logger.LogError("Executing 2 Job2.");
            for (int i = 1; i <= numOfBatchJobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job2(criticalJobcounter), current);
            };


            current = current.AddMinutes(2);
            _logger.LogError("Executing 2 Job2.");
            numOfBatchJobs = 2;

            for (int i = 1; i <= numOfBatchJobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job2(criticalJobcounter), current);
            };


            current = current.AddMinutes(1);
            numOfBatchJobs = 2;
            _logger.LogError("Executing 10 Job1 (2) Jobs.");
            for (int i = 1; i <= numOfBatchJobs; i++)
            {
                criticalJobcounter += 1;
                var jobId = BackgroundJob.Schedule(() => _hangfireTestService.Job1(criticalJobcounter), current);
            };

            return Ok();
        }
        #endregion


    }
}
