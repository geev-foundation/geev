using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Geev.Aspects;
using Geev.AspNetCore.Configuration;
using Geev.AspNetCore.Mvc.Extensions;
using Geev.Auditing;
using Geev.Dependency;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Auditing
{
    public class GeevAuditPageFilter : IAsyncPageFilter, ITransientDependency
    {
        private readonly IGeevAspNetCoreConfiguration _configuration;
        private readonly IAuditingHelper _auditingHelper;

        public GeevAuditPageFilter(IGeevAspNetCoreConfiguration configuration, IAuditingHelper auditingHelper)
        {
            _configuration = configuration;
            _auditingHelper = auditingHelper;
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context))
            {
                await next();
                return;
            }

            using (GeevCrossCuttingConcerns.Applying(context.HandlerInstance, GeevCrossCuttingConcerns.Auditing))
            {
                var auditInfo = _auditingHelper.CreateAuditInfo(
                    context.HandlerInstance.GetType(),
                    context.HandlerMethod.MethodInfo,
                    context.GetBoundPropertiesAsDictionary()
                );

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var result = await next();
                    if (result.Exception != null && !result.ExceptionHandled)
                    {
                        auditInfo.Exception = result.Exception;
                    }
                }
                catch (Exception ex)
                {
                    auditInfo.Exception = ex;
                    throw;
                }
                finally
                {
                    stopwatch.Stop();
                    auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                    await _auditingHelper.SaveAsync(auditInfo);
                }
            }
        }

        private bool ShouldSaveAudit(PageHandlerExecutingContext actionContext)
        {
            return _configuration.IsAuditingEnabled &&
                   _auditingHelper.ShouldSaveAudit(actionContext.HandlerMethod.MethodInfo, true);
        }
    }
}
