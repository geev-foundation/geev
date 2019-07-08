using System.Threading.Tasks;
using Geev.Application.Services;
using Geev.Aspects;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.Mvc.Extensions;
using Geev.Dependency;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Validation
{
    public class GeevValidationActionFilter : IAsyncActionFilter, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly IGeevAspNetCoreConfiguration _configuration;

        public GeevValidationActionFilter(IIocResolver iocResolver, IGeevAspNetCoreConfiguration configuration)
        {
            _iocResolver = iocResolver;
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!_configuration.IsValidationEnabledForControllers || !context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            using (GeevCrossCuttingConcerns.Applying(context.Controller, GeevCrossCuttingConcerns.Validation))
            {
                using (var validator = _iocResolver.ResolveAsDisposable<MvcActionInvocationValidator>())
                {
                    validator.Object.Initialize(context);
                    validator.Object.Validate();
                }

                await next();
            }
        }
    }
}
