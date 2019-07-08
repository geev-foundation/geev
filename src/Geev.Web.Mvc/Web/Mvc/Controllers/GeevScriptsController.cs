using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Geev.Auditing;
using Geev.Extensions;
using Geev.Localization;
using Geev.Web.Authorization;
using Geev.Web.Features;
using Geev.Web.Localization;
using Geev.Web.Minifier;
using Geev.Web.MultiTenancy;
using Geev.Web.Navigation;
using Geev.Web.Security;
using Geev.Web.Sessions;
using Geev.Web.Settings;
using Geev.Web.Timing;

namespace Geev.Web.Mvc.Controllers
{
    /// <summary>
    /// This controller is used to create client side scripts
    /// to work with ABP.
    /// </summary>
    public class GeevScriptsController : GeevController
    {
        private readonly IMultiTenancyScriptManager _multiTenancyScriptManager;
        private readonly ISettingScriptManager _settingScriptManager;
        private readonly INavigationScriptManager _navigationScriptManager;
        private readonly ILocalizationScriptManager _localizationScriptManager;
        private readonly IAuthorizationScriptManager _authorizationScriptManager;
        private readonly IFeaturesScriptManager _featuresScriptManager;
        private readonly ISessionScriptManager _sessionScriptManager;
        private readonly ITimingScriptManager _timingScriptManager;
        private readonly ISecurityScriptManager _securityScriptManager;
        private readonly IJavaScriptMinifier _javaScriptMinifier;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GeevScriptsController(
            IMultiTenancyScriptManager multiTenancyScriptManager,
            ISettingScriptManager settingScriptManager,
            INavigationScriptManager navigationScriptManager,
            ILocalizationScriptManager localizationScriptManager,
            IAuthorizationScriptManager authorizationScriptManager,
            IFeaturesScriptManager featuresScriptManager,
            ISessionScriptManager sessionScriptManager, 
            ITimingScriptManager timingScriptManager,
            ISecurityScriptManager securityScriptManager, 
            IJavaScriptMinifier javaScriptMinifier)
        {
            _multiTenancyScriptManager = multiTenancyScriptManager;
            _settingScriptManager = settingScriptManager;
            _navigationScriptManager = navigationScriptManager;
            _localizationScriptManager = localizationScriptManager;
            _authorizationScriptManager = authorizationScriptManager;
            _featuresScriptManager = featuresScriptManager;
            _sessionScriptManager = sessionScriptManager;
            _timingScriptManager = timingScriptManager;
            _securityScriptManager = securityScriptManager;
            _javaScriptMinifier = javaScriptMinifier;
        }

        /// <summary>
        /// Gets all needed scripts.
        /// </summary>
        [DisableAuditing]
        public async Task<ActionResult> GetScripts(string culture = "", bool minify = false)
        {
            if (!culture.IsNullOrEmpty())
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfoHelper.Get(culture);
            }

            var sb = new StringBuilder();

            sb.AppendLine(_multiTenancyScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(_sessionScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(_localizationScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(await _featuresScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(await _authorizationScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(await _navigationScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(await _settingScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(await _timingScriptManager.GetScriptAsync());
            sb.AppendLine();

            sb.AppendLine(_securityScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(GetTriggerScript());

            return Content(minify ? _javaScriptMinifier.Minify(sb.ToString()) : sb.ToString(),
                "application/x-javascript", Encoding.UTF8);
        }

        private static string GetTriggerScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    geev.event.trigger('geev.dynamicScriptsInitialized');");
            script.Append("})();");

            return script.ToString();
        }
    }
}
