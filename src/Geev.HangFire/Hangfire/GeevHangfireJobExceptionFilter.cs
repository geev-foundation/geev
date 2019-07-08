using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geev.BackgroundJobs;
using Geev.Dependency;
using Geev.Events.Bus;
using Geev.Events.Bus.Exceptions;
using Hangfire.Common;
using Hangfire.Server;

namespace Geev.Hangfire
{
    public class GeevHangfireJobExceptionFilter : JobFilterAttribute, IServerFilter, ITransientDependency
    {
        public IEventBus EventBus { get; set; }

        public GeevHangfireJobExceptionFilter()
        {
            EventBus = NullEventBus.Instance;
        }

        public void OnPerforming(PerformingContext filterContext)
        {
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                EventBus.Trigger(
                    this,
                    new GeevHandledExceptionData(
                        new BackgroundJobException(
                            "A background job execution is failed on Hangfire. See inner exception for details. Use JobObject to get Hangfire background job object.",
                            filterContext.Exception
                        )
                        {
                            JobObject = filterContext.BackgroundJob
                        }
                    )
                );
            }
        }
    }
}
