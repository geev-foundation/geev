using System.Globalization;
using System.Reflection;
using Geev.Localization;
using Geev.TestBase;
using Geev.Web.Localization;
using Shouldly;
using Xunit;

namespace Geev.Web.Tests.Localization
{
    public class GeevWebLocalizationTests : GeevIntegratedTestBase<GeevWebModule>
    {
        private readonly ILocalizationManager _localizationManager;

        public GeevWebLocalizationTests()
        {
            _localizationManager = Resolve<ILocalizationManager>();
        }

        [Fact]
        public void Should_Get_Localized_Strings()
        {
            var names = Assembly.GetAssembly(typeof(GeevWebModule)).GetManifestResourceNames();

            var source = _localizationManager.GetSource(GeevWebConsts.LocalizaionSourceName);
            source.GetString("Yes", new CultureInfo("en-US")).ShouldBe("Yes");
            source.GetString("Yes", new CultureInfo("tr-TR")).ShouldBe("Evet");
        }
    }
}
