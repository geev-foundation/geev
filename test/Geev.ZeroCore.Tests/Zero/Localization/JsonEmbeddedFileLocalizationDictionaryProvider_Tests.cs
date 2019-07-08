using Geev.Localization;
using Geev.Localization.Dictionaries;
using Geev.Localization.Dictionaries.Json;
using Geev.Localization.Dictionaries.Xml;
using Geev.Localization.Sources;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.TestBase;
using Shouldly;
using Xunit;

namespace Geev.Zero.Localization
{
    public class JsonEmbeddedFileLocalizationDictionaryProvider_Tests : GeevIntegratedTestBase<MyCustomJsonLangModule>
    {
        [Fact]
        public void Test_Json_Override()
        {
            var mananger = LocalIocManager.Resolve<ILocalizationManager>();

            using (CultureInfoHelper.Use("en"))
            {
                var geevSource = mananger.GetSource(GeevConsts.LocalizationSourceName);
                geevSource.GetString("TimeZone").ShouldBe("Time-zone");

                var geevZeroSource = mananger.GetSource(GeevZeroConsts.LocalizationSourceName);
                geevZeroSource.GetString("Email").ShouldBe("Email address");
            }
        }
    }

    public class MyCustomJsonLangModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Clear();

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GeevConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomJsonLangModule).GetAssembly(),
                        "Geev.Zero.Localization.Sources.Base.Geev"
                    )
                )
            );

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GeevZeroConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomJsonLangModule).GetAssembly(),
                        "Geev.Zero.Localization.Sources.Base.GeevZero"
                    )
                )
            );

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    GeevConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomJsonLangModule).GetAssembly(), "Geev.Zero.Localization.Sources.Extensions.Json.Geev"
                    )
                )
            );

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    GeevZeroConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomJsonLangModule).GetAssembly(), "Geev.Zero.Localization.Sources.Extensions.Json.GeevZero"
                    )
                )
            );
        }
    }
}
