using Geev.Web.Security.AntiForgery;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Geev.AspNetCore.Security.AntiForgery
{
    public class GeevAspNetCoreAntiForgeryManager : IGeevAntiForgeryManager
    {
        public IGeevAntiForgeryConfiguration Configuration { get; }

        private readonly IAntiforgery _antiforgery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeevAspNetCoreAntiForgeryManager(
            IAntiforgery antiforgery,
            IHttpContextAccessor httpContextAccessor,
            IGeevAntiForgeryConfiguration configuration)
        {
            Configuration = configuration;
            _antiforgery = antiforgery;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken()
        {
            return _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken;
        }
    }
}