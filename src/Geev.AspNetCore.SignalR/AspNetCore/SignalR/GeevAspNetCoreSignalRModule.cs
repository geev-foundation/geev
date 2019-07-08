using System.Reflection;
using Geev.AspNetCore.SignalR.Notifications;
using Geev.Modules;

namespace Geev.AspNetCore.SignalR
{
    /// <summary>
    /// ABP ASP.NET Core SignalR integration module.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevAspNetCoreSignalRModule : GeevModule
    {
        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Notifications.Notifiers.Add<SignalRRealTimeNotifier>();
        }
    }
}
