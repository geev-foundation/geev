using Geev.Authorization;
using Geev.ZeroCore.SampleApp.Core;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Geev.Zero.Users
{
    public class SecurityStampValidator_Tests : GeevZeroTestBase
    {
        [Fact]
        public void Should_Resolve_GeevSecurityStampValidator()
        {
            (Resolve<ISecurityStampValidator>() is GeevSecurityStampValidator<Tenant, Role, User>).ShouldBeTrue();
            (Resolve<SecurityStampValidator<User>>() is GeevSecurityStampValidator<Tenant, Role, User>).ShouldBeTrue();
        }
    }
}