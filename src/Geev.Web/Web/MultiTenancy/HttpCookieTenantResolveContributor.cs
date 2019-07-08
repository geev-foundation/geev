using System.Web;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Extensions;
using Geev.MultiTenancy;

namespace Geev.Web.MultiTenancy
{
    public class HttpCookieTenantResolveContributor : ITenantResolveContributor, ITransientDependency
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public HttpCookieTenantResolveContributor(IMultiTenancyConfig multiTenancyConfig)
        {
            _multiTenancyConfig = multiTenancyConfig;
        }

        public int? ResolveTenantId()
        {
            var cookie = HttpContext.Current?.Request.Cookies[_multiTenancyConfig.TenantIdResolveKey];
            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return null;
            }

            return int.TryParse(cookie.Value, out var tenantId) ? tenantId : (int?) null;
        }
    }
}
