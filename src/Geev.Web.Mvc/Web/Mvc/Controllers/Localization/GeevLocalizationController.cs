using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Geev.Auditing;
using Geev.Configuration;
using Geev.Localization;
using Geev.Runtime.Session;
using Geev.Timing;
using Geev.Web.Configuration;
using Geev.Web.Models;

namespace Geev.Web.Mvc.Controllers.Localization
{
    public class GeevLocalizationController : GeevController
    {
        private readonly IGeevWebLocalizationConfiguration _webLocalizationConfiguration;

        public GeevLocalizationController(IGeevWebLocalizationConfiguration webLocalizationConfiguration)
        {
            _webLocalizationConfiguration = webLocalizationConfiguration;
        }

        [DisableAuditing]
        public virtual ActionResult ChangeCulture(string cultureName, string returnUrl = "")
        {
            if (!GlobalizationHelper.IsValidCultureCode(cultureName))
            {
                throw new GeevException("Unknown language: " + cultureName + ". It must be a valid culture!");
            }

            Response.Cookies.Add(
                new HttpCookie(_webLocalizationConfiguration.CookieName, cultureName)
                {
                    Expires = Clock.Now.AddYears(2),
                    Path = Request.ApplicationPath
                }
            );

            if (GeevSession.UserId.HasValue)
            {
                SettingManager.ChangeSettingForUser(
                    GeevSession.ToUserIdentifier(),
                    LocalizationSettingNames.DefaultLanguage,
                    cultureName
                );
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new AjaxResponse(), JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Request.Url != null && GeevUrlHelper.IsLocalUrl(Request.Url, returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect(Request.ApplicationPath);
        }
    }
}
