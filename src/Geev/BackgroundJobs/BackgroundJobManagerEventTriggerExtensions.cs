using System;
using System.Threading.Tasks;
using Geev.Events.Bus;

namespace Geev.BackgroundJobs
{
    public static class BackgroundJobManagerEventTriggerExtensions
    {
        public static Task EnqueueEventAsync<TEvent>(this IBackgroundJobManager backgroundJobManager,
            TEvent e,BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null) where TEvent:EventData
        {
            return backgroundJobManager.EnqueueAsync<EventTriggerAsyncBackgroundJob<TEvent>,TEvent>(e,priority,delay);
        }
    }
}