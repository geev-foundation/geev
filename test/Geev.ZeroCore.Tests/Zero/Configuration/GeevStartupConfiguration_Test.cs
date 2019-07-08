using Geev.Configuration.Startup;
using Shouldly;
using Xunit;

namespace Geev.Zero.Configuration
{
    public class GeevStartupConfiguration_Test : GeevZeroTestBase
    {
        private readonly IGeevStartupConfiguration _startupConfiguration;

        public GeevStartupConfiguration_Test()
        {
            _startupConfiguration = Resolve<IGeevStartupConfiguration>();
        }

        [Fact]
        public void Should_Get_Custom_Config_Providers()
        {
            var providers = _startupConfiguration.CustomConfigProviders;

            providers.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Should_Get_Custom_Config_Providers_Values()
        {
            _startupConfiguration.GetCustomConfig().Count.ShouldBeGreaterThan(0);
        }
    }
}
