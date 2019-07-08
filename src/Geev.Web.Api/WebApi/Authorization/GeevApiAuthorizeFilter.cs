using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Geev.Authorization;
using Geev.Dependency;
using Geev.Events.Bus;
using Geev.Events.Bus.Exceptions;
using Geev.Localization;
using Geev.Logging;
using Geev.Web;
using Geev.Web.Models;
using Geev.WebApi.Configuration;
using Geev.WebApi.Controllers;
using Geev.WebApi.Validation;

namespace Geev.WebApi.Authorization
{
    public class GeevApiAuthorizeFilter : IAuthorizationFilter, ITransientDependency
    {
        public bool AllowMultiple => false;

        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IGeevWebApiConfiguration _configuration;
        private readonly ILocalizationManager _localizationManager;
        private readonly IEventBus _eventBus;

        public GeevApiAuthorizeFilter(
            IAuthorizationHelper authorizationHelper, 
            IGeevWebApiConfiguration configuration,
            ILocalizationManager localizationManager,
            IEventBus eventBus)
        {
            _authorizationHelper = authorizationHelper;
            _configuration = configuration;
            _localizationManager = localizationManager;
            _eventBus = eventBus;
        }

        public virtual async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return await continuation();
            }
            
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            if (actionContext.ActionDescriptor.IsDynamicGeevAction())
            {
                return await continuation();
            }

            try
            {
                await _authorizationHelper.AuthorizeAsync(methodInfo, methodInfo.DeclaringType);
                return await continuation();
            }
            catch (GeevAuthorizationException ex)
            {
                LogHelper.Logger.Warn(ex.ToString(), ex);
                _eventBus.Trigger(this, new GeevHandledExceptionData(ex));
                return CreateUnAuthorizedResponse(actionContext);
            }
        }

        protected virtual HttpResponseMessage CreateUnAuthorizedResponse(HttpActionContext actionContext)
        {
            var statusCode = GetUnAuthorizedStatusCode(actionContext);

            var wrapResultAttribute =
                HttpActionDescriptorHelper.GetWrapResultAttributeOrNull(actionContext.ActionDescriptor) ??
                _configuration.DefaultWrapResultAttribute;

            if (!wrapResultAttribute.WrapOnError)
            {
                return new HttpResponseMessage(statusCode);
            }

            return new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<AjaxResponse>(
                    new AjaxResponse(
                        GetUnAuthorizedErrorMessage(statusCode),
                        true
                    ),
                    _configuration.HttpConfiguration.Formatters.JsonFormatter
                )
            };
        }

        private ErrorInfo GetUnAuthorizedErrorMessage(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
            {
                return new ErrorInfo(
                    _localizationManager.GetString(GeevWebConsts.LocalizaionSourceName, "DefaultError403"),
                    _localizationManager.GetString(GeevWebConsts.LocalizaionSourceName, "DefaultErrorDetail403")
                );
            }

            return new ErrorInfo(
                _localizationManager.GetString(GeevWebConsts.LocalizaionSourceName, "DefaultError401"),
                _localizationManager.GetString(GeevWebConsts.LocalizaionSourceName, "DefaultErrorDetail401")
            );
        }

        private static HttpStatusCode GetUnAuthorizedStatusCode(HttpActionContext actionContext)
        {
            return (actionContext.RequestContext.Principal?.Identity?.IsAuthenticated ?? false)
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }
    }
}