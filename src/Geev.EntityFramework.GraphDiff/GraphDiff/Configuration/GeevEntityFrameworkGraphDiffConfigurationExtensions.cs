using System.Collections.Generic;
using System.Linq;
using Geev.Configuration.Startup;
using Geev.EntityFramework.GraphDiff.Mapping;

namespace Geev.EntityFramework.GraphDiff.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Geev.EntityFramework.GraphDiff module.
    /// </summary>
    public static class GeevEntityFrameworkGraphDiffConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.EntityFramework.GraphDiff module.
        /// </summary>
        public static IGeevEntityFrameworkGraphDiffModuleConfiguration GeevEfGraphDiff(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevEntityFrameworkGraphDiffModuleConfiguration>();
        }

        /// <summary>
        /// Used to provide a mappings for the Geev.EntityFramework.GraphDiff module.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="entityMappings"></param>
        public static void UseMappings(this IGeevEntityFrameworkGraphDiffModuleConfiguration configuration, IEnumerable<EntityMapping> entityMappings)
        {
            configuration.EntityMappings = entityMappings.ToList();
        }
    }
}
