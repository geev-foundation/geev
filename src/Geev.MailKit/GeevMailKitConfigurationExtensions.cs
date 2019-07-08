using Geev.Configuration.Startup;

namespace Geev.MailKit
{
    public static class GeevMailKitConfigurationExtensions
    {
        public static IGeevMailKitConfiguration GeevMailKit(this IModuleConfigurations configurations)
        {
            return configurations.GeevConfiguration.Get<IGeevMailKitConfiguration>();
        }
    }
}