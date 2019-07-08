using Geev.Dependency;
using Geev.Configuration.Startup;
using Geev.Modules;
using Geev.Net.Mail;
using Geev.Reflection.Extensions;

namespace Geev.MailKit
{
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevMailKitModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevMailKitConfiguration, GeevMailKitConfiguration>();
            Configuration.ReplaceService<IEmailSender, MailKitEmailSender>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevMailKitModule).GetAssembly());
        }
    }
}
