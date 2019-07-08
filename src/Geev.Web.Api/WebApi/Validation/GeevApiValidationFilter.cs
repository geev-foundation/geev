using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Geev.Dependency;
using Geev.WebApi.Configuration;

namespace Geev.WebApi.Validation
{
    public class GeevApiValidationFilter : IActionFilter, ITransientDependency
    {
        public bool AllowMultiple => false;

        private readonly IIocResolver _iocResolver;
        private readonly IGeevWebApiConfiguration _configuration;

        public GeevApiValidationFilter(IIocResolver iocResolver, IGeevWebApiConfiguration configuration)
        {
            _iocResolver = iocResolver;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (!_configuration.IsValidationEnabledForControllers)
            {
                return await continuation();
            }

            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            /* ModelState.IsValid is being checked to handle parameter binding errors (ex: send string for an int value).
             * These type of errors can not be catched from application layer. */

            if (actionContext.ModelState.IsValid
                && actionContext.ActionDescriptor.IsDynamicGeevAction())
            {
                return await continuation();
            }

            using (var validator = _iocResolver.ResolveAsDisposable<WebApiActionInvocationValidator>())
            {
                validator.Object.Initialize(actionContext, methodInfo);
                validator.Object.Validate();
            }

            return await continuation();
        }
    }
}