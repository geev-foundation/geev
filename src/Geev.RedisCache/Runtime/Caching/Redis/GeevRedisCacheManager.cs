using Geev.Dependency;
using Geev.Runtime.Caching.Configuration;

namespace Geev.Runtime.Caching.Redis
{
    /// <summary>
    /// Used to create <see cref="GeevRedisCache"/> instances.
    /// </summary>
    public class GeevRedisCacheManager : CacheManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeevRedisCacheManager"/> class.
        /// </summary>
        public GeevRedisCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {
            IocManager.RegisterIfNot<GeevRedisCache>(DependencyLifeStyle.Transient);
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Resolve<GeevRedisCache>(new { name });
        }
    }
}
