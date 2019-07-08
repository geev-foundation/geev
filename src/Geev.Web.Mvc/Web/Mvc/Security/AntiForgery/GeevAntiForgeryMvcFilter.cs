using System.Net;
using System.Reflection;
using System.Web.Mvc;
using Geev.Dependency;
using Geev.Web.Models;
using Geev.Web.Mvc.Configuration;
using Geev.Web.Mvc.Controllers.Results;
using Geev.Web.Mvc.Extensions;
using Geev.Web.Mvc.Helpers;
using Geev.Web.Security.AntiForgery;
using Castle.Core.Logging;

namespace Geev.Web.Mvc.Security.AntiForgery
{
    public class GeevAntiForgeryMvcFilter: IAuthorizationFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IGeevAntiForgeryManager _geevAntiForgeryManager;
        private readonly IGeevMvcConfiguration _mvcConfiguration;
        private readonly IGeevAntiForgeryWebConfiguration _antiForgeryWebConfiguration;

        public GeevAntiForgeryMvcFilter(
            IGeevAntiForgeryManager geevAntiForgeryManager, 
            IGeevMvcConfiguration mvcConfiguration,
            IGeevAntiForgeryWebConfiguration antiForgeryWebConfiguration)
        {
            _geevAntiForgeryManager = geevAntiForgeryManager;
            _mvcConfiguration = mvcConfiguration;
            _antiForgeryWebConfiguration = antiForgeryWebConfiguration;
            Logger = NullLogger.Instance;
        }

        public void OnAuthorization(AuthorizationContext context)
        {
            var methodInfo = context.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return;
            }

            var httpVerb = HttpVerbHelper.Create(context.HttpContext.Request.HttpMethod);
            if (!_geevAntiForgeryManager.ShouldValidate(_antiForgeryWebConfiguration, methodInfo, httpVerb, _mvcConfiguration.IsAutomaticAntiForgeryValidationEnabled))
            {
                return;
            }

            if (!_geevAntiForgeryManager.IsValid(context.HttpContext))
            {
                CreateErrorResponse(context, methodInfo, "Empty or invalid anti forgery header token.");
            }
        }

        private void CreateErrorResponse(
            AuthorizationContext context, 
            MethodInfo methodInfo, 
            string message)
        {
            Logger.Warn(message);
            Logger.Warn("Requested URI: " + context.HttpContext.Request.Url);

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.HttpContext.Response.StatusDescription = message;

            var isJsonResult = MethodInfoHelper.IsJsonResult(methodInfo);

            if (isJsonResult)
            {
                context.Result = CreateUnAuthorizedJsonResult(message);
            }
            else
            {
                context.Result = CreateUnAuthorizedNonJsonResult(context, message);
            }

            if (isJsonResult || context.HttpContext.Request.IsAjaxRequest())
            {
                context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }

        protected virtual GeevJsonResult CreateUnAuthorizedJsonResult(string message)
        {
            return new GeevJsonResult(new AjaxResponse(new ErrorInfo(message), true))
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected virtual HttpStatusCodeResult CreateUnAuthorizedNonJsonResult(AuthorizationContext filterContext, string message)
        {
            return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, message);
        }
    }
}
