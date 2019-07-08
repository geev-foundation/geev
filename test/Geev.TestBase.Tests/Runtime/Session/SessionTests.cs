using Geev.Configuration.Startup;
using Geev.Runtime.Session;
using Shouldly;
using Xunit;

namespace Geev.TestBase.Tests.Runtime.Session
{
    public class SessionTests : GeevIntegratedTestBase<GeevKernelModule>
    {
        [Fact]
        public void Should_Be_Default_On_Startup()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = false;

            GeevSession.UserId.ShouldBe(null);
            GeevSession.TenantId.ShouldBe(1);

            Resolve<IMultiTenancyConfig>().IsEnabled = true;

            GeevSession.UserId.ShouldBe(null);
            GeevSession.TenantId.ShouldBe(null);
        }

        [Fact]
        public void Can_Change_Session_Variables()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;

            GeevSession.UserId = 1;
            GeevSession.TenantId = 42;

            var resolvedGeevSession = LocalIocManager.Resolve<IGeevSession>();

            resolvedGeevSession.UserId.ShouldBe(1);
            resolvedGeevSession.TenantId.ShouldBe(42);

            Resolve<IMultiTenancyConfig>().IsEnabled = false;

            GeevSession.UserId.ShouldBe(1);
            GeevSession.TenantId.ShouldBe(1);
        }
    }
}
