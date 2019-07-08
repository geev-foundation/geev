using Geev.Configuration.Startup;

namespace Geev.AutoMapper
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Geev.AutoMapper module.
    /// </summary>
    public static class GeevAutoMapperConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Geev.AutoMapper module.
        /// </summary>
        public static IGeevAutoMapperConfiguration GeevAutoMapper(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevAutoMapperConfiguration>();
        }
    }
}