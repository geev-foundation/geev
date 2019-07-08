using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Extensions;
using Geev.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace Geev.AspNetCore.MultiTenancy
{
    public class HttpCookieTenantResolveContributor : ITenantResolveContributor, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public HttpCookieTenantResolveContributor(
            IHttpContextAccessor httpContextAccessor, 
            IMultiTenancyConfig multiTenancyConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _multiTenancyConfig = multiTenancyConfig;
        }

        public int? ResolveTenantId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var tenantIdValue = httpContext.Request.Cookies[_multiTenancyConfig.TenantIdResolveKey];
            if (tenantIdValue.IsNullOrEmpty())
            {
                return null;
            }

            return int.TryParse(tenantIdValue, out var tenantId) ? tenantId : (int?) null;
        }
    }
}
