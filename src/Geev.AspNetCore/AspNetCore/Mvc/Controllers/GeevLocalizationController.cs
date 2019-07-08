using Geev.AspNetCore.Mvc.Extensions;
using Geev.Auditing;
using Geev.Configuration;
using Geev.Localization;
using Geev.Runtime.Session;
using Geev.Timing;
using Geev.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.Mvc.Controllers
{
    public class GeevLocalizationController : GeevController
    {
        [DisableAuditing]
        public virtual ActionResult ChangeCulture(string cultureName, string returnUrl = "")
        {
            if (!GlobalizationHelper.IsValidCultureCode(cultureName))
            {
                throw new GeevException("Unknown language: " + cultureName + ". It must be a valid culture!");
            }

            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName, cultureName));

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                cookieValue,
                new CookieOptions
                {
                    Expires = Clock.Now.AddYears(2),
                    HttpOnly = true 
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
                return Json(new AjaxResponse());
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && GeevUrlHelper.IsLocalUrl(Request, returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/"); //TODO: Go to app root
        }
    }
}
