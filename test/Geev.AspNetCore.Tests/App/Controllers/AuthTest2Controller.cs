using Geev.AspNetCore.Mvc.Authorization;
using Geev.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.Controllers
{
    [GeevMvcAuthorize]
    public class AuthTest2Controller : GeevController
    {
        [AllowAnonymous]
        public ActionResult NonAuthorizedAction()
        {
            return Content("public content 2");
        }
        
        public ActionResult AuthorizedAction()
        {
            return Content("secret content 2");
        }
    }
}