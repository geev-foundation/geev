using System.Threading.Tasks;
using Geev.AspNetCore.Configuration;
using Geev.Dependency;
using Geev.Domain.Uow;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Uow
{
    public class GeevUowPageFilter : IAsyncPageFilter, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGeevAspNetCoreConfiguration _aspnetCoreConfiguration;
        private readonly IUnitOfWorkDefaultOptions _unitOfWorkDefaultOptions;

        public GeevUowPageFilter(
            IUnitOfWorkManager unitOfWorkManager,
            IGeevAspNetCoreConfiguration aspnetCoreConfiguration,
            IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _aspnetCoreConfiguration = aspnetCoreConfiguration;
            _unitOfWorkDefaultOptions = unitOfWorkDefaultOptions;
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var unitOfWorkAttr = _unitOfWorkDefaultOptions
                                     .GetUnitOfWorkAttributeOrNull(context.HandlerMethod.MethodInfo) ??
                                 _aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

            if (unitOfWorkAttr.IsDisabled)
            {
                await next();
                return;
            }

            var uowOpts = new UnitOfWorkOptions
            {
                IsTransactional = unitOfWorkAttr.IsTransactional,
                IsolationLevel = unitOfWorkAttr.IsolationLevel,
                Timeout = unitOfWorkAttr.Timeout,
                Scope = unitOfWorkAttr.Scope
            };

            using (var uow = _unitOfWorkManager.Begin(uowOpts))
            {
                var result = await next();
                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync();
                }
            }
        }
    }
}