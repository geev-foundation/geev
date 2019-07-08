using System;
using System.Diagnostics;
using System.Web.Mvc;
using Geev.Auditing;
using Geev.Dependency;
using Geev.Web.Mvc.Configuration;
using Geev.Web.Mvc.Extensions;

namespace Geev.Web.Mvc.Auditing
{
    public class GeevMvcAuditFilter : IActionFilter, ITransientDependency
    {
        private readonly IGeevMvcConfiguration _configuration;
        private readonly IAuditingHelper _auditingHelper;

        public GeevMvcAuditFilter(IGeevMvcConfiguration configuration, IAuditingHelper auditingHelper)
        {
            _configuration = configuration;
            _auditingHelper = auditingHelper;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!ShouldSaveAudit(filterContext))
            {
                GeevAuditFilterData.Set(filterContext.HttpContext, null);
                return;
            }

            var auditInfo = _auditingHelper.CreateAuditInfo(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerType,
                filterContext.ActionDescriptor.GetMethodInfoOrNull(),
                filterContext.ActionParameters
            );

            var actionStopwatch = Stopwatch.StartNew();

            GeevAuditFilterData.Set(
                filterContext.HttpContext,
                new GeevAuditFilterData(
                    actionStopwatch,
                    auditInfo
                )
            );
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var auditData = GeevAuditFilterData.GetOrNull(filterContext.HttpContext);
            if (auditData == null)
            {
                return;
            }

            auditData.Stopwatch.Stop();

            auditData.AuditInfo.ExecutionDuration = Convert.ToInt32(auditData.Stopwatch.Elapsed.TotalMilliseconds);
            auditData.AuditInfo.Exception = filterContext.Exception;

            _auditingHelper.Save(auditData.AuditInfo);
        }

        private bool ShouldSaveAudit(ActionExecutingContext filterContext)
        {
            var currentMethodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
            if (currentMethodInfo == null)
            {
                return false;
            }

            if (_configuration == null)
            {
                return false;
            }

            if (!_configuration.IsAuditingEnabled)
            {
                return false;
            }

            if (filterContext.IsChildAction && !_configuration.IsAuditingEnabledForChildActions)
            {
                return false;
            }

            return _auditingHelper.ShouldSaveAudit(currentMethodInfo, true);
        }
    }
}
