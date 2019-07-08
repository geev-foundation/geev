using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Geev.Collections.Extensions;
using Geev.Extensions;
using Geev.Runtime.Caching;
using Geev.UI;
using Geev.Web.Models;
using Geev.WebApi.Controllers;

namespace Geev.WebApi.Runtime.Caching
{
    [DontWrapResult]
    public class GeevCacheController : GeevApiController
    {
        private readonly ICacheManager _cacheManager;

        public GeevCacheController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        [HttpPost]
        public async Task<AjaxResponse> Clear(ClearCacheModel model)
        {
            if (model.Password.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Password can not be null or empty!");
            }

            if (model.Caches.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Caches can not be null or empty!");
            }

            await CheckPassword(model.Password);

            var caches = _cacheManager.GetAllCaches().Where(c => model.Caches.Contains(c.Name));
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }

            return new AjaxResponse();
        }

        [HttpPost]
        [Route("api/GeevCache/ClearAll")]
        public async Task<AjaxResponse> ClearAll(ClearAllCacheModel model)
        {
            if (model.Password.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Password can not be null or empty!");
            }

            await CheckPassword(model.Password);

            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }

            return new AjaxResponse();
        }

        private async Task CheckPassword(string password)
        {
            var actualPassword = await SettingManager.GetSettingValueAsync(ClearCacheSettingNames.Password);
            if (actualPassword != password)
            {
                throw new UserFriendlyException("Password is not correct!");
            }
        }
    }
}
