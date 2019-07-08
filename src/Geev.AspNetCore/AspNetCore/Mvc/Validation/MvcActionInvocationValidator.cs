using System.ComponentModel.DataAnnotations;
using Geev.AspNetCore.Mvc.Extensions;
using Geev.Collections.Extensions;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Web.Validation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Validation
{
    public class MvcActionInvocationValidator : ActionInvocationValidatorBase
    {
        protected ActionExecutingContext ActionContext { get; private set; }

        public MvcActionInvocationValidator(IValidationConfiguration configuration, IIocResolver iocResolver)
            : base(configuration, iocResolver)
        {
        }

        public void Initialize(ActionExecutingContext actionContext)
        {
            ActionContext = actionContext;

            base.Initialize(actionContext.ActionDescriptor.GetMethodInfo());
        }

        protected override object GetParameterValue(string parameterName)
        {
            return ActionContext.ActionArguments.GetOrDefault(parameterName);
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
    }
}