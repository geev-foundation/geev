using System.Reflection;
using Geev.Localization.Dictionaries.Xml;
using Geev.Localization.Sources;
using Geev.Modules;
using Geev.Zero.Ldap.Configuration;

namespace Geev.Zero.Ldap
{
    /// <summary>
    /// This module extends module zero to add LDAP authentication.
    /// </summary>
    [DependsOn(typeof (GeevZeroCommonModule))]
    public class GeevZeroLdapModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevZeroLdapModuleConfig, GeevZeroLdapModuleConfig>();

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    GeevZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Geev.Zero.Ldap.Localization.Source")
                    )
                );

            Configuration.Settings.Providers.Add<LdapSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
