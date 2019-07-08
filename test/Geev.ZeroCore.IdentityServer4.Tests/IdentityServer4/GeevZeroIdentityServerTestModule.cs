using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.Zero;
using Geev.ZeroCore.SampleApp.Core;
using Geev.ZeroCore.SampleApp.EntityFramework;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.IdentityServer4
{
    [DependsOn(typeof(GeevZeroCoreIdentityServerEntityFrameworkCoreModule), typeof(GeevZeroTestModule))]
    public class GeevZeroIdentityServerTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            var services = new ServiceCollection();

            services.AddGeevIdentity<Tenant, User, Role>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddGeevPersistedGrants<SampleAppDbContext>()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddGeevIdentityServer<User>()
                .AddProfileService<GeevProfileService<User>>();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                IocManager.IocContainer,
                services
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroIdentityServerTestModule).GetAssembly());
        }
    }
}
