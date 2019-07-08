using Geev.Localization;
using Geev.Localization.Dictionaries;
using Geev.Localization.Dictionaries.Json;
using Geev.Localization.Dictionaries.Xml;
using Geev.Localization.Sources;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Shouldly;
using Xunit;

namespace Geev.Tests.Localization.Json
{
    public class JsonAndXmlSourceMixing_Tests : TestBaseWithLocalIocManager
    {
        private readonly GeevBootstrapper _bootstrapper;

        public JsonAndXmlSourceMixing_Tests()
        {
            LocalIocManager.Register<ILanguageManager, LanguageManager>();
            LocalIocManager.Register<ILanguageProvider, DefaultLanguageProvider>();

            _bootstrapper = GeevBootstrapper.Create<MyLangModule>(options =>
            {
                options.IocManager = LocalIocManager;
            });

            _bootstrapper.Initialize();
        }

        [Fact]
        public void Test_Xml_Json()
        {
            var mananger = LocalIocManager.Resolve<LocalizationManager>();

            using (CultureInfoHelper.Use("en"))
            {
                var source = mananger.GetSource("Lang");

                source.GetString("Apple").ShouldBe("Apple");
                source.GetString("Banana").ShouldBe("Banana");
                source.GetString("ThisIsATest").ShouldBe("This is a test.");
                source.GetString("HowAreYou").ShouldBe("How are you?");
            }

            using (CultureInfoHelper.Use("zh-Hans"))
            {
                var source = mananger.GetSource("Lang");

                source.GetString("Apple").ShouldBe("苹果");
                source.GetString("Banana").ShouldBe("香蕉");
                source.GetString("ThisIsATest").ShouldBe("这是一个测试.");
                source.GetString("HowAreYou").ShouldBe("你好吗?");
            }
        }
    }

    public class MyLangModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    "Lang",
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyLangModule).GetAssembly(),
                         "Geev.Tests.Localization.Json.XmlSources"
                        )
                    )
                );

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    "Lang",
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyLangModule).GetAssembly(),
                         "Geev.Tests.Localization.Json.JsonSources"
                        )));

            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MyLangModule).GetAssembly());
        }
    }
}
