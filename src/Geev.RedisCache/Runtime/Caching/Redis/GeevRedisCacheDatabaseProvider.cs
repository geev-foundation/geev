using System;
using Geev.Dependency;
using StackExchange.Redis;

namespace Geev.Runtime.Caching.Redis
{
    /// <summary>
    /// Implements <see cref="IGeevRedisCacheDatabaseProvider"/>.
    /// </summary>
    public class GeevRedisCacheDatabaseProvider : IGeevRedisCacheDatabaseProvider, ISingletonDependency
    {
        private readonly GeevRedisCacheOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeevRedisCacheDatabaseProvider"/> class.
        /// </summary>
        public GeevRedisCacheDatabaseProvider(GeevRedisCacheOptions options)
        {
            _options = options;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }
    }
}
