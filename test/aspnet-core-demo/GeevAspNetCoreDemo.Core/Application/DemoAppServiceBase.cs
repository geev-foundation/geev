using Geev.Application.Services;

namespace GeevAspNetCoreDemo.Core.Application
{
    public class DemoAppServiceBase : ApplicationService
    {
        public DemoAppServiceBase()
        {
            LocalizationSourceName = "GeevAspNetCoreDemoModule";
        }
    }
}
