using System;
using System.Threading.Tasks;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Runtime.Session;

namespace Geev.Application.Features
{
    /// <summary>
    /// Default implementation for <see cref="IFeatureChecker"/>.
    /// </summary>
    public class FeatureChecker : IFeatureChecker, ITransientDependency, IIocManagerAccessor
    {
        /// <summary>
        /// Reference to the current session.
        /// </summary>
        public IGeevSession GeevSession { get; set; }

        /// <summary>
        /// Reference to the store used to get feature values.
        /// </summary>
        public IFeatureValueStore FeatureValueStore { get; set; }

        public IIocManager IocManager { get; set; }

        private readonly IFeatureManager _featureManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        /// <summary>
        /// Creates a new <see cref="FeatureChecker"/> object.
        /// </summary>
        public FeatureChecker(IFeatureManager featureManager, IMultiTenancyConfig multiTenancyConfig)
        {
            _featureManager = featureManager;
            _multiTenancyConfig = multiTenancyConfig;

            FeatureValueStore = NullFeatureValueStore.Instance;
            GeevSession = NullGeevSession.Instance;
        }

        /// <inheritdoc/>
        public Task<string> GetValueAsync(string name)
        {
            if (GeevSession.TenantId == null)
            {
                throw new GeevException("FeatureChecker can not get a feature value by name. TenantId is not set in the IGeevSession!");
            }

            return GetValueAsync(GeevSession.TenantId.Value, name);
        }

        /// <inheritdoc/>
        public async Task<string> GetValueAsync(int tenantId, string name)
        {
            var feature = _featureManager.Get(name);
            var value = await FeatureValueStore.GetValueOrNullAsync(tenantId, feature);
            
            return value ?? feature.DefaultValue;
        }

        /// <inheritdoc/>
        public async Task<bool> IsEnabledAsync(string featureName)
        {
            if (GeevSession.TenantId == null && _multiTenancyConfig.IgnoreFeatureCheckForHostUsers)
            {
                return true;
            }

            return string.Equals(await GetValueAsync(featureName), "true", StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public async Task<bool> IsEnabledAsync(int tenantId, string featureName)
        {
            if (_multiTenancyConfig.IgnoreFeatureCheckForHostUsers)
            {
                return true;
            }

            return string.Equals(await GetValueAsync(tenantId, featureName), "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}