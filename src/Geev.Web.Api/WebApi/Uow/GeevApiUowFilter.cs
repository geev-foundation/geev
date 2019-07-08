using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.WebApi.Configuration;
using Geev.WebApi.Validation;

namespace Geev.WebApi.Uow
{
    public class GeevApiUowFilter : IActionFilter, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGeevWebApiConfiguration _webApiConfiguration;
        private readonly IUnitOfWorkDefaultOptions _unitOfWorkDefaultOptions;

        public bool AllowMultiple => false;

        public GeevApiUowFilter(
            IUnitOfWorkManager unitOfWorkManager,
            IGeevWebApiConfiguration webApiConfiguration, 
            IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _webApiConfiguration = webApiConfiguration;
            _unitOfWorkDefaultOptions = unitOfWorkDefaultOptions;
        }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            if (actionContext.ActionDescriptor.IsDynamicGeevAction())
            {
                return await continuation();
            }

            var unitOfWorkAttr = _unitOfWorkDefaultOptions.GetUnitOfWorkAttributeOrNull(methodInfo) ??
                                 _webApiConfiguration.DefaultUnitOfWorkAttribute;

            if (unitOfWorkAttr.IsDisabled)
            {
                return await continuation();
            }

            using (var uow = _unitOfWorkManager.Begin(unitOfWorkAttr.CreateOptions()))
            {
                var result = await continuation();
                await uow.CompleteAsync();
                return result;
            }
        }
    }
}