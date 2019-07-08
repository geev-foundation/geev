using Geev.NHibernate.Configuration;

namespace Geev.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP NHibernate module.
    /// </summary>
    public static class GeevNHibernateConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP NHibernate module.
        /// </summary>
        public static IGeevNHibernateModuleConfiguration GeevNHibernate(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevNHibernateModuleConfiguration>();
        }
    }
}