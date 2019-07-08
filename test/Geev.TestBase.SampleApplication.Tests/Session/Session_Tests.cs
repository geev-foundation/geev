using Geev.Configuration.Startup;
using Geev.Runtime.Session;
using Shouldly;
using Xunit;

namespace Geev.TestBase.SampleApplication.Tests.Session
{
    public class Session_Tests : SampleApplicationTestBase
    {
        private readonly IGeevSession _session;

        public Session_Tests()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            _session = Resolve<IGeevSession>();
        }

        [Fact]
        public void Session_Override_Test()
        {
            _session.UserId.ShouldBeNull();
            _session.TenantId.ShouldBeNull();

            using (_session.Use(42, 571))
            {
                _session.TenantId.ShouldBe(42);
                _session.UserId.ShouldBe(571);

                using (_session.Use(null, 3))
                {
                    _session.TenantId.ShouldBeNull();
                    _session.UserId.ShouldBe(3);
                }

                _session.TenantId.ShouldBe(42);
                _session.UserId.ShouldBe(571);
            }

            _session.UserId.ShouldBeNull();
            _session.TenantId.ShouldBeNull();
        }
    }
}
