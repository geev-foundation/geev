using Geev.Dependency;
using Geev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.Controllers
{
    [DontWrapResult]
    public class DontWrapResultTestController : DontWrapResultTestControllerBase, ITransientDependency
    {
        [HttpGet]
        [Route("DontWrapResultTest/Get")]
        public int Get()
        {
            return 42;
        }
    }

    public abstract class DontWrapResultTestControllerBase : Controller
    {
        [HttpGet]
        [Route("DontWrapResultTest/GetBase")]
        public int GetBase()
        {
            return 42;
        }
    }
}