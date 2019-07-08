using System.Net;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.Mvc.Extensions;
using Geev.AspNetCore.Mvc.Results;
using Geev.Authorization;
using Geev.Dependency;
using Geev.Domain.Entities;
using Geev.Events.Bus;
using Geev.Events.Bus.Exceptions;
using Geev.Logging;
using Geev.Reflection;
using Geev.Runtime.Validation;
using Geev.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.ExceptionHandling
{
    public class GeevExceptionFilter : IExceptionFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public IEventBus EventBus { get; set; }

        private readonly IErrorInfoBuilder _errorInfoBuilder;
        private readonly IGeevAspNetCoreConfiguration _configuration;

        public GeevExceptionFilter(IErrorInfoBuilder errorInfoBuilder, IGeevAspNetCoreConfiguration configuration)
        {
            _errorInfoBuilder = errorInfoBuilder;
            _configuration = configuration;

            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            var wrapResultAttribute =
                ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                    context.ActionDescriptor.GetMethodInfo(),
                    _configuration.DefaultWrapResultAttribute
                );

            if (wrapResultAttribute.LogError)
            {
                LogHelper.LogException(Logger, context.Exception);
            }

            HandleAndWrapException(context, wrapResultAttribute);
        }

        protected virtual void HandleAndWrapException(ExceptionContext context, WrapResultAttribute wrapResultAttribute)
        {
            if (!ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = GetStatusCode(context, wrapResultAttribute.WrapOnError);

            if (!wrapResultAttribute.WrapOnError)
            {
                return;
            }

            context.Result = new ObjectResult(
                new AjaxResponse(
                    _errorInfoBuilder.BuildForException(context.Exception),
                    context.Exception is GeevAuthorizationException
                )
            );

            EventBus.Trigger(this, new GeevHandledExceptionData(context.Exception));

            context.Exception = null; //Handled!
        }

        protected virtual int GetStatusCode(ExceptionContext context, bool wrapOnError)
        {
            if (context.Exception is GeevAuthorizationException)
            {
                return context.HttpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            if (context.Exception is GeevValidationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return (int)HttpStatusCode.NotFound;
            }

            if (wrapOnError)
            {
                return (int)HttpStatusCode.InternalServerError;
            }

            return context.HttpContext.Response.StatusCode;
        }
    }
}
