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
    public class XmlEmbeddedFileLocalizationDictionaryProvider_Tests : GeevIntegratedTestBase<MyCustomXmlLangModule>
    {
        [Fact]
        public void Test_Xml_Override()
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

    public class MyCustomXmlLangModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GeevZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GeevZeroCommonModule).GetAssembly(), "Geev.Zero.Localization.Source"
                    )
                )
            );

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    GeevConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomXmlLangModule).GetAssembly(), "Geev.Zero.Localization.Sources.Extensions.Xml.Geev"
                    )
                )
            );

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    GeevZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomXmlLangModule).GetAssembly(), "Geev.Zero.Localization.Sources.Extensions.Xml.GeevZero"
                    )
                )
            );
        }
    }
}
