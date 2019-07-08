using System.Reflection;
using Geev.Modules;
using Geev.Web;

namespace Geev.Owin
{
    /// <summary>
    /// OWIN integration module for ABP.
    /// </summary>
    [DependsOn(typeof (GeevWebCommonModule))]
    public class GeevOwinModule : GeevModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
