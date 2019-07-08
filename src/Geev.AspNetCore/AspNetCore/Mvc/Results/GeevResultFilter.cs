using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.Mvc.Extensions;
using Geev.AspNetCore.Mvc.Results.Wrapping;
using Geev.Dependency;
using Geev.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results
{
    public class GeevResultFilter : IResultFilter, ITransientDependency
    {
        private readonly IGeevAspNetCoreConfiguration _configuration;
        private readonly IGeevActionResultWrapperFactory _actionResultWrapperFactory;

        public GeevResultFilter(IGeevAspNetCoreConfiguration configuration, 
            IGeevActionResultWrapperFactory actionResultWrapper)
        {
            _configuration = configuration;
            _actionResultWrapperFactory = actionResultWrapper;
        }

        public virtual void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            var methodInfo = context.ActionDescriptor.GetMethodInfo();

            //var clientCacheAttribute = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
            //    methodInfo,
            //    _configuration.DefaultClientCacheAttribute
            //);

            //clientCacheAttribute?.Apply(context);
            
            var wrapResultAttribute =
                ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                    methodInfo,
                    _configuration.DefaultWrapResultAttribute
                );

            if (!wrapResultAttribute.WrapOnSuccess)
            {
                return;
            }

            _actionResultWrapperFactory.CreateFor(context).Wrap(context);
        }

        public virtual void OnResultExecuted(ResultExecutedContext context)
        {
            //no action
        }
    }
}
