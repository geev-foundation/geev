using System.Linq;
using System.Threading.Tasks;
using Geev.Configuration;
using Geev.Runtime.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Geev.Localization;
using Geev.Extensions;
using JetBrains.Annotations;

namespace Geev.AspNetCore.Localization
{
    public class GeevUserRequestCultureProvider : RequestCultureProvider
    {
        public CookieRequestCultureProvider CookieProvider { get; set; }
        public GeevLocalizationHeaderRequestCultureProvider HeaderProvider { get; set; }

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var geevSession = httpContext.RequestServices.GetRequiredService<IGeevSession>();
            if (geevSession.UserId == null)
            {
                return null;
            }

            var settingManager = httpContext.RequestServices.GetRequiredService<ISettingManager>();

            var culture = await settingManager.GetSettingValueForUserAsync(
                LocalizationSettingNames.DefaultLanguage,
                geevSession.TenantId,
                geevSession.UserId.Value,
                fallbackToDefault: false
            );

            if (!culture.IsNullOrEmpty())
            {
                return new ProviderCultureResult(culture, culture);
            }

            var result = await GetResultOrNull(httpContext, CookieProvider) ??
                         await GetResultOrNull(httpContext, HeaderProvider);

            if (result == null || !result.Cultures.Any())
            {
                return null;
            }

            //Try to set user's language setting from cookie if available.
            await settingManager.ChangeSettingForUserAsync(
                geevSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                result.Cultures.First().Value
            );

            return result;
        }

        protected virtual async Task<ProviderCultureResult> GetResultOrNull([NotNull] HttpContext httpContext, [CanBeNull] IRequestCultureProvider provider)
        {
            if (provider == null)
            {
                return null;
            }

            return await provider.DetermineProviderCultureResult(httpContext);
        }
    }
}
