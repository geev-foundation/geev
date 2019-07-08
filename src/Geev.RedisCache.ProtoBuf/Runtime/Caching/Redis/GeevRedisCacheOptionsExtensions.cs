using Geev.Configuration.Startup;
using Geev.Dependency;

namespace Geev.Runtime.Caching.Redis
{
    public static class GeevRedisCacheOptionsExtensions
    {
        public static void UseProtoBuf(this GeevRedisCacheOptions options)
        {
            options.GeevStartupConfiguration
                .ReplaceService<IRedisCacheSerializer, ProtoBufRedisCacheSerializer>(DependencyLifeStyle.Transient);
        }
    }
}
