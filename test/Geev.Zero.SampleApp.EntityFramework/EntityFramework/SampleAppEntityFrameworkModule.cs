using System.Reflection;
using Geev.Modules;
using Geev.Zero.EntityFramework;

namespace Geev.Zero.SampleApp.EntityFramework
{
    [DependsOn(typeof(GeevZeroEntityFrameworkModule), typeof(SampleAppModule))]
    public class SampleAppEntityFrameworkModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
