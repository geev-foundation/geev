using System.Linq;
using System.Reflection;
using Geev.Application.Services;
using Geev.AspNetCore.Configuration;
using Geev.Collections.Extensions;
using Geev.Dependency;
using Geev.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Geev.AspNetCore.Mvc.Providers
{
    /// <summary>
    /// Used to add application services as controller.
    /// </summary>
    public class GeevAppServiceControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IIocResolver _iocResolver;

        public GeevAppServiceControllerFeatureProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            var type = typeInfo.AsType();

            if (!typeof(IApplicationService).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }

            var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(typeInfo);

            if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
            {
                return false;
            }

            var settings = _iocResolver.Resolve<GeevAspNetCoreConfiguration>().ControllerAssemblySettings.GetSettings(type);
            return settings.Any(setting => setting.TypePredicate(type));
        }
    }
}