using Hangfire;
using Hangfire.States;
using HangfireTest1.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace HangfireTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private readonly ILogger _logger;
        JobsHelper _jobsHelper = new JobsHelper();
        public JobsController(ILogger<JobsController> logger)
        {
            _logger = logger;
        }
        // GET: api/Jobs
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogError("Hey, this is a ERROR message.");
            return new string[] { "Job1", "Job2" };
        }

        [HttpPost]
        [Route("WelcomeMessage")]
        public IActionResult WelcomeMessage(string message)
        {
            //Fire-and-Forget Jobs
            //Fire - and - forget jobs are executed only once and almost immediately after creation.
            
            var jobId = BackgroundJob.Enqueue(() => _jobsHelper.Print(message));
           
            return Ok($"JobId : {jobId} Completed. Welcome Message Sent");
        }


        [HttpPost]
        [Route("WelcomeMessage_Delayed")]
        public IActionResult WelcomeMessage_Delayed(string message)
        {
           
            var jobId = BackgroundJob.Schedule(() => _jobsHelper.Print(message), TimeSpan.FromSeconds(20));

            return Ok($"JobId : {jobId} Completed. Welcome Message Sent");
        }
        [HttpPost]
        [Route("WelcomeMessage_OnTime")]
        [Queue("TopPriority")]
        public IActionResult WelcomeMessage_OnTime(string message)
        {
           
            
            var client = new BackgroundJobClient();
            var state = new EnqueuedState("top_priority");
            var state2 = new EnqueuedState("secondary_priority");
            var state3 = new EnqueuedState(); // Use the default queue

            
            client.Create(() => _jobsHelper.Print(message), state);
           
            client.Create(() => _jobsHelper.Print(message), state2);
            client.Create(() => _jobsHelper.Print(message), state3);

            //var jobId = BackgroundJob.Schedule(() => _jobsHelper.Print(message), TimeSpan.FromSeconds(5));
            return Ok($"Job Created. Welcome Message Sent");
        }
    }
}

