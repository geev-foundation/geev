using Geev.AspNetCore.Mvc.Controllers;
using Geev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.Controllers
{
    public class WrapResultTestController : GeevController
    {
        [HttpGet]
        [Route("WrapResultTest/Get")]
        public int Get()
        {
            return 42;
        }

        [HttpGet]
        [Route("WrapResultTest/GetDontWrap")]
        [DontWrapResult]
        public int GetDontWrap()
        {
            return 42;
        }

        [HttpGet]
        [Route("WrapResultTest/GetXml")]
        [Produces("application/xml")]
        public int GetXml()
        {
            return 42;
        }
    }
}
