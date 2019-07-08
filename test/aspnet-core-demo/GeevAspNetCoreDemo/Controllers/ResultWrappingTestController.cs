using Geev.AspNetCore.Mvc.Controllers;
using Geev.Dependency;
using Geev.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GeevAspNetCoreDemo.Controllers
{
    public class ResultWrappingTestController : GeevController
    {
        public ResultWrappingTestController(IHostingEnvironment environment)
        {
            
        }

        [HttpGet]
        [Route("ResultWrappingTest/Get")]
        public int Get()
        {
            return 42;
        }

        [HttpGet]
        [Route("ResultWrappingTest/GetDontWrap")]
        [DontWrapResult]
        public int GetDontWrap()
        {
            return 42;
        }
    }

    [DontWrapResult]
    public class ResultWrappingTest2Controller : Controller, ITransientDependency
    {
        public ResultWrappingTest2Controller(IHostingEnvironment environment)
        {
            
        }

        [HttpGet]
        [Route("ResultWrappingTest2/Get")]
        public int Get()
        {
            return 43;
        }

        [HttpGet]
        [Route("ResultWrappingTest2/GetDontWrap")]
        //[DontWrapResult]
        public int GetDontWrap()
        {
            return 43;
        }
    }
}
