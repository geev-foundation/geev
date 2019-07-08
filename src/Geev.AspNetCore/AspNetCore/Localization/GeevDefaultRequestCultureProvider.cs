using System.Threading.Tasks;
using Geev.Configuration;
using Geev.Extensions;
using Geev.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.AspNetCore.Localization
{
    public class GeevDefaultRequestCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var settingManager = httpContext.RequestServices.GetRequiredService<ISettingManager>();

            var culture = await settingManager.GetSettingValueAsync(LocalizationSettingNames.DefaultLanguage);

            if (culture.IsNullOrEmpty())
            {
                return null;
            }

            return new ProviderCultureResult(culture, culture);
        }
    }
}
