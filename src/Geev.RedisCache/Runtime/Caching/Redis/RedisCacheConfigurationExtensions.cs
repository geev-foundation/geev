using System;
using Geev.Dependency;
using Geev.Runtime.Caching.Configuration;

namespace Geev.Runtime.Caching.Redis
{
    /// <summary>
    /// Extension methods for <see cref="ICachingConfiguration"/>.
    /// </summary>
    public static class RedisCacheConfigurationExtensions
    {
        /// <summary>
        /// Configures caching to use Redis as cache server.
        /// </summary>
        /// <param name="cachingConfiguration">The caching configuration.</param>
        public static void UseRedis(this ICachingConfiguration cachingConfiguration)
        {
            cachingConfiguration.UseRedis(options => { });
        }

        /// <summary>
        /// Configures caching to use Redis as cache server.
        /// </summary>
        /// <param name="cachingConfiguration">The caching configuration.</param>
        /// <param name="optionsAction">Ac action to get/set options</param>
        public static void UseRedis(this ICachingConfiguration cachingConfiguration, Action<GeevRedisCacheOptions> optionsAction)
        {
            var iocManager = cachingConfiguration.GeevConfiguration.IocManager;

            iocManager.RegisterIfNot<ICacheManager, GeevRedisCacheManager>();

            optionsAction(iocManager.Resolve<GeevRedisCacheOptions>());
        }
    }
}
