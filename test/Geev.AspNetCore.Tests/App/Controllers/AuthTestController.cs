using Geev.AspNetCore.Mvc.Authorization;
using Geev.AspNetCore.Mvc.Controllers;
using Geev.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.Controllers
{
    public class AuthTestController : GeevController
    {
        public ActionResult NonAuthorizedAction()
        {
            return Content("public content");
        }

        [Authorize]
        public ActionResult AuthorizedAction()
        {
            return Content("secret content");
        }

        [GeevMvcAuthorize]
        public ActionResult GeevMvcAuthorizedAction()
        {
            return Content("secret content");
        }

        [GeevMvcAuthorize]
        public AjaxResponse GeevMvcAuthorizedActionReturnsObject()
        {
            return new AjaxResponse("OK");
        }
    }
}
