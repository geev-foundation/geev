using System.Web.Mvc;
using Geev.Dependency;
using Geev.Web.Mvc.Configuration;
using Geev.Web.Mvc.Extensions;

namespace Geev.Web.Mvc.Validation
{
    public class GeevMvcValidationFilter : IActionFilter, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly IGeevMvcConfiguration _configuration;

        public GeevMvcValidationFilter(IIocResolver iocResolver, IGeevMvcConfiguration configuration)
        {
            _iocResolver = iocResolver;
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!_configuration.IsValidationEnabledForControllers)
            {
                return;
            }

            var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return;
            }

            using (var validator = _iocResolver.ResolveAsDisposable<MvcActionInvocationValidator>())
            {
                validator.Object.Initialize(filterContext, methodInfo);
                validator.Object.Validate();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}
