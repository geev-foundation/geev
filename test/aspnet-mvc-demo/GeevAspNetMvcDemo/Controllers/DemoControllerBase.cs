using Geev.IdentityFramework;
using Geev.Web.Mvc.Controllers;
using Microsoft.AspNet.Identity;

namespace GeevAspNetMvcDemo.Controllers
{
    public abstract class DemoControllerBase : GeevController
    {
        protected DemoControllerBase()
        {
            LocalizationSourceName = "GeevAspNetMvcDemoModule";
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}