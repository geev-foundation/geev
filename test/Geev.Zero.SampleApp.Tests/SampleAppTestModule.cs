using System;
using Geev.Modules;
using Geev.MultiTenancy;
using Geev.TestBase;
using Geev.Zero.Configuration;
using Geev.Zero.Ldap;
using Geev.Zero.SampleApp.EntityFramework;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Security;
using NSubstitute;

namespace Geev.Zero.SampleApp.Tests
{
    [DependsOn(
        typeof(SampleAppEntityFrameworkModule),
        typeof(GeevZeroLdapModule),
        typeof(GeevTestBaseModule))]
    public class SampleAppTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(2);
        }

        public override void Initialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IAuthenticationManager>().Instance(Substitute.For<IAuthenticationManager>())
            );
        }
    }
}