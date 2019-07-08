using Geev.AutoMapper;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.TestBase;
using Geev.Zero.Configuration;
using Geev.Zero.Notifications;
using Geev.ZeroCore.SampleApp;
using Castle.MicroKernel.Registration;

namespace Geev.Zero
{
    [DependsOn(typeof(GeevZeroCoreSampleAppModule), typeof(GeevTestBaseModule))]
    public class GeevZeroTestModule : GeevModule
    {
        public GeevZeroTestModule(GeevZeroCoreSampleAppModule sampleAppModule)
        {
            sampleAppModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
            Configuration.Modules.GeevAutoMapper().UseStaticMapper = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            Configuration.UnitOfWork.IsTransactional = false;
            Configuration.Notifications.Distributers.Add<FakeNotificationDistributer>();
        }

        public override void Initialize()
        {
            TestServiceCollectionRegistrar.Register(IocManager);

            IocManager.IocContainer.Register(
                Component
                    .For<FakeNotificationDistributer>()
                    .LifestyleSingleton()
            );

            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroTestModule).GetAssembly());
        }
    }
}
