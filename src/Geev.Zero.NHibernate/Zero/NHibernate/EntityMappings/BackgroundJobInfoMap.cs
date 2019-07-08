using Geev.BackgroundJobs;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public class BackgroundJobInfoMap : EntityMap<BackgroundJobInfo, long>
    {
        public BackgroundJobInfoMap()
            : base("GeevBackgroundJobs")
        {
            Map(x => x.JobType);
            Map(x => x.JobArgs);
            Map(x => x.TryCount);
            Map(x => x.NextTryTime);
            Map(x => x.LastTryTime);
            Map(x => x.IsAbandoned);
            Map(x => x.Priority).CustomType<BackgroundJobPriority>();

            this.MapCreationAudited();
        }
    }
}