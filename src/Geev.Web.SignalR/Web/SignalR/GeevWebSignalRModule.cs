using System.Reflection;
using Geev.Modules;
using Geev.Web.SignalR.Notifications;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Geev.Web.SignalR
{
    /// <summary>
    /// ABP SignalR integration module.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevWebSignalRModule : GeevModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            GlobalHost.DependencyResolver = new WindsorDependencyResolver(IocManager.IocContainer);
            UseGeevSignalRContractResolver();

            Configuration.Notifications.Notifiers.Add<SignalRRealTimeNotifier>();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        private void UseGeevSignalRContractResolver()
        {
            var serializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    ContractResolver = new GeevSignalRContractResolver()
                });

            IocManager.IocContainer.Register(
                Component.For<JsonSerializer>().Instance(serializer)
            );
        }
    }
}
