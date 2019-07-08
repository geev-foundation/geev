using Geev.AspNetCore.Mvc.Controllers;

namespace GeevAspNetCoreDemo.Controllers
{
    public class DemoControllerBase : GeevController
    {
        public DemoControllerBase()
        {
            LocalizationSourceName = "GeevAspNetCoreDemoModule";
        }
    }
}
