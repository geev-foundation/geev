using Geev.AspNetCore.Mvc.Auditing;
using Geev.AspNetCore.Mvc.Authorization;
using Geev.AspNetCore.Mvc.Conventions;
using Geev.AspNetCore.Mvc.ExceptionHandling;
using Geev.AspNetCore.Mvc.ModelBinding;
using Geev.AspNetCore.Mvc.Results;
using Geev.AspNetCore.Mvc.Uow;
using Geev.AspNetCore.Mvc.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.AspNetCore.Mvc
{
    internal static class GeevMvcOptionsExtensions
    {
        public static void AddGeev(this MvcOptions options, IServiceCollection services)
        {
            AddConventions(options, services);
            AddActionFilters(options);
            AddPageFilters(options);
            AddModelBinders(options);
        }

        private static void AddConventions(MvcOptions options, IServiceCollection services)
        {
            options.Conventions.Add(new GeevAppServiceConvention(services));
        }

        private static void AddActionFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(GeevAuthorizationFilter));
            options.Filters.AddService(typeof(GeevAuditActionFilter));
            options.Filters.AddService(typeof(GeevValidationActionFilter));
            options.Filters.AddService(typeof(GeevUowActionFilter));
            options.Filters.AddService(typeof(GeevExceptionFilter));
            options.Filters.AddService(typeof(GeevResultFilter));
        }

        private static void AddPageFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(GeevUowPageFilter));
            options.Filters.AddService(typeof(GeevAuditPageFilter));
        }

        private static void AddModelBinders(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new GeevDateTimeModelBinderProvider());
        }
    }
}