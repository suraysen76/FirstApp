using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireTest1.Attribute
{
    public sealed class QueueAttribute : JobFilterAttribute, IElectStateFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueAttribute"/> class
        /// using the specified queue name.
        /// </summary>
        /// <param name="queue">Queue name.</param>
        public QueueAttribute(string queue)
        {
            Queue = queue;
            Order = Int32.MaxValue;
        }

        /// <summary>
        /// Gets the queue name that will be used for background jobs.
        /// </summary>
        public string Queue { get; }

        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is EnqueuedState enqueuedState)
            {
                enqueuedState.Queue = String.Format(Queue, context.BackgroundJob.Job.Args.ToArray());
            }
        }
    }
}
