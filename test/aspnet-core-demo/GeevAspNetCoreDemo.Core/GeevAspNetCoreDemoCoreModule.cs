using System.Reflection;
using Geev.AutoMapper;
using Geev.Localization;
using Geev.Localization.Dictionaries;
using Geev.Localization.Dictionaries.Json;
using Geev.Modules;
using Geev.Reflection.Extensions;

namespace GeevAspNetCoreDemo.Core
{
    [DependsOn(typeof(GeevAutoMapperModule))]
    public class GeevAspNetCoreDemoCoreModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", isDefault: true));
            Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe"));

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource("GeevAspNetCoreDemoModule",
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GeevAspNetCoreDemoCoreModule).GetAssembly(),
                        "GeevAspNetCoreDemo.Core.Localization.SourceFiles"
                    )
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevAspNetCoreDemoCoreModule).GetAssembly());
        }
    }
}