using System.Threading.Tasks;
using Geev.Configuration.Startup;
using Geev.MultiTenancy;
using Geev.Runtime.Remoting;
using Geev.Runtime.Session;
using Geev.TestBase.Runtime.Session;
using Geev.Tests.Application.Navigation;
using Geev.Web.Navigation;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Geev.Web.Tests.Navigation
{
    public class NavigationScript_Tests
    {
        [Fact]
        public async Task Should_Get_Script()
        {
            var testCase = new NavigationTestCase();
            var scriptManager = new NavigationScriptManager(testCase.UserNavigationManager)
            {
                GeevSession = CreateTestGeevSession()
            };

            var script = await scriptManager.GetScriptAsync();
            script.ShouldNotBeNullOrEmpty();
        }

        private static TestGeevSession CreateTestGeevSession()
        {
            return new TestGeevSession(
                new MultiTenancyConfig { IsEnabled = true },
                new DataContextAmbientScopeProvider<SessionOverride>(
                    new AsyncLocalAmbientDataContext()
                ),
                Substitute.For<ITenantResolver>()
            );
        }
    }
}
