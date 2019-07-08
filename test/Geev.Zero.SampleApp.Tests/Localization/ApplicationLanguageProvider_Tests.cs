using System.Linq;
using Geev.Localization;
using Geev.Zero.SampleApp.MultiTenancy;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Localization
{
    public class ApplicationLanguageProvider_Tests : SampleAppTestBase
    {
        private readonly ApplicationLanguageProvider _languageProvider;
        private readonly Tenant _defaultTenant;

        public ApplicationLanguageProvider_Tests()
        {
            _defaultTenant = GetDefaultTenant();
            _languageProvider = Resolve<ApplicationLanguageProvider>();
        }

        [Fact]
        public void Should_Get_Languages_For_Host()
        {
            //Arrange
            GeevSession.TenantId = null;

            //Act
            var languages = _languageProvider.GetLanguages();

            //Assert
            languages.Count.ShouldBe(3);
            languages.FirstOrDefault(l => l.Name == "en").ShouldNotBeNull();
            languages.FirstOrDefault(l => l.Name == "tr").ShouldNotBeNull();
            languages.FirstOrDefault(l => l.Name == "de").ShouldNotBeNull();
        }

        [Fact]
        public void Should_Get_Languages_For_Tenant()
        {
            //Arrange
            GeevSession.TenantId = _defaultTenant.Id;

            //Act
            var languages = _languageProvider.GetLanguages();

            //Assert
            languages.Count.ShouldBe(4);
            languages.FirstOrDefault(l => l.Name == "en").ShouldNotBeNull();
            languages.FirstOrDefault(l => l.Name == "tr").ShouldNotBeNull();
            languages.FirstOrDefault(l => l.Name == "de").ShouldNotBeNull();
            languages.FirstOrDefault(l => l.Name == "zh-Hans").ShouldNotBeNull();
        }
    }
}
