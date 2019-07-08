using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using Geev.Collections.Extensions;
using Geev.Configuration;
using Geev.Dependency;
using Geev.Extensions;
using Geev.Localization;
using Geev.Runtime.Session;
using Geev.Timing;
using Geev.Web.Configuration;

namespace Geev.Web.Localization
{
    public class CurrentCultureSetter : ICurrentCultureSetter, ITransientDependency
    {
        private readonly IGeevWebLocalizationConfiguration _webLocalizationConfiguration;
        private readonly ISettingManager _settingManager;
        private readonly IGeevSession _geevSession;

        public CurrentCultureSetter(
            IGeevWebLocalizationConfiguration webLocalizationConfiguration,
            ISettingManager settingManager,
            IGeevSession geevSession)
        {
            _webLocalizationConfiguration = webLocalizationConfiguration;
            _settingManager = settingManager;
            _geevSession = geevSession;
        }

        public virtual void SetCurrentCulture(HttpContext httpContext)
        {
            if (IsCultureSpecifiedInGlobalizationConfig())
            {
                return;
            }

            // 1: Query String
            var culture = GetCultureFromQueryString(httpContext);
            if (culture != null)
            {
                SetCurrentCulture(culture);
                return;
            }

            // 2: User preference
            culture = GetCultureFromUserSetting();
            if (culture != null)
            {
                SetCurrentCulture(culture);
                return;
            }

            // 3 & 4: Header / Cookie
            culture = GetCultureFromHeader(httpContext) ?? GetCultureFromCookie(httpContext);
            if (culture != null)
            {
                if (_geevSession.UserId.HasValue)
                {
                    SetCultureToUserSetting(_geevSession.ToUserIdentifier(), culture);
                }

                SetCurrentCulture(culture);
                return;
            }

            // 5 & 6: Default / Browser
            culture = GetDefaultCulture() ?? GetBrowserCulture(httpContext);
            if (culture != null)
            {
                SetCurrentCulture(culture);
                SetCultureToCookie(httpContext, culture);
            }
        }

        private void SetCultureToUserSetting(UserIdentifier user, string culture)
        {
            _settingManager.ChangeSettingForUser(
                user,
                LocalizationSettingNames.DefaultLanguage,
                culture
            );
        }

        private string GetCultureFromUserSetting()
        {
            if (_geevSession.UserId == null)
            {
                return null;
            }

            var culture = _settingManager.GetSettingValueForUser(
                LocalizationSettingNames.DefaultLanguage,
                _geevSession.TenantId,
                _geevSession.UserId.Value,
                fallbackToDefault: false
            );

            if (culture.IsNullOrEmpty() || !GlobalizationHelper.IsValidCultureCode(culture))
            {
                return null;
            }

            return culture;
        }

        protected virtual bool IsCultureSpecifiedInGlobalizationConfig()
        {
            var globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
            if (globalizationSection == null || globalizationSection.UICulture.IsNullOrEmpty())
            {
                return false;
            }

            return !string.Equals(globalizationSection.UICulture, "auto", StringComparison.InvariantCultureIgnoreCase);
        }

        protected virtual string GetCultureFromCookie(HttpContext httpContext)
        {
            var culture = httpContext.Request.Cookies[_webLocalizationConfiguration.CookieName]?.Value;
            if (culture.IsNullOrEmpty() || !GlobalizationHelper.IsValidCultureCode(culture))
            {
                return null;
            }

            return culture;
        }

        protected virtual void SetCultureToCookie(HttpContext context, string culture)
        {
            context.Response.SetCookie(
                new HttpCookie(_webLocalizationConfiguration.CookieName, culture)
                {
                    Expires = Clock.Now.AddYears(2),
                    Path = context.Request.ApplicationPath
                }
            );
        }

        protected virtual string GetDefaultCulture()
        {
            var culture = _settingManager.GetSettingValue(LocalizationSettingNames.DefaultLanguage);
            if (culture.IsNullOrEmpty() || !GlobalizationHelper.IsValidCultureCode(culture))
            {
                return null;
            }

            return culture;
        }

        protected virtual string GetCultureFromHeader(HttpContext httpContext)
        {
            var culture = httpContext.Request.Headers[_webLocalizationConfiguration.CookieName];
            if (culture.IsNullOrEmpty() || !GlobalizationHelper.IsValidCultureCode(culture))
            {
                return null;
            }

            return culture;
        }

        protected virtual string GetBrowserCulture(HttpContext httpContext)
        {
            if (httpContext.Request.UserLanguages.IsNullOrEmpty())
            {
                return null;
            }

            return httpContext.Request?.UserLanguages?.FirstOrDefault(GlobalizationHelper.IsValidCultureCode);
        }

        protected virtual string GetCultureFromQueryString(HttpContext httpContext)
        {
            var culture = httpContext.Request.QueryString[_webLocalizationConfiguration.CookieName];
            if (culture.IsNullOrEmpty() || !GlobalizationHelper.IsValidCultureCode(culture))
            {
                return null;
            }

            return culture;
        }

        protected virtual void SetCurrentCulture(string language)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfoHelper.Get(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfoHelper.Get(language);
        }
    }
}