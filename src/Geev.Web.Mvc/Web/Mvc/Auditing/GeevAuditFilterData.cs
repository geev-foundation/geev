using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Geev.Auditing;

namespace Geev.Web.Mvc.Auditing
{
    public class GeevAuditFilterData
    {
        private const string GeevAuditFilterDataHttpContextKey = "__GeevAuditFilterData";

        public Stopwatch Stopwatch { get; }

        public AuditInfo AuditInfo { get; }

        public GeevAuditFilterData(
            Stopwatch stopwatch,
            AuditInfo auditInfo)
        {
            Stopwatch = stopwatch;
            AuditInfo = auditInfo;
        }

        public static void Set(HttpContextBase httpContext, GeevAuditFilterData auditFilterData)
        {
            GetAuditDataStack(httpContext).Push(auditFilterData);
        }

        public static GeevAuditFilterData GetOrNull(HttpContextBase httpContext)
        {
            var stack = GetAuditDataStack(httpContext);
            return stack.Count <= 0
                ? null
                : stack.Pop();
        }

        private static Stack<GeevAuditFilterData> GetAuditDataStack(HttpContextBase httpContext)
        {
            var stack = httpContext.Items[GeevAuditFilterDataHttpContextKey] as Stack<GeevAuditFilterData>;

            if (stack == null)
            {
                stack = new Stack<GeevAuditFilterData>();
                httpContext.Items[GeevAuditFilterDataHttpContextKey] = stack;
            }

            return stack;
        }
    }
}