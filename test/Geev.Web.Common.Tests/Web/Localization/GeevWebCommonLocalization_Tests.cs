using Geev.Localization;
using Geev.TestBase;
using Shouldly;
using Xunit;

namespace Geev.Web.Common.Tests.Web.Localization
{
    public class GeevWebCommonLocalization_Tests : GeevIntegratedTestBase<GeevWebCommonTestModule>
    {
        private readonly ILocalizationManager _localizationManager;

        public GeevWebCommonLocalization_Tests()
        {
            _localizationManager = Resolve<ILocalizationManager>();
        }

        [Fact]
        public void Should_Localize_GeevWebCommon_Texts()
        {
            using (CultureInfoHelper.Use("en"))
            {
                _localizationManager
                    .GetSource(GeevWebConsts.LocalizaionSourceName)
                    .GetString("ValidationError")
                    .ShouldBe("Your request is not valid!");
            }
        }
    }
}
