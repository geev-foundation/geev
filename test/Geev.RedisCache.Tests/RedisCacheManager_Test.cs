using System;
using Geev.Configuration.Startup;
using Geev.Runtime.Caching;
using Geev.Runtime.Caching.Configuration;
using Geev.Runtime.Caching.Redis;
using Geev.Tests;
using Castle.MicroKernel.Registration;
using NSubstitute;
using Xunit;
using Shouldly;

namespace Geev.RedisCache.Tests
{
    public class RedisCacheManager_Test : TestBaseWithLocalIocManager
    {
        private readonly ITypedCache<string, MyCacheItem> _cache;

        public RedisCacheManager_Test()
        {
            LocalIocManager.Register<GeevRedisCacheOptions>();
            LocalIocManager.Register<ICachingConfiguration, CachingConfiguration>();
            LocalIocManager.Register<IGeevRedisCacheDatabaseProvider, GeevRedisCacheDatabaseProvider>();
            LocalIocManager.Register<ICacheManager, GeevRedisCacheManager>();
            LocalIocManager.Register<IRedisCacheSerializer,DefaultRedisCacheSerializer>();
            LocalIocManager.IocContainer.Register(Component.For<IGeevStartupConfiguration>().Instance(Substitute.For<IGeevStartupConfiguration>()));

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            LocalIocManager.Resolve<ICachingConfiguration>().Configure("MyTestCacheItems", cache =>
            {
                cache.DefaultSlidingExpireTime = defaultSlidingExpireTime;
            });

            _cache = LocalIocManager.Resolve<ICacheManager>().GetCache<string, MyCacheItem>("MyTestCacheItems");
            _cache.DefaultSlidingExpireTime.ShouldBe(defaultSlidingExpireTime);
        }

        //[Theory]
        //[InlineData("A", 42)]
        //[InlineData("B", 43)]
        public void Simple_Get_Set_Test(string cacheKey, int cacheValue)
        {
            var item = _cache.Get(cacheKey, () => new MyCacheItem { Value = cacheValue });

            item.ShouldNotBe(null);
            item.Value.ShouldBe(cacheValue);

            _cache.GetOrDefault(cacheKey).Value.ShouldBe(cacheValue);
        }

        [Serializable]
        public class MyCacheItem
        {
            public int Value { get; set; }
        }
    }
}
