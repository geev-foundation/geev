using Geev.Configuration.Startup;

namespace Geev.Zero.Ldap.Configuration
{
    /// <summary>
    /// Extension methods for module zero configurations.
    /// </summary>
    public static class ModuleZeroLdapConfigurationExtensions
    {
        /// <summary>
        /// Configures ABP Zero LDAP module.
        /// </summary>
        /// <returns></returns>
        public static IGeevZeroLdapModuleConfig ZeroLdap(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.GeevConfiguration.Get<IGeevZeroLdapModuleConfig>();
        }
    }
}
