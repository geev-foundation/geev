using System.Reflection;
using Geev.Modules;
using Geev.Zero.EntityFrameworkCore;

namespace Geev.Zero.SampleApp.EntityFrameworkCore
{
    [DependsOn(typeof(GeevZeroEntityFrameworkCoreModule), typeof(SampleAppModule))]
    public class SampleAppEntityFrameworkCoreModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
