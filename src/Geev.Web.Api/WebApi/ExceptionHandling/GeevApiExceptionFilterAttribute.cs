using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Geev.Dependency;
using Geev.Domain.Entities;
using Geev.Events.Bus;
using Geev.Events.Bus.Exceptions;
using Geev.Extensions;
using Geev.Logging;
using Geev.Runtime.Session;
using Geev.Runtime.Validation;
using Geev.Web.Models;
using Geev.WebApi.Configuration;
using Geev.WebApi.Controllers;
using Castle.Core.Logging;

namespace Geev.WebApi.ExceptionHandling
{
    /// <summary>
    /// Used to handle exceptions on web api controllers.
    /// </summary>
    public class GeevApiExceptionFilterAttribute : ExceptionFilterAttribute, ITransientDependency
    {
        /// <summary>
        /// Reference to the <see cref="ILogger"/>.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the <see cref="IEventBus"/>.
        /// </summary>
        public IEventBus EventBus { get; set; }

        public IGeevSession GeevSession { get; set; }

        protected IGeevWebApiConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeevApiExceptionFilterAttribute"/> class.
        /// </summary>
        public GeevApiExceptionFilterAttribute(IGeevWebApiConfiguration configuration)
        {
            Configuration = configuration;
            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
            GeevSession = NullGeevSession.Instance;
        }

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            var wrapResultAttribute = HttpActionDescriptorHelper
                .GetWrapResultAttributeOrNull(context.ActionContext.ActionDescriptor) ??
                Configuration.DefaultWrapResultAttribute;

            if (wrapResultAttribute.LogError)
            {
                LogHelper.LogException(Logger, context.Exception);
            }

            if (!wrapResultAttribute.WrapOnError)
            {
                context.Response.StatusCode = GetStatusCode(context, wrapResultAttribute.WrapOnError);
                return;
            }

            if (IsIgnoredUrl(context.Request.RequestUri))
            {
                return;
            }

            if (context.Exception is HttpException)
            {
                var httpException = context.Exception as HttpException;
                var httpStatusCode = (HttpStatusCode) httpException.GetHttpCode();

                context.Response = context.Request.CreateResponse(
                    httpStatusCode,
                    new AjaxResponse(
                        new ErrorInfo(httpException.Message),
                        httpStatusCode == HttpStatusCode.Unauthorized || httpStatusCode == HttpStatusCode.Forbidden
                    )
                );
            }
            else
            {
                context.Response = context.Request.CreateResponse(
                    GetStatusCode(context, wrapResultAttribute.WrapOnError),
                    new AjaxResponse(
                        SingletonDependency<IErrorInfoBuilder>.Instance.BuildForException(context.Exception),
                        context.Exception is Geev.Authorization.GeevAuthorizationException)
                );
            }

            EventBus.Trigger(this, new GeevHandledExceptionData(context.Exception));
        }

        protected virtual HttpStatusCode GetStatusCode(HttpActionExecutedContext context, bool wrapOnError)
        {
            if (context.Exception is Geev.Authorization.GeevAuthorizationException)
            {
                return GeevSession.UserId.HasValue
                    ? HttpStatusCode.Forbidden
                    : HttpStatusCode.Unauthorized;
            }

            if (context.Exception is GeevValidationException)
            {
                return HttpStatusCode.BadRequest;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return HttpStatusCode.NotFound;
            }

            if (wrapOnError)
            {
                return HttpStatusCode.InternalServerError;
            }

            return context.Response.StatusCode;
        }

        protected virtual bool IsIgnoredUrl(Uri uri)
        {
            if (uri == null || uri.AbsolutePath.IsNullOrEmpty())
            {
                return false;
            }

            return Configuration.ResultWrappingIgnoreUrls.Any(url => uri.AbsolutePath.StartsWith(url));
        }
    }
}