using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Geev.Configuration.Startup;

namespace Geev.Runtime.Caching.Configuration
{
    internal class CachingConfiguration : ICachingConfiguration
    {
        public IGeevStartupConfiguration GeevConfiguration { get; private set; }

        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }
        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration(IGeevStartupConfiguration geevConfiguration)
        {
            GeevConfiguration = geevConfiguration;

            _configurators = new List<ICacheConfigurator>();
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}