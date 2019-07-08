using System;
using System.Threading.Tasks;
using Geev.Threading;

namespace Geev.BackgroundJobs
{
    public abstract class AsyncBackgroundJob<TArgs> : BackgroundJob<TArgs>
    {
        public override void Execute(TArgs args)
        {
            AsyncHelper.RunSync(() => ExecuteAsync(args));
        }

        protected abstract Task ExecuteAsync(TArgs args);
    }
}