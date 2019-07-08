using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Geev.Dependency;
using Geev.Web.Security.AntiForgery;
using Geev.WebApi.Configuration;
using Geev.WebApi.Controllers.Dynamic.Selectors;
using Geev.WebApi.Validation;
using Castle.Core.Logging;

namespace Geev.WebApi.Security.AntiForgery
{
    public class GeevAntiForgeryApiFilter : IAuthorizationFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public bool AllowMultiple => false;

        private readonly IGeevAntiForgeryManager _geevAntiForgeryManager;
        private readonly IGeevWebApiConfiguration _webApiConfiguration;
        private readonly IGeevAntiForgeryWebConfiguration _antiForgeryWebConfiguration;

        public GeevAntiForgeryApiFilter(
            IGeevAntiForgeryManager geevAntiForgeryManager, 
            IGeevWebApiConfiguration webApiConfiguration,
            IGeevAntiForgeryWebConfiguration antiForgeryWebConfiguration)
        {
            _geevAntiForgeryManager = geevAntiForgeryManager;
            _webApiConfiguration = webApiConfiguration;
            _antiForgeryWebConfiguration = antiForgeryWebConfiguration;
            Logger = NullLogger.Instance;
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            if (!_geevAntiForgeryManager.ShouldValidate(_antiForgeryWebConfiguration, methodInfo, actionContext.Request.Method.ToHttpVerb(), _webApiConfiguration.IsAutomaticAntiForgeryValidationEnabled))
            {
                return await continuation();
            }

            if (!_geevAntiForgeryManager.IsValid(actionContext.Request.Headers))
            {
                return CreateErrorResponse(actionContext, "Empty or invalid anti forgery header token.");
            }

            return await continuation();
        }

        protected virtual HttpResponseMessage CreateErrorResponse(HttpActionContext actionContext, string reason)
        {
            Logger.Warn(reason);
            Logger.Warn("Requested URI: " + actionContext.Request.RequestUri);
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = reason };
        }
    }
}