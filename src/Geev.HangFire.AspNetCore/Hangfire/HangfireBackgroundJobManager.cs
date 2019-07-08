using System;
using System.Threading.Tasks;
using Geev.BackgroundJobs;
using Geev.Threading.BackgroundWorkers;
using HangfireBackgroundJob = Hangfire.BackgroundJob;

namespace Geev.Hangfire
{
    public class HangfireBackgroundJobManager : BackgroundWorkerBase, IBackgroundJobManager
    {
        public Task<string> EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null) where TJob : IBackgroundJob<TArgs>
        {
            string jobUniqueIdentifier = string.Empty;

            if (!delay.HasValue)
            {
                jobUniqueIdentifier = HangfireBackgroundJob.Enqueue<TJob>(job => job.Execute(args));
            }
            else
            {
                jobUniqueIdentifier = HangfireBackgroundJob.Schedule<TJob>(job => job.Execute(args), delay.Value);
            }

            return Task.FromResult(jobUniqueIdentifier);
        }

        public Task<bool> DeleteAsync(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            bool successfulDeletion = HangfireBackgroundJob.Delete(jobId);
            return Task.FromResult(successfulDeletion);
        }
    }
}
