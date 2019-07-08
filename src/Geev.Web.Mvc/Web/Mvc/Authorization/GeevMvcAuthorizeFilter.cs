using System.Net;
using System.Reflection;
using System.Web.Mvc;
using Geev.Authorization;
using Geev.Dependency;
using Geev.Events.Bus;
using Geev.Events.Bus.Exceptions;
using Geev.Logging;
using Geev.Web.Models;
using Geev.Web.Mvc.Controllers.Results;
using Geev.Web.Mvc.Extensions;
using Geev.Web.Mvc.Helpers;

namespace Geev.Web.Mvc.Authorization
{
    public class GeevMvcAuthorizeFilter : IAuthorizationFilter, ITransientDependency
    {
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IErrorInfoBuilder _errorInfoBuilder;
        private readonly IEventBus _eventBus;

        public GeevMvcAuthorizeFilter(
            IAuthorizationHelper authorizationHelper,
            IErrorInfoBuilder errorInfoBuilder,
            IEventBus eventBus)
        {
            _authorizationHelper = authorizationHelper;
            _errorInfoBuilder = errorInfoBuilder;
            _eventBus = eventBus;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return;
            }

            try
            {
                _authorizationHelper.Authorize(methodInfo, methodInfo.DeclaringType);
            }
            catch (GeevAuthorizationException ex)
            {
                LogHelper.Logger.Warn(ex.ToString(), ex);
                HandleUnauthorizedRequest(filterContext, methodInfo, ex);
            }
        }

        protected virtual void HandleUnauthorizedRequest(
            AuthorizationContext filterContext,
            MethodInfo methodInfo,
            GeevAuthorizationException ex)
        {
            filterContext.HttpContext.Response.StatusCode =
                filterContext.RequestContext.HttpContext.User?.Identity?.IsAuthenticated ?? false
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;

            var isJsonResult = MethodInfoHelper.IsJsonResult(methodInfo);

            if (isJsonResult)
            {
                filterContext.Result = CreateUnAuthorizedJsonResult(ex);
            }
            else
            {
                filterContext.Result = CreateUnAuthorizedNonJsonResult(filterContext, ex);
            }

            if (isJsonResult || filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }

            _eventBus.Trigger(this, new GeevHandledExceptionData(ex));
        }

        protected virtual GeevJsonResult CreateUnAuthorizedJsonResult(GeevAuthorizationException ex)
        {
            return new GeevJsonResult(
                new AjaxResponse(_errorInfoBuilder.BuildForException(ex), true))
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected virtual HttpStatusCodeResult CreateUnAuthorizedNonJsonResult(AuthorizationContext filterContext, GeevAuthorizationException ex)
        {
            return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, ex.Message);
        }
    }
}