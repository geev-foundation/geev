using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http.Controllers;
using Geev.Collections.Extensions;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Web.Validation;

namespace Geev.WebApi.Validation
{
    public class WebApiActionInvocationValidator : ActionInvocationValidatorBase
    {
        protected HttpActionContext ActionContext { get; private set; }

        public WebApiActionInvocationValidator(IValidationConfiguration configuration, IIocResolver iocResolver)
            : base(configuration, iocResolver)
        {
        }

        public void Initialize(HttpActionContext actionContext, MethodInfo methodInfo)
        {
            ActionContext = actionContext;

            base.Initialize(methodInfo);
        }

        protected override void SetDataAnnotationAttributeErrors()
        {
            foreach (var state in ActionContext.ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    ValidationErrors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
                }
            }
        }

        protected override object GetParameterValue(string parameterName)
        {
            return ActionContext.ActionArguments.GetOrDefault(parameterName);
        }
    }
}